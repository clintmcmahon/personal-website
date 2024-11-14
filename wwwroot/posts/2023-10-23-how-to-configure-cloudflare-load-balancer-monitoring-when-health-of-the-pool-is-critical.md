---
title: "How to Configure Cloudflare's Load Balancer Monitoring When Health of the Pool is Critical"
description: ""
date: "2023-10-23"
draft: false
slug: "how-to-configure-cloudflare-load-balancer-monitoring-when-health-of-the-pool-is-critical"
tags:
---

<!--kg-card-begin: html-->
<p>I recently faced an issue where my Cloudflare managed API endpoint was functioning correctly—returning a 200 OK status when I hit the endpoint manually—but Cloudflare&#8217;s Load Balancer was reporting a &#8220;Critical&#8221; health status. After a lot of trial and error with some investigation, I found that the problem was with the Header Settings in the monitor health check. This post will outline the steps I took to get the health monitor check to return healthy.</p>

<h2 class="wp-block-heading">Initial Troubleshooting</h2>

<p>I created a simple endpoint in my API specifically for the health monitor. The endpoint was a simple GET and returned a 200 along with the server name as the body text. This endpoint was returning a 200 response code from Postman and through my browser, so I double checked that the monitor configuration was correct in Cloudflare:</p>

<ul>
<li><strong>URL</strong>: I made sure it was pointing to the correct endpoint that was returning 200 when I manually hit the url.</li>

<li><strong>Method</strong>: Set to <code>GET</code> as my API was expecting.</li>

<li><strong>Timeout and Frequency</strong>: Set to 60.</li>
</ul>

<p>These checked out but the health of the monitor was still returning &#8220;Critical&#8221;. At first I didn&#8217;t think that I needed to set any additional headers, ignorantly thinking that Cloudflare would would set default headers of the monitor like Postman does, but that&#8217;s not the case. So I went to the next step and manually set the headers.</p>

<h2 class="wp-block-heading">The Breakthrough: Header Settings</h2>

<p>After further digging and reading troubleshooting posts on Cloudflare forums, I decided to manually set the headers under the Advanced settings for the monitor. Within the health monitor set up, I filled out the name, url and type of request. Then I clicked Advanced Settings and created the following header settings for the monitor.</p>

<ul>
<li><strong>Host</strong>: I set this to the domain that the health check was targeting, for example, <code>api.parkasoftware.com</code>.</li>

<li><strong>User-Agent</strong>: I set this to <code>Cloudflare-Health-Check</code>.</li>

<li><strong>Authorization</strong>: Since my API was public I didn&#8217;t set this to anything.</li>

<li><strong>Accept</strong>: I set this to <code>application/json</code>.</li>
</ul>

<p>And would&#8217;nt you know it, my API health monitor started returning <strong>Healthy</strong>!</p>

<figure class="wp-block-image size-full"><img loading="lazy" decoding="async" src="/images/wordpress/2023/10/Screenshot-2023-10-23-at-10.43.05%E2%80%AFAM.png" alt="" class="wp-image-1277"/ sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<h2 class="wp-block-heading">The How-To: Setting Up Headers in Cloudflare</h2>

<p>Here are the steps I took to get the Headers set up in my Cloudflare load balancer health monitor.</p>

<ol>
<li><strong>Log in to Cloudflare Dashboard</strong>: Go to the &#8220;Traffic&#8221; tab and locate the Load Balancer section. Click edit on the load balancer to edit.</li>

<li><strong>Click Monitors</strong>: Next to your Load Balancer, you&#8217;ll see the health check configurations. Click edit. Click Remove if you already have a monitor configured. Then create a new monitor.</li>

<li><strong>Add Default Options</strong>: Fill out the default name, Action and Url.</li>

<li><strong>Add Headers</strong>: Click advanced settings and scroll down to the section where you can add headers. Here, enter the key-value pairs for each header.
<ul>
<li>Key: <code>Host</code>, Value: <code>api.parkasoftware.com</code></li>

<li>Key: <code>User-Agent</code>, Value: <code>Cloudflare-Health-Check</code></li>

<li>Key: <code>Authorization</code>, Value: None or your <code>Bearer access token if required</code></li>

<li>Key: <code>Accept</code>, Value: <code>application/json</code></li>
</ul>
</li>

<li><strong>Save and Test</strong>: Once all headers are added, save your changes. It&#8217;s a good idea to test the health check to make sure it&#8217;s working as expected.</li>
</ol>

<h2 class="wp-block-heading">Conclusion</h2>

<p>The issue was resolved after correctly configuring the Header Settings in Cloudflare&#8217;s health check. If you ever find yourself in a similar situation, double-checking these settings could be the key to solving your problem.</p>

<p>Good luck!</p>
<!--kg-card-end: html-->
