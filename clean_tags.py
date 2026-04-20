#!/usr/bin/env python3
"""
Remove non-location tags from all photo markdown files.
Run from project root: python3 clean_tags.py
"""

import os, re, glob

# ── Canonical location tags to KEEP (case-insensitive match) ──────────────────
LOCATION_TAGS = {
    "wisconsin",
    "minneapolis", "mpls",
    "california",
    "new york city", "nyc",
    "lake superior",
    "los angeles", "losangeles",
    "chicago",
    "germany",
    "caribbean",
    "north loop", "northloop",
}

# Normalise for comparison
LOCATION_TAGS_LOWER = {t.lower().strip() for t in LOCATION_TAGS}


def normalize(tag: str) -> str:
    return tag.lower().strip()


def is_location(tag: str) -> bool:
    return normalize(tag) in LOCATION_TAGS_LOWER


def parse_tags_line(tags_line: str) -> list[str]:
    """Extract individual tags from the YAML tags: line."""
    value = tags_line[len("tags:"):].strip()
    # Strip outer brackets
    value = value.strip("[]")
    if not value:
        return []
    # Split on comma, strip quotes and whitespace
    tags = []
    for part in value.split(","):
        tag = part.strip().strip('"').strip("'").strip()
        if tag:
            tags.append(tag)
    return tags


def rebuild_tags_line(tags: list[str]) -> str:
    if not tags:
        return "tags: []"
    inner = ", ".join(f'"{t}"' for t in tags)
    return f"tags: [{inner}]"


def process_file(path: str) -> tuple[int, int]:
    """Returns (original_count, kept_count)."""
    with open(path, encoding="utf-8") as f:
        content = f.read()

    lines = content.splitlines(keepends=True)
    changed = False
    orig_count = 0
    kept_count = 0

    new_lines = []
    for line in lines:
        stripped = line.rstrip("\n").rstrip("\r")
        if stripped.startswith("tags:"):
            original_tags = parse_tags_line(stripped)
            kept_tags = [t for t in original_tags if is_location(t)]
            orig_count = len(original_tags)
            kept_count = len(kept_tags)
            if set(original_tags) != set(kept_tags):
                changed = True
                new_line = rebuild_tags_line(kept_tags) + "\n"
                new_lines.append(new_line)
            else:
                new_lines.append(line)
        else:
            new_lines.append(line)

    if changed:
        with open(path, "w", encoding="utf-8") as f:
            f.writelines(new_lines)

    return orig_count, kept_count


def main():
    md_files = glob.glob(
        "wwwroot/photos/**/*.md", recursive=True
    )

    total_removed = 0
    files_changed = 0

    for path in sorted(md_files):
        orig, kept = process_file(path)
        removed = orig - kept
        if removed:
            files_changed += 1
            total_removed += removed
            print(f"  {path}: {orig} → {kept} tags ({removed} removed)")

    print(f"\nDone. {total_removed} tags removed across {files_changed} files.")


if __name__ == "__main__":
    main()
