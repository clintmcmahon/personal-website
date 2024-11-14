---
title: "Lessons from a WordPress hack"
description: ""
date: "2023-06-18"
draft: false
slug: "lessons-from-a-wordpress-hack-2"
tags:
---

<!--kg-card-begin: html--><p>This is a blog post about the lessons I learned from a client&#8217;s experience with a WordPress hack. Having one of your client&#8217;s websites get hacked is a harsh reminder that every website is under the thread of a cyberattack and how important it is to take the necessary security steps when running anything on the Internet.</p>
<p>This post aims to share a brief overview of what happened, the lessons I learned from this hack and to highlight the steps, lots of steps, I took to remedy the vulnerability, implement monitoring and how to climb out of the hole that is having your website labeled as spam online.</p>
<h2 id="what-happened">What Happened?</h2>
<p>The website that was hacked is a WordPress site that relies on a custom developed theme and utilizes a lot of third party plug-ins. The first thing I noticed was users visiting the site were not being able to access the site  due to their requests being blocked by their internal virus detection software. At first this was only happening for enterprise users who have a policy running on their machines that monitors what websites they are visiting.</p>
<p>Because I&#8217;m not running a URL monitoring tool on my machine &amp; neither does the client, none of us were getting the error thus making it pretty difficult to reproduce. To be safe I ran the website through a variety of highly reputable virus detection websites and all the scans came back clean. I did not however, run any of our website files through a deep virus scan.‌</p>
<figure class="kg-card kg-image-card kg-card-hascaption"><img decoding="async" src="https://www.parkasoftware.com/content/images/2023/06/chrome.png" class="kg-image" alt loading="lazy"><figcaption>
<div id="ember256" class="miw-100 tc bn form-text bg-transparent pr8 pl8 ember-view" data-kg-has-link-toolbar="true" data-koenig-dnd-disabled="true">
<div class="koenig-basic-html-input__editor-wrappper" style="cursor: text">
<div class="koenig-basic-html-input__editor __mobiledoc-editor" data-kg="editor" data-kg-allow-clickthrough="" data-placeholder="Type caption for image (optional)" spellcheck="true" contenteditable="true">
<p>Google malware example &#8211; Sucuri</p>
</div>
</div></div>
</figcaption></figure>
<p>‌</p>
<p>The next thing to happen &#8211; seemingly overnight &#8211; was Google labeled the website as having malware, or possibly having malware. Having Google label the site as having malware had a measurable impact on the site traffic which was very obvious by looking at visits in Google Analytics. Naturally we kicked it into overdrive as suddenly instead of having a couple of users not be able to access the site anyone coming into from Google or using Chrome would not get this warning.</p>
<h2 id="identify-the-problem">Identify the Problem</h2>
<p>The first step in dealing with this hack was to figure out what the problem was and to get it remedied ASAP. All of our WordPress sites are hosted on <a href="https://wpengine.com">WP Engine</a>. I opened a ticket with them to do a scan and remove any malware or infected files. I was under the impression that this was already in place and part of our hosting with their platform. I was wrong, virus monitoring and protection are not part of their hosting plan. It was my own mistake to wrongly assume something so vital was already taken care of without first verifying that was the case.</p>
<p>WP Engine uses Sucuri for their virus scans. After about 10 hours I received word  from Sucuri via WP Engine that the infected files had been cleared. The easy part was now solved. Now came the hard part, getting the site&#8217;s name cleared from all the different websites and software that were now listing this website as having malware. Just removing the infected files from the website is only the first step in the process.Once Google had declared the site as having potential malware a cascade of virus definitions and malware detection services were now listing the website as having malware. This includes anyone running Avast, FortiGuard and Norton to name a few. All these users were still seeing a malware notification when visiting the website.</p>
<h2 id="remedy-the-problem">Remedy the Problem</h2>
<p>The first step was to resubmit the site to Google to have them clear the website from their malware index. This was relatively quick as I was able to resubmit the website using the <a href="https://search.google.com/u/1/search-console">Google Search Console</a>. Search Console also sent us the notification when the site was listed as having malware. With a click of a button the site was reindex and after an hour or two we were notified that the site malware warning had been lifted.</p>
<p>After the site was cleared form Google there was a little relief. One major hurdle was down. We needed to make sure that this wasn&#8217;t going to happen again. I needed to get this site under a security scan to always be on the lookout for infected files. Part of the hosting package we offer is uptime monitoring. We did not include virus scans as part of the included package before this. This was because we incorrectly thought we got this protection from WP Engine. I was wrong in assuming something without verifying it first. I immediately signed the site up for security scanning and monitoring from Sucuri which offers WordPress monitoring in two flavors:</p>
<ol>
<li>From a plugin. I don&#8217;t recommend this way because if you have a large website the process of scanning from the website plugin will slow down the site or possibly crash the site if the process is big enough. I&#8217;m speaking from experience because this is what happened to us. In an effort to protect the website we set up scanning from the plugin and eventually crashed our own site in an effort to protect it.</li>
<li>Set up a <a href="https://docs.sucuri.net/website-monitoring/server-side-scanner/">server side scan where Sucuri</a> connects remotely to your file share and performs the scan. This is what we do now and there is no performance hit with this method. You can schedule when and how you want the scan to run. The service will email an alert when it finds a security concern. The service will then also try to clear the vulnerability if possible. The service also comes with a nice and easy to use dashboard for each website you have enlisted in their service.‌</li>
</ol>
<figure class="kg-card kg-image-card kg-card-hascaption"><img decoding="async" src="https://www.parkasoftware.com/content/images/2023/06/sucuri_monitor.png" class="kg-image" alt loading="lazy"><figcaption>
<div id="ember266" class="miw-100 tc bn form-text bg-transparent pr8 pl8 ember-view" data-kg-has-link-toolbar="true" data-koenig-dnd-disabled="true">
<div class="koenig-basic-html-input__editor-wrappper" style="cursor: text">
<div class="koenig-basic-html-input__editor __mobiledoc-editor" data-kg="editor" data-kg-allow-clickthrough="" data-placeholder="Type caption for image (optional)" spellcheck="true" contenteditable="true">
<p>Sucuri dashboard</p>
</div>
</div></div>
</figcaption></figure>
<p>‌</p>
<p>With the virus scan complete and in place it was now time to get our website&#8217;s name and url cleared from the list of virus sites.</p>
<p>I&#8217;m not entirely sure what the source of the virus scan of the world are but I&#8217;d be willing to bet Google is a top contender. Our client site was labeled by Google as having malware, then a bunch of virus scan software providers picked up the site as malware as well. There are a lot of sites that will report if a site is infected. One of the best sites and the one that I use is <a href="https://www.virustotal.com/gui/domain/parkasoftware.com">VirusTotal</a>. This site lists all the sites and virus scan providers that are reporting a website as malicious. Overtime these lists will get updated on their won, but there is no telling how long that takes.</p>
<p>I started off going through each of the items one by one that were listed as infected and resubmitted the urls via each providers website. Most of them were cleared after resubmitting but some, like <a href="https://www.fortiguard.com/webfilter">FortiGuard</a>, took me emailing multiple people and addresses in the organization to get the website cleared from their virus definition. Avast was another that I needed to message directly and they responded quickly and removed the classifications almost immediately. Most sites will have a contact that you can message to get your site cleared if the website submission does not work. But they&#8217;re all a little different. ‌</p>
<figure class="kg-card kg-image-card"><img decoding="async" src="https://www.parkasoftware.com/content/images/2023/06/Screenshot-2023-06-12-at-3.47.26-PM.png" class="kg-image" alt loading="lazy"></figure>
<p>‌</p>
<p>One by one I jumped through the hoops of each provider and slowely the site started falling off their lists. Eventually after two or thee weeks of submissions and resubmitting I was able to get all the virus scan sites to return a clean report for our client&#8217;s website.</p>
<p>In the end going responding to a WordPress hacked website was a headache on multiple fronts. Our customer looked bad and we looked bad plus we ended up spending days working to everything back to normal. To protect us and our clients against this soft of thing in the future, we offer security and virus scans out of the box at no additional cost with all our hosting plans. If you host with us or if we manage your hosting we will set up a virus scan and protection out of the box. It&#8217;s no longer a special up charge feature. All sites get this protection straight away.</p>
<p>To recap, if you find yourself in the middle of a WordPress hack or any website hack for that matter, follow these steps to fix the situation as soon as possible. Hopefully you do not experience any downtime, blocked sites or malware labels.</p>
<ol>
<li>Clear the virus using <a href="https://sucuri.net/">Sucuri</a></li>
<li>Set up a server side scan immediately</li>
<li>Install WP Fence or other firewall/Wordpress monitoring plug ins</li>
<li>Resubmit the website to Google for re-indexing</li>
<li>Tell your hosting provider</li>
<li>Work through <a href="https://virustotal.com">VirusTotal</a> to resubmit each site listed as infected and get it cleared</li>
<li>Set up a system scan to watch for future threats</li>
</ol>
<!--kg-card-end: html-->
