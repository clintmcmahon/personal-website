---
title: "How to display a custom icon in React Native Mapbox"
description: "It took me a while to figure out how to add a custom icon to a marker in React Native Mapbox library. This is how you do that."
date: "2025-02-18"
draft: false
slug: "custom-marker-icon-react-native-mapbox"
tags: react-native
---

 <section>
    <p>
        In the latest update to <a href="https://mplscoffee.com" target="_blank">MPLS Coffee</a>, I <a href="/blog/move-from-react-native-maps-to-mapbox">migrated the mapping framework from React Native Maps to Mapbox</a>. After the migration I couldn't get the custom icons to display for the coffee shop markers. Everything I tried didn't work. Then I came across a StackOverflow post where the user was having a problem where only the last icon in their images array was displaying. Images array...I had no idea that was a thing. I can't find that post now, so I'm not able to link to it, but their post had code in it that eventually led me to the right solution - to use the Images array component. So, I must say "Thank you" to the random Internet person for their help. I hope these good vibes get to you someday 🙂.
    </p>
    <p>
        In that user's post they were having a problem loading all their icons in the Images array. That wasn't my problem, but their post had an example that used the <strong>MapboxGL.Images</strong> component. In the examples I was looking at (thanks for nothing ChatGPT) none of them mentioned this component as a way to use custom icons. That was enough to get me what I needed. This code  shows how to get Mapbox in React Native to display custom icons for your marker instead of the default icons.
    </p>
    <p>
        If you follow this pattern for your Mapbox MapView component you will be able to create custom icons for your Mapbox markers. This is just a sampling of running Mapbox in React Native, I left out a lot of information and set up to make this example simple. If you need help implmementing Mapbox in React Native or more information <a class="fw-bold" href="/contact">feel free to drop me a line</> - I love helping people</a>.
    </p>
       <pre><code class="language-jsx">
            
&lt;MapboxGL.MapView&gt;
    &lt;MapboxGL.Camera 
        defaultSettings={{
            centerCoordinate: userCoords,
            zoomLevel: zoomLevel,
        }}
        ref={mapCameraRef}
    /&gt;
    //Create a "custom-marker" row in the MapboxGL.Images array that references the icon you want to use for your markers
    &lt;MapboxGL.Images
        images={{
            "custom-marker": require("../assets/coffee_icon.png")
        }}
    /&gt;
    {filteredShops && filteredShops.features.length > 0 && (
        &lt;MapboxGL.ShapeSource 
            id="shopsSource" 
            shape={filteredShops}
        &gt;
        //Then in The SymbolLayer component reference the "custom-maker" in the iconImage property
            &lt;MapboxGL.SymbolLayer
                id="shopsLayer"
                style={{
                    iconImage: "custom-marker",
                    iconSize: 0.4,
                    iconAllowOverlap: true,
                    iconIgnorePlacement: true,
                }}
            /&gt;
        &lt;/MapboxGL.ShapeSource&gt;
    )}
    
&lt;/MapboxGL.MapView&gt;
</code></pre>        
</section>
