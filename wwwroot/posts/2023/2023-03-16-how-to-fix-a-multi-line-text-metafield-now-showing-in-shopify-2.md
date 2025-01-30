---
title: "How to fix a multi-line text Metafield now showing in Shopify"
description: ""
date: "2023-03-16"
draft: false
slug: "how-to-fix-a-multi-line-text-metafield-now-showing-in-shopify-2"
tags:
---

<!--kg-card-begin: html--><p><!--kg-card-begin: html--></p>
<p>Here&#8217;s how to how to fix a multi-line text Metafield now showing in Shopify. It&#8217;s not really a fix, more like the correct way to display this field inside a Liquid file. When trying to display a custom Metafield of a product in Shopify the documentation online indicates the correct way is to use this format: </p>
<p><strong>{{ product.metafields.{namespace}.{meta_field_name}.value }} </strong></p>
<p>Example: <strong>{{ product.metafields.global.meta_product_summary.value }}.</strong></p>
<p>This format works great for single line Metafields, but when trying to do this with a multi-line Metafield nothing is going to show up. Instead, all you have to do is drop the <strong>.value</strong> from your code and the multi-line text will now show up. Even though the Metafield App will give you a text string with .value when you click &#8216;Copy liquid code&#8217; that only works for single line Metafields. </p>
<p>Here&#8217;s an example for a product that has a meta_product_summary Metafield: </p>
<p><strong>{{ product.metafields.global.meta_product_summary}}</strong>. </p>
</p>
<p>That&#8217;s all! </p>
<p><!--kg-card-end: html--></p>
<!--kg-card-end: html-->
