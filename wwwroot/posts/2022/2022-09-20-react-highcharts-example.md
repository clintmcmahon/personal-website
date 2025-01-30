---
title: "React Highcharts example"
description: ""
date: "2022-09-20"
draft: false
slug: "react-highcharts-example"
tags:
---

<!--kg-card-begin: html-->
<p>In this post I show a React Highcharts example to demostrate how I&#8217;m using Highcharts with React in my data visualization projects. Assuming you are starting from scratch I&#8217;ll go from start to end. If you already have an app created then you can skip the set up section.</p>

<p>The entire project can be cloned and run from the <a href="https://github.com/clintmcmahon/reach-highcharts-example" data-type="URL" data-id="https://github.com/clintmcmahon/reach-highcharts-example" target="_blank" rel="noreferrer noopener">Github repo</a>.</p>

<h2>Set up a new React app</h2>

<p>The first step is to create a new React app by running the <a href="https://reactjs.org/docs/create-a-new-react-app.html#create-react-app" data-type="URL" data-id="https://reactjs.org/docs/create-a-new-react-app.html#create-react-app" target="_blank" rel="noreferrer noopener">Create React App command</a>. There are a number of different ways to create a new React app, but for simplicity we will use CRA here. </p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="sh" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">npx create-react-app react-highcharts-example</pre></div>

<p>Once that&#8217;s complete, move into the app directory and install the Highchart dependencies. The below script installs the latest version of the Highcharts library as well as the official <a href="https://github.com/highcharts/highcharts-react" data-type="URL" data-id="https://github.com/highcharts/highcharts-react" target="_blank" rel="noreferrer noopener">Highcharts React wrapper package</a>. I&#8217;ve also included Bootstrap in this project to help with the display. </p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="sh" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">npm install highcharts highcharts-react-official bootstrap react-bootstrap</pre></div>

<p>Now run <strong>npm start</strong> and you should have a working minimal React site running on <strong>http://localhost:3000/</strong></p>

<h2>Create the chart component</h2>

<p>Start by creating a new component called <strong>Chart.js</strong> inside the src folder<strong>.</strong> All the work for this example is done in the Chart.js component. This work can be broken down into multiple sub-components and services, but for the example it is easier to show in a single component. </p>

<p>The component renders a dataset that displays the average temperature in Minneapolis for each month of the year from the year 2021. I&#8217;m getting this data from the Regional Climate Center web services (<a href="https://www.rcc-acis.org/" data-type="URL" data-id="https://www.rcc-acis.org/" target="_blank" rel="noreferrer noopener">RCC ACIS)</a>. You&#8217;re most likely rendering data  from an API, so I&#8217;ve include a fetch method as well as a template for how to wait for the fetch method to complete in the code below. </p>

<p>When the component renders for the first time we make a call to the RCC ACIS web service to get the average temperatures for each month of 2021. Using JavaScript async/await the app waits until the API promise resolves before continuing on. You can have multiple series rendered within a single chart, so the Series property of the Highcharts Options object expects an array of arrays where each series is it&#8217;s own array. The data that&#8217;s returned from the API is not in a state to automatically apply as a series so I do some manipulation to get the data in the right format for Highcharts to consume.</p>

<p>This is the entire component code below, I&#8217;ve commented the important parts of the code in more detail to describe each part&#8217;s purpose.</p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><div style="position:absolute;top:-20px;right:0px;cursor:pointer" class="copy-simple-code-block"><span class="dashicon dashicons dashicons-admin-page"></span></div><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="javascript" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="true">import React, { useEffect, useState } from "react";
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

///
//Import necessary Highcharts dependencies
///
import Highcharts from 'highcharts'
import HighchartsReact from 'highcharts-react-official'
import HighchartsMore from 'highcharts/highcharts-more';
import HC_exporting from 'highcharts/modules/exporting'

HighchartsMore(Highcharts);
HC_exporting(Highcharts);

function Chart() {

    ///
    ///Create and set up two state variables. Options holds the options for the Highcharts
    ///component and isLoading keeps that state of the fetch request
    const [options, setOptions] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const url = "https://data.rcc-acis.org/StnData";

    useEffect(() => {
        setIsLoading(true);

        ///
        /// Create a variable for the fetchRecords method so that we can use async/await
        /// The query parts are specific to the RCC endpoint
        const fetchRecords = async () => {
            const query = {
                elems: [
                    {
                        interval: "mly",
                        duration: 1,
                        name: "avgt",
                        reduce: { "reduce": "mean" },
                        prec: 3
                    }],
                sid: "MSPthr 9",
                sDate: "2021-01-01",
                eDate: "2021-12-31",
                meta: ["name", "state", "sids"]
            }

            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                redirect: 'follow',
                body: JSON.stringify(query)
            });

            const responseData = await response.json();

            ///
            /// After the retch request returns go through the each item in the array to assign
            /// categories and create an array to be used as the series data source
            ///
            const categories = [];
            const seriesData = [];
            responseData.data.map((item) => {
                categories.push(item[0]);
                seriesData.push(parseInt(item[1]));
            });

            ///
            /// Setting the Highcharts options variable with the options object right from Highcharts
            /// Here we set things like the title and other plot options.
            /// Notice that the Series and Categories are also set here
            ///
            setOptions({
                title: {
                    text: `2021 Average temperatures in Minneapolis`
                },
                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: "Temperature (°F)"
                    }
                },
                tooltip: {
                    crosshairs: true,
                    shared: true,
                    valueSuffix: '°F'
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                    }
                },
                series: [
                    {
                        name: 'Average Temperature',
                        color: "#7393B3",
                        data: seriesData,
                        zIndex: 1,

                    }
                ]
            });

            setIsLoading(false);
        }

        fetchRecords();

    }, []);

    return (
        &lt;Row>
            &lt;Col xs={12}>
                &lt;div>
                    &lt;h1>React Highcharts example&lt;/h1>
                    &lt;Row>
                        &lt;Col s={12}>
                            {isLoading &amp;&amp;
                                &lt;div>Loading data...&lt;/div>
                            }
                            {!isLoading &amp;&amp; options &amp;&amp;
                                &lt;HighchartsReact
                                    highcharts={Highcharts}
                                    options={options}
                                />
                            }
                        &lt;/Col>
                    &lt;/Row>
                &lt;/div>
            &lt;/Col>
        &lt;/Row>
    );

}

export default Chart;</pre></div>

<h2>Update App component</h2>

<p>Remove everything inside the root div from the App.js component and replace that code with a reference to the new Chart.js component. The App.js component should now look like this:</p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><div style="position:absolute;top:-20px;right:0px;cursor:pointer" class="copy-simple-code-block"><span class="dashicon dashicons dashicons-admin-page"></span></div><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="javascript" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="true">import Chart from "./Chart.js";
import './App.css';

function App() {
return (
&lt;div className="App">
&lt;Chart />
&lt;/div>
);
}

export default App;

</pre></div>

<h2>Conclusion</h2>

<p>There you have it. If you have not already run the start command, run <strong>npm start</strong> to see the updated chart rendering twelve months of average temperatures. </p>

<p>Using the Chart.js component above you can integrate Highcharts easily into your React app. The source code for the entire React project can be found in the <a href="https://github.com/clintmcmahon/reach-highcharts-example" data-type="URL" data-id="https://github.com/clintmcmahon/reach-highcharts-example" target="_blank" rel="noreferrer noopener">Github repo</a>. There is also a working copy of the app running on an instance of the r<a href="https://clintmcmahon.github.io/react-highcharts-example/" data-type="URL" data-id="https://clintmcmahon.github.io/react-highcharts-example/" target="_blank" rel="noreferrer noopener">epo&#8217;s Github pages.</a></p>
<!--kg-card-end: html-->
