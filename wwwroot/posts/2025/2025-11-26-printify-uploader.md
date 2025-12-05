---
title: "Programatically create and upload products to Printify"
description: "Manually creating Printify products was taking too long to do manually. I created a Python application to programatically create products and upload them to Printify."
date: "2025-11-26"
draft: true
slug: "printify-uploader"
tags: programming, printify
---

My t-shirt company, Big Little Cities, has over 80 cities in our catalog. When we release a new product type (t-shirt, sweatshirt, etc) I have to manually create each product listing in Printify one by one. This process is tediious and takes a lot of time that I could be spending doiong something productive. Printify has a pretty good API that allows developers to create and manage their products. In this post I'll show you how I utilize the Printify API to get a list of available products and their Ids from Printify (aka the Blueprint Ids) then how to use those Ids to create products via the Printify API.



