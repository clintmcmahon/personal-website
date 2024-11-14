---
title: "Charting historical record high temperatures: Part 1"
description: ""
date: "2021-12-06"
draft: false
slug: "charting-historical-record-high-temperatures"
tags:
---

<!--kg-card-begin: html-->
<p>I&#8217;m going to create a website where users can get a quick look at historical record high temperatures throughout the United States along with what the average temperature is for any given date within the year. Users will select a location and date to return a bar chart of the last 10 record high temperatures in addition to the average high temperature for that day.</p>

<p>Over the last couple of years the weather feels like it&#8217;s becoming more extreme. I keep asking myself &#8220;is this normal?&#8221; and want to see what history can tell me. I&#8217;d like to document the process of creating this website as part of my <a href="https://clintmcmahon.com/writing-to-becoming-a-better-writer/" data-type="post" data-id="972">writing to become a better writer</a> process. </p>

<h2>Outline</h2>

<p>The website will consist of a single web page with the following options for the user to select the location and date for which to return the data for.</p>

<p><strong>State </strong>&#8211; A list of all the states in the United States. This field will be required as it is needed to get data for the next dropdown which is a list of each available station.</p>

<p><strong>Station </strong>&#8211; A list of a particular weather station to query. Examples are Grand Marais or Minneapolis International Airport.</p>

<p><strong>Date</strong> &#8211; The date to return the temperature data for. This will be a calendar pop up for the user to select a date easily. The year does not matter in this case.</p>

<p><strong>Submit button</strong> &#8211; The button to make the magic happen.  </p>

<p>Aside from the historical records, I will also return the average high temperature for the selected date.</p>

<h2>Finding a data source</h2>

<p>The first step is finding a good data source to provide the temperature data to the website. Local meteorologist <a href="https://minnesota.cbslocal.com/personality/mike-augustyniak/" target="_blank" rel="noreferrer noopener">Mike Augustyniak</a> maintains a really nice set of tables for all of the <a href="https://minnesota.cbslocal.com/minnesota-weather-records/" target="_blank" rel="noreferrer noopener">Minnesota Weather Records</a>. At first I thought Mike&#8217;s cohort of data would be a great place to start. I actually reached out to him on Twitter to get his blessing which he kindly responded saying to go for it. The thought of copying all that data or even writing a parser seemed like a big lift at the time. But, shortly after tweeting with Mike I came across the <a href="https://www.rcc-acis.org/docs_webservices.html" target="_blank" data-type="URL" data-id="https://www.rcc-acis.org/docs_webservices.html" rel="noreferrer noopener">Applied Climate Information System</a> which has a web service that provides a huge amount of climate data for <strong>free</strong>. The ACIS provides a query builder wizard to help you create requests for every weather station in the country. The use of this service gave me the idea to open this website up to handle queries for the entire country. Since the data is available we might as well use it, right?</p>

<p>Using the query builder I was able to put together this POST request to return the top 10 record highs for a given date. In this query I&#8217;m using January 1st.  From the query results below you can see that this will return exactly the data I need. With this response I only need to order the results and pipe them into a charting software. </p>

<p><a href="http://jsfiddle.net/clintmcmahon/wz1dfxq3/" target="_blank" data-type="URL" data-id="http://jsfiddle.net/clintmcmahon/wz1dfxq3/" rel="noreferrer noopener">View in jsFiddle</a></p>

<pre class="EnlighterJSRAW" data-enlighter-language="js" data-enlighter-theme="" data-enlighter-highlight="" data-enlighter-linenumbers="" data-enlighter-lineoffset="" data-enlighter-title="" data-enlighter-group="">var url = "https://data.rcc-acis.org/StnData",
    params = {
    "sid":"MSPthr",
    "sdate":"por",
    "edate":"por",
    "elems":[
        {
            "name":"maxt",
            "interval":"dly",
            "duration":"dly",
            "smry":
                {"reduce":"max",
                    "add":"date",
                    "n":10
                },
            "smry_only":1,
            "groupby":[
                "year",
                "01-01",
                "01-01"
                ]
        }
    ],
    "meta":[
        "name",
        "state",
        "valid_daterange"
    ]
};
postResults(url, params);</pre>

<p>The above query returns this result:</p>

<pre class="EnlighterJSRAW" data-enlighter-language="json" data-enlighter-theme="" data-enlighter-highlight="" data-enlighter-linenumbers="" data-enlighter-lineoffset="" data-enlighter-title="" data-enlighter-group="">{
    "meta": {
        "state": "MN",
        "name": "Minneapolis-St Paul Area",
        "valid_daterange": [
            [
                "1872-10-01",
                "2021-12-02"
            ]
        ]
    },
    "smry": [
        [
            [
                [
                    "48",
                    "1897-01-01"
                ],
                [
                    "43",
                    "1998-01-01"
                ],
                [
                    "42",
                    "1913-01-01"
                ],
                [
                    "42",
                    "1880-01-01"
                ],
                [
                    "41",
                    "1964-01-01"
                ],
                [
                    "41",
                    "1944-01-01"
                ],
                [
                    "41",
                    "1892-01-01"
                ],
                [
                    "39",
                    "1950-01-01"
                ],
                [
                    "39",
                    "1894-01-01"
                ],
                [
                    "39",
                    "1874-01-01"
                ]
            ]
        ]
    ]
}</pre>

<p>The next step in creating this website is to wire up the response to a React front end. I&#8217;ll cover the front end set up in another blog post.</p>
<!--kg-card-end: html-->
