---
title: "Mapbox GL JS Access Denied In IE 11"
description: ""
date: "2020-06-22"
draft: false
slug: "mapbox-gl-js-access-denied-in-ie-11"
tags:
---

<!--kg-card-begin: html-->
<p>Mapbox gives you the ability to load custom icons for different points in your maps. When I tried to load a custom icon to a Mapbox map I got the error &#8220;SCRIPT5: Access is denied.&#8221; in IE 11 &#8211; Firefox and Chrome loaded the icon fine.&nbsp; My code was loading an imported image as an object inside a React component like this:</p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="javascript" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">import centerImage from "./images/centerImage.png";

map.on('load', () => {
map.loadImage(centerImage, (error, image) => {
if (error) {
return;
}
map.addImage('centerIcon', image);
});
...
});

</pre></div>

<p>Updating the image URL parameter to a string URL fixed the problem. I didn&#8217;t dig too much into the issue to figure out why this was happening, but passing a valid image string URL was enough to get passed this issue.</p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="javascript" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">map.on('load', () => {
    map.loadImage('/hospital.png', (error, image) => {
        if (error) {
            return;
        }
        map.addImage('centerIcon', image);
    });
    ...
});</pre></div>
<!--kg-card-end: html-->
