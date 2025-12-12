---
title: "Why I created a bulk product uploader for Printify"
description: "Manually creating Printify products was taking too long to do manually. I created a Python application to programatically create products and upload them to Printify."
date: "2025-12-08"
draft: false
slug: "printify-uploader"
tags: programming, printify
---

Just show me the code: <a href="https://github.com/clintmcmahon/printify-uploader/" target="_blank">View the entire project in Github</a>

My t-shirt company, <a href="https://biglittlecities.com">Big Little Cities</a>, has over 80 cities in our catalog. When we release a new product type (t-shirt, sweatshirt, etc) I have to manually create each product listing in Printify one by one. That's a lot of clicking around. This process is tedious and takes a lot of time. 

Printify has a pretty good API that allows developers to create and manage their products. The current process for creating a new product involves creating a product, manually uploading the artwork, selecting the artwork per variant (color/size combination) and then publishing the product. I decided that I would write a Python app to programmatically create new Printify products, create the variants for each product, upload the associated artwork for each variant and finally assign each variant artwork to the correct product. 

This blog post is about how I utilize the Printify API to get a list of available products and their IDs from Printify (Printify calls these Blueprint IDs - essentially product templates) and then how to use those IDs to create products via the Printify API. What used to take me a couple of days now takes an hour or two.

<h2>The Core Components</h2>
<h3>Overall Structure</h3>
I set up a folder structure that lists all of the cities by their name along with each of the designs as children. For each of our products we have different t-shirt color options (variants). Each variant uses a different design color chosen to match the t-shirt color. The design file is named as a template that the application can easily parse. The naming convention is City_DesignColor_Font_Weight.png. Pretty simple stuff but smart enough that the program will know exactly what to do with it when it's time to parse out the design files.

<img src="/images/2025/biglittlecities_design_folder_designs.png" class="w-100 my-3" alt="Big Little Cities design folder"/>

The app opens the parent folder and loops through each city name. For each city name/city folder new products are created for each design inside the city folder. That means the system scans the folder directory containing the design files, extracts metadata from filenames, uploads the design files as images to Printify's media library, and creates products with variant-specific configurations. Our products are in a specific format, like <strong>Chicago 01 T-Shirt</strong>. Using the pattern of {{City}} 01 T-Shirt, the application can easily scan a subfolder of city names, create the product for each folder name/city and upload the associated artwork.   
    
By doing it this way, I don't need to keep an ongoing list in a CSV file or manually enter each product at the command prompt. Adding a new city to the shop just requires placing the named design files in the city folder executing the batch upload. The folder, product type and variants are all controlled via a <a href="https://github.com/clintmcmahon/printify-uploader/blob/main/product_config.json"><code>product_config.json</code></a> file at the root of the project.

<h3>Single configuration file</h3>
It's all run via a product configuration file called  <a href="https://github.com/clintmcmahon/printify-uploader/blob/main/product_config.json"><code>product_config.json</code></a>, which maintains the product blueprint IDs, print provider, variant configurations, pricing structures and image positioning parameters. The positioning parameters don't work as intended yet - I still have to manually adjust positioning after upload. 

Each variant entry includes:</p>
<ul>
    <li>Printify variant ID (color/size combination)</li>
    <li>Price in cents</li>
    <li>Associated image color identifier</li>
    <li>Positioning properties (x, y, height, scale, angle)</li>
</ul>

<img src="/images/2025/biglittlecities_product_config.png" alt="Product configuration file" class="w-100 py-3" />
    
<p>API credentials are separated from configuration files and stored in environment variables via a <code>.env</code> file, this ensures the secure handling of authentication tokens and allows for easy public hosting on Github.</p>
    
<h4>Blueprint Discovery Utility</h4>
<p>Before you can publish a new product, you need to have what Printify calls a Blueprint ID. The <a href="https://github.com/clintmcmahon/printify-uploader/blob/main/get_blueprint_info.py"><code>get_blueprint_info.py</code></a> tool lists all the different Blueprint IDs that are available in Printify. I created this module because Printify's documentation doesn't make it easy to find valid variant IDs for specific product types. Printify's web interface doesn't expose these identifiers, which are required for programmatic product creation. This utility queries the Printify API catalog, displays available blueprints and print providers, and enumerates all variant IDs with their corresponding color and size attributes.</p>

<h2>Where Printify Falls Short</h2>
    
<h3>Lack of Bulk Operations</h3>
Printify's web interface does not provide bulk product creation or bulk image upload. Printify does have a image gallery, however there is no way to upload multiple images at a time to this gallery. Each image uploaded to the gallery is done one by one from their design studio. Even if you are not programatically creating new products, having the ability to bulk upload your design files to Printify saves a huge chunk of time. 

Creating a new product requires a bunch of extra clicks, having the bulk uploader just run through and create products is great.
    
<h3>Hard to find variant and product identifiers</h3>
Variant IDs and product IDs are not exposed in the web interface, making it harder to set up the Printify API calls. That's why I wrote the Blueprint module which will spit out the product IDs that are needed to create products via the Printify API.
    
<h3>Difficult Positioning Tool</h3>
Design positioning in the web interface relies on visual drag-and-drop controls that work with percentages instead of inches. The API doesn't work the same way as the UI drag and drop tool so I found it difficult to accurately place the designs on the t-shirts programmatically. This could very well be a me problem and not a Printify problem. I'm working more on this feature as time allows.

<h2>Conclusion</h2>
This application addresses some gaps in my Printify product creation process and definitely speeds up the amount of time it takes to create new products and variants. The folder-driven architecture, configuration files, and programmatic design placement enable me to scale creating products that would otherwise be frustrating to manually create for every city.

<a href="https://github.com/clintmcmahon/printify-uploader/" target="_blank">View the entire project in Github</a>




