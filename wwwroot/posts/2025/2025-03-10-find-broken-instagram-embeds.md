---
title: "Find Broken Instagram Embed Links"
description: "With the help of ChatGPT I wrote a Python script that iterates through a website's sitemap to report which Instagram embeds are broken."
date: "2025-03-10"
draft: false
slug: "find-broken-instagram-embeds"
tags: python
---

 <section>    
    <p>My wife works for an organization that regulary posts articles to their website that include embeded Instagram images. As it is, sometimes the users change their privacy settings or delete the image. When this happens Instagram returns a generic "Missing image" message instead of the image as the embed.</p>
    <p>As she told me they need to constantly monitor each of the articles throughout the year to determine if any of their Instagram embeds are broken my brain started to think of ways to automate this process. What I ended up with was a nice python script that will find broken Instagram embed links on a website and report them to the users.
    </p>
    <p class="text-center">
        <img src="/images/2025/instagram_missing_embed.png" alt="Missing Instagram Embed Message" />
    <p>
    <p>
        To do this I enlisted ChatGPT to speed up the process as this wasn't something I had a lot of time to dedicated a lot of development time to. The core functionality is:
        <ul>
            <li>Accept a website's XML sitemap as an input</li>
            <li>Crawls all the pages and extracts Instagram embeded iFrames</li>
            <li>Checks if the embed is still loading correctly</li>
            <li>Returns a report listing the broken embeds and their parent urls</li>
        </ul>
    </p>
    <p>
        The implementation is pretty simple as you can see. The script asks for the website sitemap xml url and from there it iterates over each node of the XML output. If there are children nodes off the parent the script will also iterate over those. For each node the sript will render the webpage using BeautifulSoup then inspect the HTML to determine if there is an Instagram embed. If if there is an embed the script looks at the possible messages that are returned if the embed is broken. This can change in the future if Instagram updates how they return a broken embed link. But for now the script isn't do anything revolutionary, it's just checking if the embed is returning a set of messages that indicate the embed is broken. If any of those messages are returned, then that webpage gets added to the list.
    </p>
    <p>
        With the help of ChatGPT I was able to spin up this script below in under an hour. It wasn't perfect the first couple times around and I definitly needed to refactor the output to get what I wanted, but overall the script does it's job and will return a list of broken Instagram embeds within a website. 
    </p>
    <p>
        View the full code on <a href="https://github.com/clintmcmahon/instagram-embed-checker/blob/main/instagram_embed_checker.py">Github here</a>
    </p>
    <pre lang="python"><code class="language-py">
import requests
from bs4 import BeautifulSoup
import xml.etree.ElementTree as ET

class InstagramEmbedChecker:
    def __init__(self, sitemap_url):
        self.sitemap_url = sitemap_url
        self.pages_to_scan = set()
        self.broken_pages = set()

        # Patterns of sitemaps to skip
        self.skip_patterns = [
            "/example-1","example-2"
        ]

    def fetch_xml(self, url):
        """ Fetch an XML sitemap and return its parsed root. """
        try:
            response = requests.get(url, headers={"User-Agent": "Mozilla/5.0"}, timeout=10)
            response.raise_for_status()
            return ET.fromstring(response.text)
        except requests.RequestException as e:
            print(f"❌ Failed to fetch XML: {url} | Error: {e}")
            return None

    def should_skip_sitemap(self, url):
        """ Check if the sitemap should be skipped based on its URL. """
        return any(url.startswith(pattern) for pattern in self.skip_patterns)

    def parse_sitemap(self, url):
        """ Recursively parse a sitemap (handling both direct URLs and nested sitemaps). """
        if self.should_skip_sitemap(url):
            print(f"⏭️  Skipping sitemap: {url}")
            return
        
        print(f"Fetching sitemap: {url}")
        root = self.fetch_xml(url)
        if root is None:
            return

        namespace = {'ns': 'http://www.sitemaps.org/schemas/sitemap/0.9'}

        for elem in root.findall(".//ns:loc", namespace):
            page_url = elem.text.strip()

            if page_url.endswith(".xml"):  # It's a nested sitemap
                self.parse_sitemap(page_url)
            else:
                self.pages_to_scan.add(page_url)

    def fetch_page(self, url):
        """ Fetch a webpage and return its parsed HTML soup. """
        try:
            response = requests.get(url, headers={"User-Agent": "Mozilla/5.0"}, timeout=10)
            if response.status_code != 200:
                return None
            return BeautifulSoup(response.text, "html.parser")
        except requests.RequestException:
            return None

    def check_instagram_embeds(self, soup, page_url):
        """ Check for broken Instagram embeds on a page. """
        instagram_iframes = soup.find_all("iframe", src=True)

        for iframe in instagram_iframes:
            iframe_url = iframe["src"]
            if "instagram.com" in iframe_url:
                try:
                    iframe_response = requests.get(iframe_url, headers={"User-Agent": "Mozilla/5.0"}, timeout=5)
                    iframe_soup = BeautifulSoup(iframe_response.text, "html.parser")

                    # Look for Instagram's broken embed error message
                    error_message = iframe_soup.find("div", class_="ebmMessage")
                    if error_message and "may be broken" in error_message.text:
                        self.broken_pages.add(page_url)
                        return  # No need to check more, mark page as broken

                except requests.RequestException:
                    self.broken_pages.add(page_url)
                    return

    def scan(self):
        """ Start full website scan using sitemap URLs. """
        self.parse_sitemap(self.sitemap_url)  # Get all real pages

        print(f"\n🔍 Scanning {len(self.pages_to_scan)} pages...\n")

        for page_url in self.pages_to_scan:
            print(f"Scanning: {page_url}")
            soup = self.fetch_page(page_url)
            if not soup:
                continue

            # Check Instagram embeds
            self.check_instagram_embeds(soup, page_url)

        print("\n🔎 Scan Complete!\n")
        if self.broken_pages:
            print("🚨 Pages with Broken Instagram Embeds:")
            for page in self.broken_pages:
                print(f"- {page}")
        else:
            print("✅ No broken Instagram embeds found!")

if __name__ == "__main__":
    sitemap_url = input("Enter the XML Sitemap URL: ").strip()
    scanner = InstagramEmbedChecker(sitemap_url)
    scanner.scan()
    </code>
    </pre>
</section>
