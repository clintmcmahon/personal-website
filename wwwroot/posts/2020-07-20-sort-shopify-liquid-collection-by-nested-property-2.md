---
title: "Sort Shopify Liquid Collection By A Nested Property"
description: ""
date: "2020-07-20"
draft: false
slug: "sort-shopify-liquid-collection-by-nested-property-2"
tags:
---

<!--kg-card-begin: html--><p><!--kg-card-begin: html--></p>
<p>Suppose you want to sort or filter a <a href="https://shopify.com" target="_blank" rel="noreferrer noopener nofollow" title="https://shopify.com">Shopify</a> collection by a sub/nested property. Well, this is an example of how you&#8217;d sort a Shopify collection by a nested property, specifically how to sort by a setting property of a logo list block. </p>
<figure class="wp-block-image size-large"><img decoding="async" loading="lazy" width="1024" height="613" src="http://clintmcmahon.com/content/images/wordpress/2020/08/Capture.jpg" alt="Shopify tab example" class="wp-image-781" sizes="(max-width: 1024px) 100vw, 1024px" /><figcaption>Shopify filter tab example</figcaption></figure>
<p>I created a Shopify tab snippet to filter and sort the above list of logo blocks by a property within a single logo/block. The code below gets all the blocks in the logo list section, maps over the settings and returns the items where the logo_list property equals &#8220;brewery&#8221; then sorts the results by title.  If you don&#8217;t need to return a specific sub-set of data and only want to sort by the property then simply remove <em>| where: &#8220;logo_category&#8221;, &#8220;brewery&#8221;</em> from the assign block.</p>
<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace">
<pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="ruby" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">{% assign breweries = section.blocks | map: 'settings' | where: "logo_category", "brewery" | sort: "title" %}{% for block in breweries %}    {%- assign logo = block.logo -%}    {%- assign url = block.url -%}        &lt;div class="module-inline-item dynamic-logo-list-item" {{ block.shopify_attributes }}>            {% if url != blank %}              &lt;a href="{{ url }}" target="_blank">            {% endif %}            {% if block.logo != blank %}              {%                include 'rimg',                img: logo,                size: '330x280',                lazy: true              %}            {% else %}              {{ 'logo' | placeholder_svg_tag: 'placeholder-svg' }}            {% endif %}            {% if url != blank %}                &lt;/a>            {% endif %}          &lt;/div>      {% endfor %}</pre>
</div>
<p>And that&#8217;s all you need to do to filter a Shopify collection by a nested property. I will post the code for the full tab snippet in another post.</p>
<p><!--kg-card-end: html--></p>
<!--kg-card-end: html-->
