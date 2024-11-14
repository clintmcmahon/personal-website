---
title: "Centralized Streaming Radio Stations"
description: ""
date: "2021-06-17"
draft: false
slug: "centralized-streaming-radio-stations"
tags:
---

<!--kg-card-begin: html-->
<p>I listen to a lot of streaming radio stations during the day and wanted a way listen to each one on demand like a Spotify or Apple Music playlist, essentially a playlist for centralized streaming radio stations. Switching between a new browser tab every couple of hours isn&#8217;t an efficient way to stream, so I created a playlist file of indie rock in Seattle and Los Angeles, classical in New York and Switzerland and jazz in New Orleans and Newark and internet trance stations all into a nice collection.  </p>

<p>To make it easy to go from station to station I created a <a href="https://github.com/clintmcmahon/radio-playlist" data-type="URL" data-id="https://github.com/clintmcmahon/radio-playlist" target="_blank" rel="noreferrer noopener nofollow">radio.pls file</a> that&#8217;s a collection of all my streaming radio stations in a single file. I can then open this file in <a href="https://www.videolan.org/vlc/" target="_blank" data-type="URL" data-id="https://www.videolan.org/vlc/" rel="noreferrer noopener nofollow">VLC player</a> which loads them all in a nicely organized playlist. When I find a new Internet radio station I like I&#8217;ll find the mp3 or stream source url and add it to this playlist file.</p>

<p>Here&#8217;s an example of what the playlist file source looks like and <a href="https://www.techwalla.com/articles/how-to-make-a-pls-file-for-a-stream" target="_blank" data-type="URL" data-id="https://www.techwalla.com/articles/how-to-make-a-pls-file-for-a-stream" rel="noreferrer noopener nofollow">how to make a pls file for streaming sources</a>.</p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="plain_text" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">
numberofentries=14
File1=http://kcrw.streamguys1.com/kcrw_192k_mp3_e24_internet_radio
Title1=KCRW (Eclectic Mix) - California listener powered radio

File2=http://fr.ah.fm:8000/192k
Title2=AH.FM - Internet Trance

File3=http://jabba.dasource.ch:9010
Title3=UZIC - Internet Techno (Switzerland)

File4=https://kexp-mp3-128.streamguys1.com/kexp128.mp3
Title4=KEXP - Seattle listener powered radio</pre></div>

<p>Here&#8217;s what the playlist looks like in the VLC player, too. This will also work with other player programs that are compatible with pls files &#8211; like Winamp.<br></p>

<div class="wp-block-image"><figure class="aligncenter size-large is-resized"><img decoding="async" loading="lazy" src="/images/wordpress/2021/06/Capture.jpg" alt="VLC playlist example" class="wp-image-1002" width="568" height="508" sizes="(max-width: 568px) 100vw, 568px" /></figure></div>

<p></p>
<!--kg-card-end: html-->
