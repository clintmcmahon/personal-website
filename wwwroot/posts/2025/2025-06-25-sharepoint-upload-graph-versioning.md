---
title: "How to Upload Files to SharePoint Using Microsoft Graph (Without Breaking Versioning)"
description: "A step-by-step guide to uploading files to SharePoint with Microsoft Graph API—while preserving version history."
date: "2025-06-25"
draft: true
slug: "sharepoint-upload-graph-versioning"
tags: [sharepoint, microsoft-graph, api, consulting]
---

<section>
<p>
Uploading files to SharePoint via Microsoft Graph sounds simple—until you realize you’re breaking version history or running into weird errors. Here’s how I help clients upload files the right way, keeping versioning intact.
</p>

<h2>Why Versioning Breaks</h2>
<ul>
<li>Overwriting files without proper headers</li>
<li>Using the wrong API endpoint (driveItem vs. listItem)</li>
<li>Missing required permissions</li>
</ul>

<h2>How to Upload Without Breaking Versioning</h2>
<ol>
<li>Use the <code>/drive/items/{item-id}:/filename:/content</code> endpoint for uploads</li>
<li>Set the correct <code>If-Match</code> header to preserve versions</li>
<li>Test with a non-production file first</li>
</ol>

<h2>Sample C# Code</h2>
<pre><code>// Example: Upload a PDF to SharePoint with versioning
var fileStream = File.OpenRead("myfile.pdf");
await graphClient
    .Sites[siteId]
    .Drive.Items[itemId]
    .ItemWithPath("myfile.pdf")
    .Content
    .Request()
    .PutAsync<DriveItem>(fileStream);
</code></pre>

<p>
Need help with a tricky SharePoint integration? <a href="/contact">Let’s get it working</a>.
</p>
</section>
