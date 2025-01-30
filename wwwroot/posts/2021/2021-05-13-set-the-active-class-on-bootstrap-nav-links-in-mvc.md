---
title: "Set The Active Class On Bootstrap Nav Links In MVC"
description: ""
date: "2021-05-13"
draft: false
slug: "set-the-active-class-on-bootstrap-nav-links-in-mvc"
tags:
---

<!--kg-card-begin: html--><p>Here&#8217;s a quick way to set the <code class="EnlighterJSRAW" data-enlighter-language="css">active</code> class on the current URL with a .Net MVC application.</p>
<p>1. Set a string variable equal to the request path at the top of the layout or view:</p>
<pre class="EnlighterJSRAW" data-enlighter-language="csharp">@{ 
    string path = Context.Request.Path;
}</pre>
<p>2. Check the path variable against whatever the base url of the nav-link is. Add the &#8220;active&#8221; class if the path starts with your base path.</p>
<pre class="EnlighterJSRAW" data-enlighter-language="csharp">&lt;a class="nav-link @(path.StartsWith("/robots") ? "active" : "")" href="/robots"&gt;Robots&lt;/a&gt;</pre>
<p>Now you will get the &#8220;active&#8221; highlight class on the correct nav-link when the current request matches the appropriate URL.</p>
<!--kg-card-end: html-->
