#!/usr/bin/env python3
"""
Remove non-location tags from all photo markdown files.
Run from project root: python3 clean_tags.py
"""

import os, re, glob

# ── Canonical location tags to KEEP (case-insensitive match) ──────────────────
LOCATION_TAGS = {
    # Cities
    "minneapolis", "mpls", "minneaoplis", "minnepaolis",
    "st. paul", "saint paul", "st paul", "st. anothony main", "st. anthony main",
    "new york city", "nyc", "manhattan", "chicago", "nashville", "austin",
    "venice", "hamburg", "dusseldorf", "fergus falls", "newark", "moab",
    "los angeles", "losangeles", "grand marais", "new york",

    # States / countries / regions
    "minnesota", "california", "utah", "texas", "tennessee", "tn",
    "colorado", "wisconsin", "new jersey", "germany", "ghana", "caribbean",
    "turks and caicos", "tci",

    # Minneapolis neighbourhoods / districts
    "north loop", "northloop", "uptown", "uptown minneapolis", "uptown mpls",
    "downtown", "northeast", "south minneapolis", "mill district",
    "warehouse district", "elliot park", "stevens square", "longfellow",
    "whittier", "sheffield", "lyndale", "mill city", "loring park",
    "powderhorn park", "riverside", "cedar riverside",

    # Minneapolis streets / bridges
    "hennepin avenue", "hennepin ave", "hennepin", "nicollet mall",
    "washington avenue", "marquette avenue", "lake street",
    "hennepin avenue bridge", "hennepin ave bridge", "cedar",
    "6th avenue", "sixth avenue",

    # Minneapolis landmarks / buildings / venues
    "foshay", "foshay tower", "ids center", "ids", "capella tower",
    "wells fargo center", "wells fargo tower", "ford center",
    "target field", "target field station", "metrodome", "vikings stadium",
    "guthrie theater", "guthrie", "first avenue", "stone arch bridge",
    "spoon bridge", "spoon and cherry", "basilica",
    "minneapolis institute of arts", "mia", "hennepin county library", "hclib",
    "northrup king", "nicollet island", "minnehaha falls", "minnehaha",
    "soo line", "skyway", "state fair",

    # Minneapolis transit lines (specific named routes)
    "green line", "blue line", "light rail", "lrt", "light rail transit",

    # Minneapolis / Minnesota lakes & waterways
    "lake calhoun", "lake of the isles", "lake harriet", "lake superior",
    "mississippi river", "lake street",

    # Greater Minnesota
    "north shore", "gunflint trail", "boundary waters", "bwca",
    "otter tail county", "ottertail county",

    # NYC landmarks / neighbourhoods
    "lower east side", "chelsea", "flatiron", "rockefeller center",
    "empire state building", "world trade center", "freedom tower",
    "john hancock", "john hancock building", "chrysler building",
    "30 rock", "avenue of the americas", "14th street", "union squre",

    # Turks & Caicos
    "chalk sound", "providenciales",

    # National parks / natural areas
    "canyonlands", "arches", "national park", "half moon bay",
    "beaver creek", "blue hills", "north shore",
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
