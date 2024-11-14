---
title: "List of all weather stations by state"
description: ""
date: "2022-04-21"
draft: false
slug: "list-of-all-weather-stations-by-state"
tags:
---

<!--kg-card-begin: html-->
<p>I&#8217;m currently writing an app to display and <a href="https://wxrecords.com" data-type="URL" data-id="https://wxrecords.com" target="_blank" rel="noreferrer noopener">chart record high and low temperatures</a> for locations around the United States. As I was writing the station selector I realized that I would need to create a list of all weather stations by state. Because of the large number of weather stations around the country, I have the user filter down the stations by first selecting the state from a drop down, then the weather stations load based on the selected state in another drop down. The data is all coming from the <a href="https://www.rcc-acis.org/" data-type="URL" data-id="https://www.rcc-acis.org/" target="_blank" rel="noreferrer noopener">RCC-ACIS </a>API and the request to get the list of stations can take a long time depending on what state is selected. For example, the request to get all the weather stations for California can take up to five seconds.  </p>

<p>If you are just looking for a list of all the weather stations sorted by state, then <a href="https://github.com/clintmcmahon/fetch-all-weather-stations/blob/main/json_data.json" data-type="URL" data-id="https://github.com/clintmcmahon/fetch-all-weather-stations/blob/main/json_data.json" target="_blank" rel="noreferrer noopener">download the entire file from the Github repo</a>. Otherwise here&#8217;s the California request that takes some time and prompted me to change the functionality of the app:</p>

<pre class="EnlighterJSRAW" data-enlighter-language="js" data-enlighter-theme="" data-enlighter-highlight="" data-enlighter-linenumbers="" data-enlighter-lineoffset="" data-enlighter-title="" data-enlighter-group="">const response = await fetch(metaUrl, {
    method: 'POST',
    headers: {
       'Content-Type': 'application/json'
    },
    redirect: 'follow',
    body: JSON.stringify({
            'elems': 'maxt,mint',
            'sdate': '1871-01-01',
            'edate': '2022-04-21',
            'state': 'CA'
        })
 }); </pre>

<p>Instead of a creating a new request every time a user selects a state, I decided to combine the states and their stations into a single file. The file is loaded at startup with the app so there&#8217;s no more hitting the RCC ACIS API to get the list of stations. Although this method saves time on the front end for the user, the trade off is that the app is no longer dynamically in sync with what is available on the RCC ACIS API. I&#8217;ll need to create a system to keep the list of stations from the RCC ACIS API in sync with my program. But given that the stations don&#8217;t change frequently and the app is much more reactive for the user, I think this is a pretty good trade off.</p>

<p>I couldn&#8217;t find an existing list of states and stations online so I wrote a Python script to loop through all the states to build one large json file that combined both state and list of weather stations. The file contains every state available in the RCC ACIS as a parent with an array of available weather stations as children. </p>

<p>Here&#8217;s the Python script that loops through the list of states to return an array of stations for that state. <a href="https://github.com/clintmcmahon/fetch-all-weather-stations" target="_blank" data-type="URL" data-id="https://github.com/clintmcmahon/fetch-all-weather-stations" rel="noreferrer noopener">The full source is available on Github</a>.</p>

<pre class="EnlighterJSRAW" data-enlighter-language="python" data-enlighter-theme="" data-enlighter-highlight="" data-enlighter-linenumbers="" data-enlighter-lineoffset="" data-enlighter-title="" data-enlighter-group=""># encoding: utf-8

import sys
import datetime
import json
import requests


def get_stations(state):
    response = requests.post('https://data.rcc-acis.org/StnMeta', data={
                             "elems": "maxt,mint", "sdate": "1871-01-01", "edate": "2022-04-21", "state": state})
    data = response.json()
    return data["meta"]


def main():
    '''
    Main method
    '''
    stations = []
    states = open('states.json')
    statesData = json.load(states)
    for state in statesData:
        stations.append(
            {"name": state["name"], "stations": get_stations(state["shortCode"])})

    with open('json_data.json', 'w') as outfile:
        json.dump(stations, outfile)


if __name__ == '__main__':
    main()
</pre>

<p>Here&#8217;s a snippet from the file produced file. You can <a href="https://github.com/clintmcmahon/fetch-all-weather-stations/blob/main/json_data.json" data-type="URL" data-id="https://github.com/clintmcmahon/fetch-all-weather-stations/blob/main/json_data.json" target="_blank" rel="noreferrer noopener">download the entire file from the Github repo</a>.</p>

<pre class="EnlighterJSRAW" data-enlighter-language="json" data-enlighter-theme="" data-enlighter-highlight="" data-enlighter-linenumbers="" data-enlighter-lineoffset="" data-enlighter-title="" data-enlighter-group="">[
  {
    "name": "Alabama",
    "stations": [
      {
        "name": "ALABASTER SHELBY CO AP ASOS",
        "ll": [-86.78178, 33.17835],
        "sids": [
          "53864 1",
          "010116 2",
          "EET 3",
          "72230 4",
          "KEET 5",
          "USW00053864 6",
          "EET 7"
        ],
        "state": "AL",
        "elev": 566.0,
        "uid": 2
      },
      {
        "name": "MUSCLE SHOALS AP",
        "ll": [-87.59971, 34.74388],
        "sids": [
          "13896 1",
          "015749 2",
          "MSL 3",
          "KMSL 5",
          "USW00013896 6",
          "MSL 7"
        ],
        "state": "AL",
        "elev": 544.0,
        "uid": 3
      },
      ....
]
</pre>

<p></p>
<!--kg-card-end: html-->
