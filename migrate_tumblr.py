#!/usr/bin/env python3
"""
Migrate minnepixel.tumblr.com photo posts to the local photoblog format.
Run from the project root: python migrate_tumblr.py
"""

import os
import re
import json
import time
import requests
from datetime import datetime, timezone
from collections import defaultdict
from html.parser import HTMLParser

BLOG = "minnepixel"
BASE_URL = f"https://{BLOG}.tumblr.com/api/read/json"
PHOTOS_DIR = os.path.join("wwwroot", "photos")
NUM_PER_PAGE = 50


# ── Helpers ──────────────────────────────────────────────────────────────────

class _HTMLStripper(HTMLParser):
    def __init__(self):
        super().__init__()
        self.fed = []

    def handle_data(self, d):
        self.fed.append(d)

    def get_data(self):
        return "".join(self.fed)


def strip_html(html):
    s = _HTMLStripper()
    s.feed(html)
    return re.sub(r"\s+", " ", s.get_data()).strip()


def slugify(text):
    text = text.lower()
    text = re.sub(r"[^\w\s-]", "", text)
    text = re.sub(r"[\s_]+", "-", text)
    text = re.sub(r"-+", "-", text)
    return text.strip("-")


def get_extension(url):
    path = url.split("?")[0]
    ext = os.path.splitext(path)[1].lower()
    return ext if ext in {".jpg", ".jpeg", ".png", ".gif", ".webp"} else ".jpg"


def best_photo_url(obj):
    """Return the highest-resolution photo URL from a post or photo object."""
    for size in ("photo-url-1280", "photo-url-500", "photo-url-400", "photo-url-250"):
        url = obj.get(size, "")
        if url:
            return url
    return ""


def download(url, dest):
    if os.path.exists(dest):
        return True
    try:
        r = requests.get(url, timeout=60, stream=True)
        r.raise_for_status()
        with open(dest, "wb") as f:
            for chunk in r.iter_content(8192):
                f.write(chunk)
        return True
    except Exception as e:
        print(f"    ERROR downloading {url}: {e}")
        return False


# ── API fetching ──────────────────────────────────────────────────────────────

def fetch_page(start, num=NUM_PER_PAGE):
    url = f"{BASE_URL}?type=photo&num={num}&start={start}"
    r = requests.get(url, timeout=30)
    r.raise_for_status()
    text = r.text.strip()
    # Strip JSONP wrapper: "var tumblr_api_read = {...};"
    if text.startswith("var tumblr_api_read"):
        text = text.split("=", 1)[1].strip().rstrip(";")
    return json.loads(text)


def fetch_all_posts():
    data = fetch_page(start=0, num=1)
    total = int(data["posts-total"])
    print(f"Total posts to migrate: {total}")

    posts = []
    start = 0
    while start < total:
        print(f"  Fetching {start}–{min(start + NUM_PER_PAGE, total) - 1} …")
        data = fetch_page(start)
        batch = data.get("posts", [])
        if not batch:
            break
        posts.extend(batch)
        start += NUM_PER_PAGE
        time.sleep(0.4)

    print(f"Fetched {len(posts)} posts\n")
    return posts


# ── Per-post processing ───────────────────────────────────────────────────────

def parse_date(post):
    """Return YYYY-MM-DD string from post, using unix-timestamp for accuracy."""
    ts = post.get("unix-timestamp")
    if ts:
        return datetime.fromtimestamp(int(ts), tz=timezone.utc).strftime("%Y-%m-%d")
    date_str = post.get("date-gmt") or post.get("date", "")
    for fmt in ("%Y-%m-%d %H:%M:%S %Z", "%a, %d %b %Y %H:%M:%S"):
        try:
            return datetime.strptime(date_str[:25].strip(), fmt).strftime("%Y-%m-%d")
        except ValueError:
            continue
    return None


def photos_from_post(post):
    """Return list of photo URLs for a post (handles single and multi-photo)."""
    photos_arr = post.get("photos", [])
    if photos_arr:
        return [best_photo_url(p) for p in photos_arr if best_photo_url(p)]
    url = best_photo_url(post)
    return [url] if url else []


def caption_text(post):
    raw = post.get("photo-caption", "") or ""
    return strip_html(raw)


# ── Markdown generation ───────────────────────────────────────────────────────

def escape_yaml(text):
    return text.replace('"', "'")


def build_md(date_key, rows, description, tags, body):
    tags_yaml = "[" + ", ".join(f'"{t}"' for t in tags) + "]" if tags else "[]"
    rows_yaml = "\n".join(f"  - [{r}]" for r in rows)
    desc = escape_yaml(description[:120] + "…" if len(description) > 120 else description)
    return (
        f'---\n'
        f'date: "{date_key}"\n'
        f'description: "{desc}"\n'
        f'tags: {tags_yaml}\n'
        f'rows:\n{rows_yaml}\n'
        f'---\n\n'
        f'{body.strip()}\n'
    )


# ── Main ──────────────────────────────────────────────────────────────────────

def main():
    posts = fetch_all_posts()

    # Group by date (merge same-day posts)
    by_date = defaultdict(list)
    for post in posts:
        date_key = parse_date(post)
        if date_key:
            by_date[date_key].append(post)
        else:
            print(f"  Skipping post {post.get('id')} — could not parse date")

    print(f"Unique dates: {len(by_date)}\n")

    for date_key in sorted(by_date):
        posts_on_day = by_date[date_key]
        folder = os.path.join(PHOTOS_DIR, date_key)
        os.makedirs(folder, exist_ok=True)

        rows = []
        captions = []
        all_tags = []

        for post in posts_on_day:
            slug = post.get("slug") or slugify(caption_text(post)[:50]) or date_key
            caption = caption_text(post)
            photo_urls = photos_from_post(post)
            tags = post.get("tags", [])

            if caption:
                captions.append(caption)
            for tag in tags:
                if tag not in all_tags:
                    all_tags.append(tag)

            for i, url in enumerate(photo_urls):
                ext = get_extension(url)
                filename = f"{slug}{ext}" if len(photo_urls) == 1 else f"{slug}-{i + 1}{ext}"
                dest = os.path.join(folder, filename)
                print(f"  [{date_key}] {filename}")
                download(url, dest)
                alt = escape_yaml(caption) if caption else slug.replace("-", " ")
                rows.append(f"{filename} | {alt}")

        if not rows:
            print(f"  [{date_key}] No photos — skipping\n")
            continue

        description = captions[0] if captions else date_key
        body = "\n\n".join(captions)
        md_path = os.path.join(folder, f"{date_key}.md")

        with open(md_path, "w", encoding="utf-8") as f:
            f.write(build_md(date_key, rows, description, all_tags, body))

        print(f"  → wrote {md_path}\n")

    print("Migration complete.")


if __name__ == "__main__":
    main()
