---
title: "FizzBuzz in C#"
description: ""
date: "2023-12-03"
draft: false
slug: "fizzbuzz-in-c"
tags:
---

<!--kg-card-begin: html-->

<p><a href="https://news.ycombinator.com/item?id=38503486" data-type="link" data-id="https://news.ycombinator.com/item?id=38503486" target="_blank" rel="noreferrer noopener">The comments on this Hacker News post</a> generated a bunch of opinions on the ability for developers to solve a FizzBuzz problem. Specifically in a coding interview. I’ve never actually seen what the FizzBuzz problem was, so I had to look it up. In this blog post I solve FizzBuzz in C#. </p>

<p>The FizzBuzz problem is a challenge for a developer to write a method, in any language, to return different parts of the word FizzBuzz. </p>

<p>It goes like this:</p>

<ul>
<li>If a number is divisible by 3 or 5 => return FizzBuzz</li>

<li>If a number is only divisible by 3 => return Fizz</li>

<li>If a number is divisible by 5 => return Buzz</li>

<li>If a number is not divisible by 3 or 5 => return the number</li>
</ul>

<p>Over twenty years of programming experience and I’ve never come across question in an interview so I thought it would be fun to jump onto <a href="https://dotnetfiddle.net/" data-type="link" data-id="https://dotnetfiddle.net/" target="_blank" rel="noreferrer noopener">.Net Fiddle</a> to give it a try. In this function you take any number and check if it can be divisible by 3 or 5. The trick to this function is to use the <code>% mod</code> operator to check if a number is divisible by another number. The <code>mod</code> operator will return a number if there is a remainder after division between the two supplied numbers. If the number returned after division using the <code>mod</code> operator you will return zero – meaning there isn’t a remainder after division. This is what the function is all about. Here’s my FizzBuzz implementation I wrote this morning after reading the Hacker News post. </p>

<h2 class="wp-block-heading">Simple FizzBuzz implementation in C#</h2>

<div class="wp-block-kevinbatdorf-code-block-pro" data-code-block-pro-font-family="Code-Pro-JetBrains-Mono" style="font-size:.875rem;font-family:Code-Pro-JetBrains-Mono,ui-monospace,SFMono-Regular,Menlo,Monaco,Consolas,monospace;line-height:1.25rem;--cbp-tab-width:2;tab-size:var(--cbp-tab-width, 2)"><span style="display:block;padding:16px 0 0 16px;margin-bottom:-1px;width:100%;text-align:left;background-color:#1E1E1E"><svg xmlns="http://www.w3.org/2000/svg" width="54" height="14" viewBox="0 0 54 14"><g fill="none" fill-rule="evenodd" transform="translate(1 1)"><circle cx="6" cy="6" r="6" fill="#FF5F56" stroke="#E0443E" stroke-width=".5"></circle><circle cx="26" cy="6" r="6" fill="#FFBD2E" stroke="#DEA123" stroke-width=".5"></circle><circle cx="46" cy="6" r="6" fill="#27C93F" stroke="#1AAB29" stroke-width=".5"></circle></g></svg></span><span role="button" tabindex="0" data-code="public class Program
{
	public static void Main()
	{
		int i = 0;
		while(i <=100)
		{
			string fizzWord = GetFizz(i);
			Console.WriteLine(fizzWord);
			i = i + 1;
		}
		
	}
	
	public static string GetFizz(int i)
	{
		if(i % 3 == 0 && i % 5==0)
		{
		  return "FizzBuzz";
		}
		else if(i % 3 == 0)
		{
			return "Fizz";
		}
		else if(i % 5 == 0)
		{
			return "Buzz";
		}
		
		return i.ToString();
	}
}" style="color:#D4D4D4;display:none" aria-label="Copy" class="code-block-pro-copy-button"><svg xmlns="http://www.w3.org/2000/svg" style="width:24px;height:24px" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path class="with-check" stroke-linecap="round" stroke-linejoin="round" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4"></path><path class="without-check" stroke-linecap="round" stroke-linejoin="round" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2"></path></svg></span><pre class="shiki dark-plus" style="background-color: #1E1E1E" tabindex="0"><code><span class="line"><span style="color: #569CD6">public</span><span style="color: #D4D4D4"> </span><span style="color: #569CD6">class</span><span style="color: #D4D4D4"> </span><span style="color: #4EC9B0">Program</span></span>
<span class="line"><span style="color: #D4D4D4">{</span></span>
<span class="line"><span style="color: #D4D4D4">	</span><span style="color: #569CD6">public</span><span style="color: #D4D4D4"> </span><span style="color: #569CD6">static</span><span style="color: #D4D4D4"> </span><span style="color: #569CD6">void</span><span style="color: #D4D4D4"> </span><span style="color: #DCDCAA">Main</span><span style="color: #D4D4D4">()</span></span>
<span class="line"><span style="color: #D4D4D4">	{</span></span>
<span class="line"><span style="color: #D4D4D4">		</span><span style="color: #569CD6">int</span><span style="color: #D4D4D4"> </span><span style="color: #9CDCFE">i</span><span style="color: #D4D4D4"> = </span><span style="color: #B5CEA8">0</span><span style="color: #D4D4D4">;</span></span>
<span class="line"><span style="color: #D4D4D4">		</span><span style="color: #C586C0">while</span><span style="color: #D4D4D4">(</span><span style="color: #9CDCFE">i</span><span style="color: #D4D4D4"> <=</span><span style="color: #B5CEA8">100</span><span style="color: #D4D4D4">)</span></span>
<span class="line"><span style="color: #D4D4D4">		{</span></span>
<span class="line"><span style="color: #D4D4D4">			</span><span style="color: #569CD6">string</span><span style="color: #D4D4D4"> </span><span style="color: #9CDCFE">fizzWord</span><span style="color: #D4D4D4"> = </span><span style="color: #DCDCAA">GetFizz</span><span style="color: #D4D4D4">(</span><span style="color: #9CDCFE">i</span><span style="color: #D4D4D4">);</span></span>
<span class="line"><span style="color: #D4D4D4">			</span><span style="color: #9CDCFE">Console</span><span style="color: #D4D4D4">.</span><span style="color: #DCDCAA">WriteLine</span><span style="color: #D4D4D4">(</span><span style="color: #9CDCFE">fizzWord</span><span style="color: #D4D4D4">);</span></span>
<span class="line"><span style="color: #D4D4D4">			</span><span style="color: #9CDCFE">i</span><span style="color: #D4D4D4"> = </span><span style="color: #9CDCFE">i</span><span style="color: #D4D4D4"> + </span><span style="color: #B5CEA8">1</span><span style="color: #D4D4D4">;</span></span>
<span class="line"><span style="color: #D4D4D4">		}</span></span>
<span class="line"><span style="color: #D4D4D4">		</span></span>
<span class="line"><span style="color: #D4D4D4">	}</span></span>
<span class="line"><span style="color: #D4D4D4">	</span></span>
<span class="line"><span style="color: #D4D4D4">	</span><span style="color: #569CD6">public</span><span style="color: #D4D4D4"> </span><span style="color: #569CD6">static</span><span style="color: #D4D4D4"> </span><span style="color: #569CD6">string</span><span style="color: #D4D4D4"> </span><span style="color: #DCDCAA">GetFizz</span><span style="color: #D4D4D4">(</span><span style="color: #569CD6">int</span><span style="color: #D4D4D4"> </span><span style="color: #9CDCFE">i</span><span style="color: #D4D4D4">)</span></span>
<span class="line"><span style="color: #D4D4D4">	{</span></span>
<span class="line"><span style="color: #D4D4D4">		</span><span style="color: #C586C0">if</span><span style="color: #D4D4D4">(</span><span style="color: #9CDCFE">i</span><span style="color: #D4D4D4"> % </span><span style="color: #B5CEA8">3</span><span style="color: #D4D4D4"> == </span><span style="color: #B5CEA8">0</span><span style="color: #D4D4D4"> && </span><span style="color: #9CDCFE">i</span><span style="color: #D4D4D4"> % </span><span style="color: #B5CEA8">5</span><span style="color: #D4D4D4">==</span><span style="color: #B5CEA8">0</span><span style="color: #D4D4D4">)</span></span>
<span class="line"><span style="color: #D4D4D4">		{</span></span>
<span class="line"><span style="color: #D4D4D4">		  </span><span style="color: #C586C0">return</span><span style="color: #D4D4D4"> </span><span style="color: #CE9178">"FizzBuzz"</span><span style="color: #D4D4D4">;</span></span>
<span class="line"><span style="color: #D4D4D4">		}</span></span>
<span class="line"><span style="color: #D4D4D4">		</span><span style="color: #C586C0">else</span><span style="color: #D4D4D4"> </span><span style="color: #C586C0">if</span><span style="color: #D4D4D4">(</span><span style="color: #9CDCFE">i</span><span style="color: #D4D4D4"> % </span><span style="color: #B5CEA8">3</span><span style="color: #D4D4D4"> == </span><span style="color: #B5CEA8">0</span><span style="color: #D4D4D4">)</span></span>
<span class="line"><span style="color: #D4D4D4">		{</span></span>
<span class="line"><span style="color: #D4D4D4">			</span><span style="color: #C586C0">return</span><span style="color: #D4D4D4"> </span><span style="color: #CE9178">"Fizz"</span><span style="color: #D4D4D4">;</span></span>
<span class="line"><span style="color: #D4D4D4">		}</span></span>
<span class="line"><span style="color: #D4D4D4">		</span><span style="color: #C586C0">else</span><span style="color: #D4D4D4"> </span><span style="color: #C586C0">if</span><span style="color: #D4D4D4">(</span><span style="color: #9CDCFE">i</span><span style="color: #D4D4D4"> % </span><span style="color: #B5CEA8">5</span><span style="color: #D4D4D4"> == </span><span style="color: #B5CEA8">0</span><span style="color: #D4D4D4">)</span></span>
<span class="line"><span style="color: #D4D4D4">		{</span></span>
<span class="line"><span style="color: #D4D4D4">			</span><span style="color: #C586C0">return</span><span style="color: #D4D4D4"> </span><span style="color: #CE9178">"Buzz"</span><span style="color: #D4D4D4">;</span></span>
<span class="line"><span style="color: #D4D4D4">		}</span></span>
<span class="line"><span style="color: #D4D4D4">		</span></span>
<span class="line"><span style="color: #D4D4D4">		</span><span style="color: #C586C0">return</span><span style="color: #D4D4D4"> </span><span style="color: #9CDCFE">i</span><span style="color: #D4D4D4">.</span><span style="color: #DCDCAA">ToString</span><span style="color: #D4D4D4">();</span></span>
<span class="line"><span style="color: #D4D4D4">	}</span></span>
<span class="line"><span style="color: #D4D4D4">}</span></span></code></pre></div>

<!--kg-card-end: html-->
