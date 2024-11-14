---
title: "TestFlight is currently unavailable. Please try again later."
description: ""
date: "2020-02-01"
draft: false
slug: "testflight-is-currently-unavailable-please-try-again-later-2"
tags:
---

<!--kg-card-begin: html--><p><!--kg-card-begin: html--></p>
<p class="graf graf--p">While testing a new iOS build for Tap In Guide I started getting this error when trying to open the app from TestFlight on my phone “TestFlight is currently unavailable. Please try again later.”</p>
<p class="graf graf--p">After trying again and again and again I still wasn’t able to open the new build — or any build — in TestFlight. I looked over <a class="markup--user markup--p-user" href="https://medium.com/u/d53dd768d047" target="_blank" rel="noopener noreferrer" data-href="https://medium.com/u/d53dd768d047" data-anchor-type="2" data-user-id="d53dd768d047" data-action-value="d53dd768d047" data-action="show-user-card" data-action-type="hover">Stack Overflow</a> and the rest of the Internet for suggestions and a lot of people were having the same issue but with quite a few different solutions.</p>
<p class="graf graf--p">These are some of the ways people have fixed or gotten around the error “TestFlight is currently unavailable. Please try again later.” .</p>
<h2 class="graf graf--p"><strong class="markup--strong markup--p-strong">My solution</strong></h2>
<ul class="postList">
<li class="graf graf--li">Remove myself from Testers (iTunes Connect → Your App → TestFlight → iTunes Connect Users → Testers → click the red circle on the line with your user.</li>
<li class="graf graf--li">Readded myself as an Tester (Make sure “send notification” is checked).</li>
<li class="graf graf--li">Open the TestFlight invite email notification from my phone.</li>
<li class="graf graf--li">Click “View in TestFlight” button from the email.</li>
<li class="graf graf--li">Done. This should open TestFlight on your phone and give you the option to install the new build.</li>
</ul>
<h2 class="graf graf--p"><strong class="markup--strong markup--p-strong">Other solutions</strong></h2>
<ul class="postList">
<li class="graf graf--li"><a class="markup--anchor markup--li-anchor" href="https://stackoverflow.com/a/15160749/118144" target="_blank" rel="noopener noreferrer" data-href="https://stackoverflow.com/a/15160749/118144">Clear Safari cache and cookies</a>.</li>
<li class="graf graf--li"><a class="markup--anchor markup--li-anchor" href="https://stackoverflow.com/a/30801899" target="_blank" rel="noopener noreferrer" data-href="https://stackoverflow.com/a/30801899">Renew the company certificate and and provisioning profiles.</a></li>
<li class="graf graf--li"><a class="markup--anchor markup--li-anchor" href="https://stackoverflow.com/a/33100429" target="_blank" rel="noopener noreferrer" data-href="https://stackoverflow.com/a/33100429">Set your device’s date time to Set Automatically.</a></li>
<li class="graf graf--li"><a class="markup--anchor markup--li-anchor" href="https://stackoverflow.com/a/38969418" target="_blank" rel="noopener noreferrer" data-href="https://stackoverflow.com/a/38969418">Turn Wi-Fi off or connect mobile network.</a></li>
<li class="graf graf--li">Delete and reinstall TestFlight.</li>
<li class="graf graf--li">Remove any zeros from the build number and resubmit the build.</li>
<li class="graf graf--li">Change the build number and resubmit the new build.</li>
</ul>
<p class="graf graf--p">Those are all the different ways to fix the error that I tried and have worked for other people. If you fix this issue using another method, please let me know and I’ll add it to this list.</p>
<p class="graf graf--p">Good luck!</p>
<p><!--kg-card-end: html--></p>
<!--kg-card-end: html-->
