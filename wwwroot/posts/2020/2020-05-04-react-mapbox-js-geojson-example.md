---
title: "React + Mapbox GeoJSON Example"
description: ""
date: "2020-05-04"
draft: false
slug: "react-mapbox-js-geojson-example"
tags:
---

<!--kg-card-begin: html--><p>Here&#8217;s an example of how to create a Mapbox map in React using a GeoJSON data set. When I started building my first React Mapbox map I built them around some of the other pre-built components out there like <a title="ReactMapboxGL" href="https://github.com/alex3165/react-mapbox-gl/blob/master/docs/API.md">ReactMapboxGL</a> or <a title="react-map-gl" href="https://github.com/visgl/react-map-gl">react-map-gl</a>. Both of these wrappers are great but eventually figured out I was better able to control all the Mapbox features on my own by targeting the Mapbox GL JS libary directly vs using a another component.</p>
<p>This example uses React hooks but could easily be updated for a class component if that is what you are working with. Below is one component called Districts that loads Minnesota&#8217;s eight congressional districts from a separate geojson file. After loading the districts they are given a unique fill-color and added as a layer.</p>
<p>You can view the entire React Mapbox GeoJSON example project in the <a href="https://github.com/clintmcmahon/react-mapbox-example" target="_blank" rel="noopener noreferrer">GitHub repository</a> or you can view a live example at <a href="https://clintmcmahon.github.io/react-mapbox-example/" rel="noopener noreferrer">clintmcmahon.github.io/react-mapbox-example</a></p>

<h2>Districts.js React Component</h2>

<div class="wp-block-simple-code-block-ace" style="height: 250px; position: relative; margin-bottom: 50px;">
<pre class="wp-block-simple-code-block-ace" style="position: absolute; top: 0; right: 0; bottom: 0; left: 0;" data-mode="javascript" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">import React, { useState, useEffect, useRef } from "react";
import mnDistricts from "./data/mn/mn-districts.geojson";
import ReactDOM from 'react-dom';
import mapboxgl from 'mapbox-gl';

function Districts(props) {
mapboxgl.accessToken = process.env.REACT_APP_MAPBOX_KEY;
const mapContainer = useRef(null);
const [long, setLong] = useState(-94.503809);
const [lat, setLat] = useState(46.443226);
const [zoom, setZoom] = useState(4.5);
const [hoveredDistrict, _setHoveredDistrict] = useState(null);
const hoveredDistrictRef = useRef(hoveredDistrict);

    const setHoveredDistrict = data =&gt; {
        hoveredDistrictRef.current = data;
        _setHoveredDistrict(data);
    };

    useEffect(() =&gt; {

        let map = new mapboxgl.Map({
            container: mapContainer.current,
            style: "mapbox://styles/mapbox/light-v10",
            center: [long, lat],
            zoom: zoom
        });


            // Add zoom and rotation controls to the map.
            map.addControl(new mapboxgl.NavigationControl());
        map.once("load", function () {

            map.addSource('district-source', {
                'type': 'geojson',
                'data': mnDistricts
            });

            map.addLayer({
                'id': 'district-layer',
                'type': 'fill',
                'source': 'district-source',
                'layout': {},
                'paint': {
                    'fill-color': [
                        'match',
                        ['get', 'CD116FP'],
                        '01',
                        '#5AA5D7',
                        '02',
                        '#02735E',
                        '03',
                        '#00E0EF',
                        '04',
                        '#84D0D9',
                        '05',
                        '#202359',
                        '06',
                        '#CE7529',
                        '07',
                        '#00AE6C',
                        '08',
                        '#0056A3',
                        /* other */ '#ffffff'
                    ],
                    'fill-opacity': [
                        'case',
                        ['boolean', ['feature-state', 'hover'], false],
                        .8,
                        0.5
                    ]
                }
            });

            map.on('mousemove', 'district-layer', function (e) {
                if (e.features.length &gt; 0) {
                    if (hoveredDistrictRef.current &amp;&amp; hoveredDistrictRef.current &gt; -1) {

                        map.setFeatureState(
                            { source: 'district-source', id: hoveredDistrictRef.current },
                            { hover: false }
                        );
                    }

                    let _hoveredDistrict = e.features[0].id;

                    map.setFeatureState(
                        { source: 'district-source', id: _hoveredDistrict },
                        { hover: true }
                    );

                    setHoveredDistrict(_hoveredDistrict);
                }

            });

            // When the mouse leaves the state-fill layer, update the feature state of the
            // previously hovered feature.
            map.on('mouseleave', 'district-layer', function () {
                if (hoveredDistrictRef.current) {
                    map.setFeatureState(
                        { source: 'district-source', id: hoveredDistrictRef.current },
                        { hover: false }
                    );
                }
                setHoveredDistrict(null);
            });

        });

    }, []);

    return (
        &lt;div className="district-map-wrapper"&gt;
            &lt;div id="districtDetailMap" className="map"&gt;
                &lt;div style={{ height: "100%" }} ref={mapContainer}&gt;

                &lt;/div&gt;
            &lt;/div&gt;
        &lt;/div&gt;
    );

}

export default Districts;</pre>

</div>

<h2>Minnesota Congressional Districts GeoJSON</h2>

<p>This geojson data set represents Minnesota&#8217;s eight congressional districts that I pulled from the <a href="https://www.census.gov/geographies/mapping-files/time-series/geo/tiger-line-file.html" target="_blank" rel="noreferrer noopener">US Census</a>. Each feature has a property &#8220;CD116FP&#8221; that I&#8217;m using to set the fill-color of each district layer. The GeoJSON data is so big I didn&#8217;t include it in this blog post but you can <a href="https://github.com/clintmcmahon/react-mapbox-example/blob/master/src/data/mn/mn-districts.geojson">download it from the Github repo</a>.</p>

<p>You can view the source on <a href="https://github.com/clintmcmahon/react-mapbox-example">Github</a>.</p>

<p>That&#8217;s it. Happy coding!</p>
<!--kg-card-end: html-->
