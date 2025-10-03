---
title: "Is September Getting Hotter in Minneapolis? A Data Visualization Look"
description: "A Highcharts data visualization analysis of September temperatures in Minneapolis, comparing historical highs and monthly averages to see if the city’s falls are warming."
date: "2025-10-03"
draft: false
slug: "minneapolis-september-temperatures"
tags: consulting, climate, data-visualization, minneapolis
---

<section>
<p>
  I remember September in Minneapolis being a lot cooler than it has been in the last couple of years. Since 2020, it feels like summer in Minneapolis now includes September and the first bit of October. Normally around this time (October 3rd), nights turn cooler, daytime highs are generally in the 60s and 70s, and we're making our way into peak fall color season. But recently, when September comes around, I find myself wondering if we're going to have a summer September or a fall September. Essentially, is September in Minneapolis actually getting hotter?
</p>
<p>
  Using more than 150 years of records, I created this Highcharts scatter plot to compare two different ways of measuring September temperatures: the single hottest day in the month (the "highest max") and the average temperature across all days. Average temperature takes into account both the high and the low for each day, which is a good way to measure the overall daily temperature. By putting these data sets into a scatter plot type of data visualization, we can see trends clearly.   
</p>
<p>
  The hottest September day in Minneapolis history still belongs to 1931, when the temperature soared to 104°F. The 1930s were the hottest decade in Minneapolis history. Other one-day extremes are scattered across the early and mid-1900s, with a few recent appearances like 2023, when the highest temp hit 98°F. These peaks show that while record-breaking daily extremes are part of the historical story, they haven’t consistently pushed higher in the last decade. A scatter chart of those single-day highs looks noisy, with dots spread across time rather than marching steadily upward. By this dataset, September in Minneapolis isn't getting hotter in the sense that we're reaching new maximum temperatures.
</p>
<div id="minneapolis-september-chart" style="height:500px; margin: 2em 0;"></div>

<script src="https://code.highcharts.com/highcharts.js"></script>
<script src="https://code.highcharts.com/modules/exporting.js"></script>
<script src="https://code.highcharts.com/modules/accessibility.js"></script>

<script>
Highcharts.chart('minneapolis-september-chart', {
  chart: {
    type: 'scatter',
    zoomType: 'xy'
  },
  title: {
    text: 'Minneapolis September Temperatures (1873–2025)'
  },
  subtitle: {
    text: 'Data visualization with Highcharts — comparing highest daily maximums and monthly averages'
  },
  accessibility: {
    description: 'Scatter plot showing Minneapolis September temperatures from 1873 to 2025. '
      + 'The chart compares the single hottest day in each September with the monthly average temperature. '
      + 'Trendlines show that while extreme one-day highs have remained relatively stable, the month as a whole has steadily warmed.'
  },
  xAxis: {
    title: { text: 'Year' },
    allowDecimals: false
  },
  yAxis: {
    title: { text: 'Temperature (°F)' }
  },
  tooltip: {
    pointFormat: 'Year: <b>{{point.x}}</b><br/>Temp: <b>{{point.y:.1f}} °F</b>'
  },
  plotOptions: {
    scatter: {
      marker: { radius: 4, symbol: 'circle' }
    },
    line: { marker: { enabled: false } }
  },
  series: [{
    name: 'HighestMaxTemperature',
    type: 'scatter',
    data: [[1873, 86.0], [1874, 92.0], [1875, 89.0], [1876, 77.0], [1877, 91.0], [1878, 94.0], [1879, 78.0], [1880, 90.0], [1881, 91.0], [1882, 91.0], [1883, 84.0], [1884, 87.0], [1885, 88.0], [1886, 88.0], [1887, 83.0], [1888, 81.0], [1889, 88.0], [1890, 84.0], [1891, 93.0], [1892, 85.0], [1893, 94.0], [1894, 94.0], [1895, 96.0], [1896, 82.0], [1897, 91.0], [1898, 96.0], [1899, 89.0], [1900, 92.0], [1901, 90.0], [1902, 82.0], [1903, 82.0], [1904, 83.0], [1905, 84.0], [1906, 92.0], [1907, 84.0], [1908, 94.0], [1909, 84.0], [1910, 88.0], [1911, 88.0], [1912, 95.0], [1913, 97.0], [1914, 85.0], [1915, 87.0], [1916, 91.0], [1917, 84.0], [1918, 81.0], [1919, 91.0], [1920, 91.0], [1921, 91.0], [1922, 98.0], [1923, 88.0], [1924, 78.0], [1925, 98.0], [1926, 88.0], [1927, 92.0], [1928, 81.0], [1929, 95.0], [1930, 88.0], [1931, 104.0], [1932, 85.0], [1933, 94.0], [1934, 86.0], [1935, 89.0], [1936, 95.0], [1937, 97.0], [1938, 89.0], [1939, 98.0], [1940, 91.0], [1941, 88.0], [1942, 90.0], [1943, 86.0], [1944, 88.0], [1945, 94.0], [1946, 85.0], [1947, 95.0], [1948, 94.0], [1949, 84.0], [1950, 86.0], [1951, 86.0], [1952, 92.0], [1953, 95.0], [1954, 88.0], [1955, 94.0], [1956, 87.0], [1957, 90.0], [1958, 86.0], [1959, 94.0], [1960, 95.0], [1961, 93.0], [1962, 82.0], [1963, 85.0], [1964, 90.0], [1965, 84.0], [1966, 87.0], [1967, 82.0], [1968, 85.0], [1969, 87.0], [1970, 89.0], [1971, 94.0], [1972, 83.0], [1973, 85.0], [1974, 85.0], [1975, 85.0], [1976, 98.0], [1977, 85.0], [1978, 96.0], [1979, 92.0], [1980, 93.0], [1981, 89.0], [1982, 86.0], [1983, 93.0], [1984, 91.0], [1985, 93.0], [1986, 85.0], [1987, 88.0], [1988, 89.0], [1989, 85.0], [1990, 90.0], [1991, 85.0], [1992, 83.0], [1993, 84.0], [1994, 86.0], [1995, 87.0], [1996, 88.0], [1997, 86.0], [1998, 93.0], [1999, 91.0], [2000, 89.0], [2001, 86.0], [2002, 92.0], [2003, 92.0], [2004, 88.0], [2005, 90.0], [2006, 83.0], [2007, 92.0], [2008, 88.0], [2009, 84.0], [2010, 80.0], [2011, 94.0], [2012, 95.0], [2013, 94.0], [2014, 86.0], [2015, 89.0], [2016, 84.0], [2017, 94.0], [2018, 92.0], [2019, 88.0], [2020, 85.0], [2021, 90.0], [2022, 92.0], [2023, 98.0], [2024, 89.0], [2025, 92.0]]
  },{
    name: 'MeanAvgTemperature',
    type: 'scatter',
    data: [[1873, 53.3], [1874, 60.9], [1875, 57.7], [1876, 57.6], [1877, 64.2], [1878, 61.6], [1879, 57.6], [1880, 59.9], [1881, 60.8], [1882, 63.2], [1883, 57.8], [1884, 63.9], [1885, 60.5], [1886, 59.1], [1887, 58.4], [1888, 58.0], [1889, 59.2], [1890, 58.2], [1891, 67.7], [1892, 63.5], [1893, 62.9], [1894, 64.3], [1895, 66.2], [1896, 57.9], [1897, 68.8], [1898, 64.2], [1899, 59.3], [1900, 61.5], [1901, 60.0], [1902, 57.3], [1903, 58.3], [1904, 59.7], [1905, 63.7], [1906, 65.4], [1907, 58.5], [1908, 68.0], [1909, 61.0], [1910, 61.5], [1911, 59.3], [1912, 61.2], [1913, 61.4], [1914, 62.4], [1915, 60.7], [1916, 59.7], [1917, 60.0], [1918, 55.1], [1919, 64.3], [1920, 65.5], [1921, 64.0], [1922, 65.7], [1923, 63.1], [1924, 57.6], [1925, 65.8], [1926, 57.5], [1927, 63.0], [1928, 58.3], [1929, 59.0], [1930, 62.4], [1931, 68.7], [1932, 61.9], [1933, 67.4], [1934, 57.3], [1935, 62.2], [1936, 66.7], [1937, 63.5], [1938, 62.2], [1939, 64.2], [1940, 65.3], [1941, 63.2], [1942, 57.2], [1943, 58.2], [1944, 61.7], [1945, 60.1], [1946, 59.9], [1947, 63.1], [1948, 67.9], [1949, 58.4], [1950, 62.6], [1951, 56.9], [1952, 63.1], [1953, 62.1], [1954, 60.5], [1955, 63.0], [1956, 58.7], [1957, 59.4], [1958, 62.7], [1959, 63.1], [1960, 61.7], [1961, 59.0], [1962, 56.4], [1963, 62.1], [1964, 58.9], [1965, 52.8], [1966, 60.3], [1967, 60.3], [1968, 61.1], [1969, 63.0], [1970, 61.2], [1971, 62.8], [1972, 57.9], [1973, 60.1], [1974, 55.3], [1975, 57.7], [1976, 61.8], [1977, 60.5], [1978, 67.3], [1979, 63.4], [1980, 59.5], [1981, 60.0], [1982, 60.9], [1983, 62.6], [1984, 57.2], [1985, 59.9], [1986, 59.8], [1987, 62.4], [1988, 62.4], [1989, 60.9], [1990, 64.4], [1991, 58.9], [1992, 59.6], [1993, 55.0], [1994, 64.3], [1995, 60.2], [1996, 62.2], [1997, 62.4], [1998, 66.6], [1999, 61.1], [2000, 61.6], [2001, 60.9], [2002, 65.5], [2003, 62.5], [2004, 67.4], [2005, 66.3], [2006, 59.6], [2007, 64.8], [2008, 63.6], [2009, 66.5], [2010, 60.2], [2011, 62.9], [2012, 63.9], [2013, 67.2], [2014, 62.7], [2015, 67.9], [2016, 66.1], [2017, 67.3], [2018, 65.1], [2019, 66.7], [2020, 61.3], [2021, 66.5], [2022, 66.0], [2023, 69.1], [2024, 70.4], [2025, 67.6]]
  },{
    name: 'Trend — HighestMax',
    type: 'line',
    dashStyle: 'ShortDash',
    data: [[1873.0, 88.4424072659367], [2025.0, 89.42687377981497]]
  },{
    name: 'Trend — MeanAvg',
    type: 'line',
    dashStyle: 'ShortDash',
    data: [[1873.0, 60.19895594601476], [2025.0, 63.49124013241661]]
  }]
});
</script>

<p>
  However, when we look at the average temperatures in September in Minneapolis, things start to change. Here, the Highcharts regression line points sharply upward. The warmest September on record wasn’t back in the Dust Bowl—it was 2024, with an average of 70.4°F, followed closely by 2023 at 69.1°F. Several of the top ten warmest Septembers have all come in the last ten years. In the chart, this shows up as a clustering of high values on the right-hand side. You can clearly see that starting around 2000, those temperatures have been clumping up over the last twenty-five years.
</p>
<p>
  What this means for Minneapolis is that while we may not see record-breaking temperatures or the "hottest day ever" every year, the entire month of September is trending warmer than it used to be. Average conditions are shifting, and that matters for everything from energy use to lake levels and outdoor events. This is where data visualizations are awesome and show why I love them so much. By putting both extremes and averages side by side, the charts show the trends in a simple visual style.
</p>
<p>
  If you are interested in this kind of temperature data, I have a website called <a href="https://todaysrecordhigh.com">Today's Record High</a> that tracks this information for all the weather-exposed areas in the country.
</section>