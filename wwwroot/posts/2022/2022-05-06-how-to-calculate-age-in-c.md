---
title: "How to calculate age in C#"
description: ""
date: "2022-05-06"
draft: false
slug: "how-to-calculate-age-in-c"
tags:
---

<!--kg-card-begin: html-->
<p>Today I had a requirement to calculate someone&#8217;s age based on that person&#8217;s birthday. It seems like a pretty simple function to write, but I decided to Google around a little bit anyway to see if what other people were doing to calculate age based on birthday. Turns out, a few people have ideas about this. The first Google result is a Stack Overflow post titled <a href="https://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-based-on-a-datetime-type-birthday"><em>How do I calculate someone&#8217;s age based on a DateTime type birthday?</em></a> that is full of ways to calculate the age. The post is over 13 years old but is plum full with so many different methods of calculating an age based off of a birthday. </p>

<p>Below are two ways that I found to be popular in the post as well as a pretty cool library that calculates age in C# with a single method call. </p>

<p><strong>//TLDR;</strong> There is a great .Net 5+ library called <a href="https://www.nuget.org/packages/AgeCalculator/" target="_blank" rel="noreferrer noopener">AgeCalculator</a> that calculates age based off of two DateTime values. Unfortunately, as far as I can tell the AgeCalculator library is only available for .Net 5+. So, in if you&#8217;re on an older version of .Net you should take a look at <a href="https://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-based-on-a-datetime-type-birthday" target="_blank" data-type="URL" data-id="https://stackoverflow.com/questions/9/how-do-i-calculate-someones-age-based-on-a-datetime-type-birthday" rel="noreferrer noopener">this Stack Overflow thread</a> or the second example below for ways to calculate age in C#.</p>

<h2>Calculating age based on year alone</h2>

<p>My initial thought was to find age by subtracting the birthday year from today&#8217;s year. However, I quickly realized that doesn&#8217;t work so well, unless the two dates are the exact same day. Since that is not the case, this method does not take into account the offset in days and months as well as other factors(leap years, etc) so this is not best way to make this calculation.</p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="csharp" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">using System;
					
public class Program
{
	public static void Main()
	{
		var birthday = new DateTime(1980,12,11);
		var age = DateTime.Now.Year - birthday.Year;
		Console.WriteLine(age);
		//Output: 42
	}
}</pre></div>

<h2>Calculating age based on number of days </h2>

<p>The method that seems to work well is by James Current from this <a href="https://stackoverflow.com/a/168703/118144" data-type="URL" data-id="https://stackoverflow.com/a/168703/118144" target="_blank" rel="noreferrer noopener">Stack Overflow comment</a> which takes into consideration the days/months between now and the birthday. This seems to be the preferred method to return the accurate age based off the birthday. If you cannot use the <a href="https://www.nuget.org/packages/AgeCalculator/" data-type="URL" data-id="https://www.nuget.org/packages/AgeCalculator/" target="_blank" rel="noreferrer noopener">AgeCalculator</a> package for .Net 5+ this could be a valid option. </p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="csharp" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">using System;
					
public class Program
{
	public static void Main()
	{
		var birthday = new DateTime(1980,12,11);
	    int age = (int) ((DateTime.Now - birthday).TotalDays/365.242199);
		Console.WriteLine(age);
		//Output: 41
	}
}</pre></div>

<h2>Calculating age using AgeCalculator library</h2>

<p>For .Net 5+ code bases you can use this AgeCalculator library. As you can see, you have access immediately to the age based off a new object&#8217;s constructor.</p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="csharp" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">using System;
					
public class Program
{
	public static void Main()
	{
		var birthday = new DateTime(1980,12,11);
	    var age = new Age(birthday, DateTime.Today);
    	Console.WriteLine(age.Years);
		//Output: 41
	}
}
 
</pre></div>

<h2>Other ways to use this library</h2>

<p>Based on the documentation of the library there are a whole lot of ways you can use this to calculate age. Here are just a few ways that I took from the README file on the AgeCalculator <a href="https://github.com/arman-g/AgeCalculator#how-to-use-c-code" data-type="URL" data-id="https://github.com/arman-g/AgeCalculator#how-to-use-c-code" target="_blank" rel="noreferrer noopener">library&#8217;s Github page</a>. It seems like a pretty extensible library to use for dates, I&#8217;m excited to use this in more projects.</p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="csharp" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">/* There are three ways to calculate the age between two dates */
using AgeCalculator;
using AgeCalculator.Extensions;

public void PrintMyAge()
{
// Date of birth or from date.
var dob = DateTime.Parse("10/03/2015");

    // #1. Using the Age class constructor.
    var age = new Age(dob, DateTime.Today); // as of 09/19/2021
    Console.WriteLine($"Age: {age.Years} years, {age.Months} months, {age.Days} days, {age.Time}");

    // #2. Using DateTime extension.
    age = dob.CalculateAge(DateTime.Today); // as of 09/19/2021
    Console.WriteLine($"Age: {age.Years} years, {age.Months} months, {age.Days} days, {age.Time}");

    // #3. Using the Age type's static function.
    age = Age.Calculate(dob, DateTime.Today); // as of 09/19/2021
    Console.WriteLine($"Age: {age.Years} years, {age.Months} months, {age.Days} days, {age.Time}");

}

// Output:
// Age: 5 years, 11 months, 16 days, 00:00:00</pre></div>

<!--kg-card-end: html-->
