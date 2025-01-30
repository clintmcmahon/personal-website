---
title: "There was an error running the selected code generator in .Net Core 5"
description: ""
date: "2021-01-15"
draft: false
slug: "there-was-an-error-running-the-selected-code-generator-in-net-core-5-2"
tags:
---

<!--kg-card-begin: html--><p><!--kg-card-begin: html--></p>
<p>I&#8217;m working on a new .Net Core 5 web app with user authentication where I need to customize some of the Identity account pages (Login, Register, Forgot Password, etc). Out of the box these pages are built into .Net Core so there&#8217;s nothing you need to do to use them. However, if you want to customize any of the account pages you&#8217;ll need to scaffold the source of those pages into your project.</p>
<p>To scaffold items in Visual Studio 2019 &#8211; (Version 16.8.4 as of today) right the project or parent folder then select &#8220;Add &#8211;&gt; New Scaffolded Item&#8221;. This has worked for me in the past but recently in .Net Core 5 Visual Studio throws this error during the scaffolding process:</p>
<p><strong>&#8220;There was an error running the selected code generator: &#8216;Package restored failed. Rolling back package changes for &#8216;Your App&#8217;.&#8221; </strong></p>
<p>I found a workaround for this error by using the <a href="https://docs.microsoft.com/en-us/dotnet/core/tools/" target="_blank" rel="noopener">dotnet CLI</a> outside of Visual Studio to execute the scaffolding tool. The following steps use the aspnet-codegenerator tool to scaffold the full Identity pages area into your .Net Core 5 app.</p>
<ol>
<li>Close Visual Studio.</li>
<li>Open a command prompt and change directories to the project location.</li>
<li>Make sure the <a href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/tools/dotnet-aspnet-codegenerator?view=aspnetcore-5.0" target="_blank" rel="noopener">aspnet-codegenerator</a> tool is installed on your machine by executing this command:
<pre class="EnlighterJSRAW" data-enlighter-language="powershell" data-enlighter-theme="atomic">dotnet tool install -g dotnet-aspnet-codegenerator</pre>
</li>
<li>Add the <strong>Microsoft.VisualStudio.Web.CodeGeneration.Design</strong> package to the project if it does not already exist in your project:
<pre class="EnlighterJSRAW" data-enlighter-language="powershell">Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design</pre>
</li>
<li>Run the following command where <strong>MyApp.Models.ApplicationDbContext</strong> is the namespace to your DbContext:
<pre class="EnlighterJSRAW" data-enlighter-language="powershell">dotnet aspnet-codegenerator identity -dc MyApp.Models.ApplicationDbContext</pre>
</li>
</ol>
<p>If the command completed without errors that should have fixed the &#8220;There was an error running the selected code generator&#8221; issue and created the necessary Identity Pages under Areas/Identity/Pages.</p>
<p><img decoding="async" loading="lazy" class="alignnone size-full wp-image-962" src="http://clintmcmahon.com/content/images/wordpress/2021/01/Capture.jpg" alt="MVC Scaffold Identity Pages" sizes="(max-width: 434px) 100vw, 434px" / sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></p>
<p><strong>dotnet aspnet-codegenerator</strong> also has the ability to scaffold only specific files versus all the Identity files if you don&#8217;t need the full set by passing in the -files parameter followed by the files you want to create. (Thanks to Nick for giving me a heads up in the comments about this parameter).</p>
<pre class="EnlighterJSRAW" data-enlighter-language="powershell">dotnet aspnet-codegenerator identity -dc MyApp.Models.ApplicationDbContext –files “Account.Register;Account.Login;Account.Logout”</pre>
<p>&nbsp;</p>
<p><!--kg-card-end: html--></p>
<!--kg-card-end: html-->
