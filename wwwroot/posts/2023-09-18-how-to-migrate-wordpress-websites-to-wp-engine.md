---
title: "How To Migrate WordPress Websites to WP Engine"
description: ""
date: "2023-09-18"
draft: false
slug: "how-to-migrate-wordpress-websites-to-wp-engine"
tags:
---

<!--kg-card-begin: html-->
<p>This blog post outlines how to migrate a WordPress website from any hosting provider to WP Engine hosting account without access to source FTP site. The post focuses around utilizing the WP Engine Migration Assistant Plugin. WP Engine offers a specialized plug in to make WordPress migrations a pretty simple from one hosting provider to another. In this example it doesn&#8217;t matter what your hosting provider is, this plug in should work with any of them. And best of all, you don&#8217;t need access to the source FTP site to do the migration.</p>

<h2 class="wp-block-heading">Managed Hosting for Business Operations</h2>

<p>A large part of my business is providing an array of WordPress hosting services that cover all the backend configurations, including SSL installations, plugin updates, and server-side optimizations for my clients&#8217; websites. This frees them up to focus on their business and what they do best instead of having to struggle working on a system they dont&#8217; understand nor do they care about understanding it. Part of my service offering is migrating WordPress sites from their old hosting provider to my own managed WP Engine hosting service.</p>

<p>Let&#8217;s get started.</p>

<h2 class="wp-block-heading">Prerequisites</h2>

<ul>
<li>Access to the existing WordPress site’s admin panel</li>

<li>WP Engine account with appropriate permissions</li>
</ul>

<h2 class="wp-block-heading">Migration Using WP Engine Migration Assistant Plugin</h2>

<p>This is how you utilize the WP Engine Migration Assistant Plug in to migrate just about any WordPress site to your WP Engine hosting account.</p>

<h3 class="wp-block-heading">Step 1: WP Engine Plugin Installation</h3>

<ol>
<li>Log into the existing/source WordPress site.</li>

<li>Navigate to <code>Plugins -&gt; Add New</code> on the WordPress Dashboard.</li>

<li>Search for <code>WP Engine Automated Migration</code>. Or you can <a href="https://wordpress.org/plugins/wp-site-migrate/" data-type="link" data-id="https://wordpress.org/plugins/wp-site-migrate/">download the plug in here</a>.</li>

<li>Click <code>Install</code> and then <code>Activate</code>. </li>
</ol>

<h3 class="wp-block-heading">Step 2: Retrieve Migration Credentials</h3>

<ol>
<li>Log in to the WP Engine User Portal.</li>

<li>Create the new site environment if you have not already done so. </li>

<li>Navigate to new WP Engine site and click<code> Migrations</code> in the left side navigation menu.</li>

<li>You&#8217;ll see a screen that looks like this one below. Here you see the site properties that you&#8217;ll need to input into the source/existing website migration plug in. You can generate the migration SFTP credentials here if you have not already done so.</li>
</ol>

<figure class="wp-block-image size-large"><img loading="lazy" decoding="async" src="/images/wordpress/2023/09/Screenshot-2023-09-18-at-4.20.42-PM-1024x711.png" alt="WP Engine Migrate website" class="wp-image-1245"/ sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<h3 class="wp-block-heading">Step 3: Plugin Configuration</h3>

<ol>
<li>On the existing/source WordPress Dashboard, go to<code> WP Engine Migratio</code>n from the left side navigation.</li>

<li>Input the migration credentials from Step 2.</li>

<li>Click <code>Start Migration</code>.</li>
</ol>

<figure class="wp-block-image size-large"><img loading="lazy" decoding="async" src="/images/wordpress/2023/09/Screenshot-2023-09-18-at-4.20.25-PM-1024x717.png" alt="" class="wp-image-1248"/ sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<h3 class="wp-block-heading">Step 4: Monitoring and Verification</h3>

<ol>
<li>Monitor migration status in the WP Engine User Portal.</li>

<li>You&#8217;ll receive a confirmation email upon successful migration.</li>
</ol>

<figure class="wp-block-image size-large"><img loading="lazy" decoding="async" src="/images/wordpress/2023/09/Screenshot-2023-09-18-at-4.22.38-PM-1024x692.png" alt="WP Engine migration status" class="wp-image-1246"/ sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<h3 class="wp-block-heading">Step 5: DNS Update</h3>

<ol>
<li>Now update the DNS records to point to WP Engine&#8217;s IP address or CNAME records.</li>
</ol>

<h2 class="wp-block-heading">Conclusion</h2>

<p>This blog post provides a step by step approach for migrating a WordPress website from any provider to WP Engine utilizing the WP Engine Migration plug in.</p>

<p>For more specialized migration services or hosting solutions, feel free to contact me. Happy migrating!</p>
<!--kg-card-end: html-->
