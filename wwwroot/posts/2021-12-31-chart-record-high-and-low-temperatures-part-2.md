---
title: "Chart record high and low temperatures: Part 2"
description: ""
date: "2021-12-31"
draft: false
slug: "chart-record-high-and-low-temperatures-part-2"
tags:
---

<!--kg-card-begin: html-->
<p>In the last post, <a href="https://clintmcmahon.com/charting-historical-record-high-temperatures/" data-type="URL" data-id="https://clintmcmahon.com/charting-historical-record-high-temperatures/">Charting historical record high temperatures: Part 1</a>, I wrote a plan and light technical specification to outline a project to chart record high and low temperatures in a dashboard for a given location as well as other record breaking weather events. Since I wrote that last post I&#8217;ve put together a stable <a href="https://wxrecords.com" target="_blank" data-type="URL" data-id="https://wxrecords.com" rel="noreferrer noopener">MVP React front end</a>. </p>

<h2>Challenges</h2>

<p>One of the biggest challenges in creating this dashboard has been to find a reliable data source to use. Specifically, one that is constantly being updated without any work on my part. In my research I came across a few exportable data sets, but in order to use those data sets I would need to create an automated process to export the data, import it into a usable back end like a database or GitHub repo and then write an API to serve the data to my React front end. Hence, finding something already automated would be optimal for this project.</p>

<p>I found the <a href="https://www.rcc-acis.org/docs_webservices.html" target="_blank" data-type="URL" data-id="https://www.rcc-acis.org/docs_webservices.html" rel="noreferrer noopener nofollow">RCC-ACIS web services</a> that are pretty extensible and give me the exact data needed for the dashboard. There&#8217;s not a ton of documentation available for the API &#8211; other than some examples on the web services page &#8211; so building the queries has been a little bit of trial and error. There are a <a href="https://xmacis.rcc-acis.org/" target="_blank" data-type="URL" data-id="https://xmacis.rcc-acis.org/" rel="noreferrer noopener nofollow">couple different</a> <a href="https://builder.rcc-acis.org/" target="_blank" data-type="URL" data-id="https://builder.rcc-acis.org/" rel="noreferrer noopener nofollow">query builders</a> that have been helpful in creating the different queries and post requests that I need to return the right data. One of the hardest parts so far has been trying to figure out the API and the different variables available in order to get the right data returned.</p>

<p>At first I was just going to create this dashboard for Minneapolis, but after seeing the RCC-ACIS API I saw that I could access all the weather stations across the United States. Now there are two drop downs on the front end for the user to choose a state and weather station area. The weather station area drop down is a subset of the weather stations for the selected state. Because not all the weather stations available have continuous data coverage, I had to use ThreadEx stations or stations that combine data for a given area. TheadEx stands for Threaded Extremes which are stations that take weather data recorded at National Weather Service Automated Surface Observing Stations and merge it together with other nearby data to create a single data set of weather information. To learn more about ThreadEx, visit the <a href="http://threadex.rcc-acis.org/help/about.html" target="_blank" rel="noreferrer noopener">project background page</a>.</p>

<p>Another challenge has been the request to the StnMeta endpoint to return the list of stations for the selected state. When a user selects the state the app sends a request to a <a href="http://jsfiddle.net/clintmcmahon/8eu7amq0/" target="_blank" data-type="URL" data-id="http://jsfiddle.net/clintmcmahon/8eu7amq0/" rel="noreferrer noopener nofollow">station meta data endpoint</a>.  This request is quite a bit slower than all the other requests to the API because it&#8217;s returning every possible station available for the selected state. After the data is returned I then trim down the results to only return the ThreadEx Areas. The next step to avoid this delay is to put every ThreadEx Area station into a local .json file that gets bundled with the app, dramatically increasing the station drop down load time. </p>

<h2>What I&#8217;ve built so far</h2>

<h3>Record temperatures for a selected date</h3>

<figure class="wp-block-image size-large"><img decoding="async" loading="lazy" src="/images/wordpress/2021/12/Screen-Shot-2021-12-31-at-8.43.02-AM.png" alt="" class="wp-image-1123" sizes="(max-width: 1024px) 100vw, 1024px" / sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<p>The first section of the front end contains six different components that display record high temperature, low temperature, coldest high temperature and warmest low temperature records. The other two components are normal high and normal low temperatures to compare the records to. </p>

<h3>All time record highs and lows for a selected date</h3>

<figure class="wp-block-image size-large"><img decoding="async" loading="lazy" src="/images/wordpress/2021/12/Screen-Shot-2021-12-31-at-8.43.10-AM.png" alt="Chart record high and low temperatures" class="wp-image-1124" sizes="(max-width: 1024px) 100vw, 1024px" / sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<p>In addition to the record high temperature for the selected date, the next section shows the all time record highs and all time record lows for the selected date. By default the order is from highest/lowest record first and down the array from there. At first I had the records ordered by date so that you could easily see if there was a pattern developing but in the end decided I liked highest to lowest was more visually appealing. </p>

<h3>Monthly temperature trends for a selected date&#8217;s month</h3>

<figure class="wp-block-image size-large"><img decoding="async" loading="lazy" src="/images/wordpress/2021/12/Screen-Shot-2021-12-31-at-8.43.19-AM.png" alt="" class="wp-image-1125" sizes="(max-width: 1024px) 100vw, 1024px" / sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<p>After the top ten record highs and low temperature charts, the section that follows is for the selected date&#8217;s month temperatures. This section displays the number of days the high temperature was above the normal high and the number of days the high temperature was below the normal high. Then the chart displays the high, low and normal temperature range for each day of the month. High and low is month to selected date, while normal range goes through the entire month.</p>

<h3>Snowfall for selected date&#8217;s season</h3>

<figure class="wp-block-image size-large"><img decoding="async" loading="lazy" src="/images/wordpress/2021/12/Screen-Shot-2021-12-31-at-8.43.30-AM.png" alt="" class="wp-image-1126" sizes="(max-width: 1024px) 100vw, 1024px" / sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<p>Finally, this week I was able to get the chart to display the snowfall for the selected date&#8217;s season. The normal snowfall numbers per &#8220;year&#8221; are not calculated January through December like precipitation (rain) numbers are, however they&#8217;re calculated from July to June. I reached out to the NWS Twin Cities on Twitter for verification on the dates and they replied within a couple minutes.</p>

<blockquote class="twitter-tweet"><p dir="ltr" lang="en">You are correct. July through June.</p>— NWS Twin Cities (@NWSTwinCities) <a href="https://twitter.com/NWSTwinCities/status/1476395759444496387?ref_src=twsrc%5Etfw">December 30, 2021</a></blockquote> <script async="" src="https://platform.twitter.com/widgets.js" charset="utf-8"></script>

<p>There is some logic built into this component to determine which season to show the user. If the selected month is &lt; July then the chart will show the previous snow season, if the month selected is &gt; July then the app looks forward to the current or upcoming snow season. I would like to improve this section by adding snowfall records per month and season. </p>

<p>That&#8217;s what I&#8217;ve built so far and am excited about where this project is going to go. I&#8217;ve got plans to make it an iOS/Android app in the future, but for now the website will live as a kind of MVP/test site for different charts and queries. Since I&#8217;ve been working on this project in the early morning or late at night while I&#8217;m not busy with billable client work or being a dad it&#8217;s a slow go. But it&#8217;s been pretty fun so far and I&#8217;m already using it to look up record highs and lows for the days when the temperature seems like it&#8217;s outside of the normal range. </p>

<p>The Weather Records dashboard website can be found at <a href="https://wxrecords.com" target="_blank" data-type="URL" data-id="https://wxrecords.com" rel="noreferrer noopener">wxrecords.com</a></p>
<!--kg-card-end: html-->
