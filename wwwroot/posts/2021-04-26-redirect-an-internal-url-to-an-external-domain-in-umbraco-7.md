---
title: "Redirect A Url To An External Domain In Umbraco 7"
description: ""
date: "2021-04-26"
draft: false
slug: "redirect-an-internal-url-to-an-external-domain-in-umbraco-7"
tags:
---

<!--kg-card-begin: html--><p>One of my clients has an Umbraco 7 site that pretty much takes care of itself other than a few minor content updates here and there. Today that client asked we set up a redirect from an internal URL to an external domain for their job postings. Normally this is an easy change using IIS rewrites in the web.config with the <a href="https://www.iis.net/downloads/microsoft/url-rewrite" target="_blank" rel="noopener">IIS Rewrite Module</a>. But this site is hosted on GoDaddy and like all GoDaddy things, what I thought was easy turned out to be a bit more of a challenge.</p>
<p>After a bit of research I found you can redirect Umbraco URLs using the <a href="https://github.com/aspnetde/UrlRewritingNet" target="_blank" rel="noopener">UrlRewritingNet component</a> that&#8217;s built into Umbraco 7. Here&#8217;s an example of how to configure Umbraco 7 to redirect an internal URL to an external domain using the UrlRewriting.config config file.</p>
<p>To configure a URL rewrite open the configuration file located at /Config/UrlRewriting.config and add a rewrite rule like the example below. I found that rewriting to an external URL was more tricky than an internal virtual URL so here are a couple specifics to keep in mind when setting up a new rule for an external URL:</p>
<ol>
<li>Set <strong>rewriteOnlyVirtualUrls=&#8221;false&#8221; </strong>attribute inside <strong>&lt;urlrewritingnet</strong></li>
<li>The <strong>virtualUrl</strong> is the source URL of the page you are redirecting from. The value in this attribute must start with http(s)://domain otherwise the rule will be ignored</li>
<li><strong>destinationUrl </strong>is the destination for the redirect</li>
<li><strong>redirect </strong>should be set to <strong>&#8220;Domain&#8221;</strong></li>
</ol>
<p>Here&#8217;s an example of the UrlRewriting.config file that will redirect an internal URL to an external domain.</p>
<pre class="EnlighterJSRAW" data-enlighter-language="xml">&lt;?xml version="1.0" encoding="utf-8"?&gt;
&lt;urlrewritingnet 
    xmlns="http://www.urlrewriting.net/schemas/config/2006/07"
    rewriteOnlyVirtualUrls="false"   &gt;
    &lt;rewrites&gt;
        &lt;add 
           name="redirectexample" 
           virtualUrl="https://example.com/jobs?$" 
           rewriteUrlParameter="ExcludeFromClientQueryString" 
           destinationUrl="https://clintmcmahon.com" 
           ignoreCase="true" 
           redirectMode="Permanent" 
           redirect="Domain" /&gt;
    &lt;/rewrites&gt;
&lt;/urlrewritingnet&gt;</pre>
<p>&nbsp;</p>
<!--kg-card-end: html-->
