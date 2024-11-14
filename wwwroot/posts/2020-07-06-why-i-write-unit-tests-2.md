---
title: "Why I Write Unit Tests"
description: ""
date: "2020-07-06"
draft: false
slug: "why-i-write-unit-tests-2"
tags:
---

<!--kg-card-begin: html--><p><!--kg-card-begin: html--></p>
<blockquote>
<p>In <a title="" href="https://en.wikipedia.org/wiki/Computer_programming">computer programming</a>, <b>unit testing</b> is a <a title="Software testing" href="https://en.wikipedia.org/wiki/Software_testing">software testing</a> method by which individual units of <a title="Source code" href="https://en.wikipedia.org/wiki/Source_code">source code</a>, sets of one or more computer program modules together with associated control data, usage procedures, and operating procedures, are tested to determine whether they are fit for use &#8212; <a href="https://en.wikipedia.org/wiki/Unit_testing" target="_blank" rel="noopener noreferrer">wikipedia</a></p>
</blockquote>
<p>Unit tests, who needs em? We all do. There are a lot of resources online that will tell you why unit testing is important or why writing unit tests before building implementations is a good idea. But I&#8217;m here to tell you how unit tests can save you a massive production headache.</p>
<p>A while back, a coworker pushed code to production that consumed and parsed large .csv files uploaded by users. The code looped through the file then parsed and mapped each record to the associated record in the model. The model contained a few bool fields that were populated by executing the C# Boolean.TryParse method against the text field to convert the string from the file to bool. If the TryParse method returned false then the value in the csv file must have been <a href="https://developer.mozilla.org/en-US/docs/Glossary/Falsy" target="_blank" rel="noopener noreferrer"><strong>falsy</strong></a>, and vice versa for true. The record was applied to the model and saved to the database.</p>
<p>As it turned out, our code didn&#8217;t work as expected which caused a major problem. For bool fields users would upload either a 1 or 0. 1 being true and 0 being false. Because the source was a csv file all values were imported as strings. Therefore, before applying the values to our model, we must convert the string representation to the appropriate type within the model.</p>
<p>Here&#8217;s an example of the code, the Parse method below takes a string argument named parsable, does a Boolean.TryParse against parsable and returns the newly converted value.</p>
<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace">
<pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="csharp" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">		public static Main(){	    var trueFalse = Parse("1");	}		public bool Parse(string parsable)	{		 bool flag;		 bool result = Boolean.TryParse(parsable, out flag);         return flag;	}</pre>
</div>
<p>From looking at the method and without knowing how C# Boolean.TryParse converts the string argument &#8220;1&#8221; to bool you&#8217;d think that this method returns <strong>true</strong>, not false. But in reality the above method returns <strong>false</strong>. Even though 1 generally equals true in most contexts, if passed in as a string the result is false, not true.</p>
<p>If we would have had proper unit tests set up we would have written a unit test that looks something like this:</p>
<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace">
<pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="csharp" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">[TestMethod]public void Parse_BooleanTryParseString__ReturnsFalse(){    string stringToParse = "1";    bool expected = true;    bool actual = Parse(stringToParse);    Assert.AreEqual(expected, actual);}</pre>
</div>
<p>The above unit test would have failed and we&#8217;d have found the problem, fixed it and been on our way. Unfortunately for us, we didn&#8217;t have a unit test for the above Parse method and QA didn&#8217;t catch the issue during their testing. The code eventually made its way to production, users start uploading files against the new code and yadda, yadda, yadda&#8230;we end up with a lot false values that should be true. </p>
<div class="wp-block-image">
<figure class="aligncenter size-large"><img decoding="async" loading="lazy" width="600" height="433" src="http://clintmcmahon.com/content/images/wordpress/2020/07/35nksf.jpg" alt="I've made a huge mistake" class="wp-image-732" sizes="(max-width: 600px) 100vw, 600px" /></figure>
</div>
<p>The good news is we were able to recover the lost data. But the recovery process took a few days and a lot of manual work which set us back on our timeline, not to mention created a garbage truck full of unwanted stress while we scrambled to recover the data and explain to our stakeholders that we&#8217;d made a huge mistake.</p>
<p>The takeaway my team and I had from this experience is how valuable a unit test can be. <a aria-label="undefined (opens in a new tab)" href="https://docs.microsoft.com/en-us/visualstudio/test/unit-test-basics?view=vs-2019" target="_blank" rel="noreferrer noopener nofollow">Unit tests</a> are not just a <a aria-label="undefined (opens in a new tab)" href="http://softwaretestingfundamentals.com/software-testing-levels/" target="_blank" rel="noreferrer noopener nofollow">key part of the testing process</a>, but they&#8217;ve proven to be invaluable in making sure the code you write is actually doing what you think it&#8217;s doing.</p>
<p><!--kg-card-end: html--></p>
<!--kg-card-end: html-->
