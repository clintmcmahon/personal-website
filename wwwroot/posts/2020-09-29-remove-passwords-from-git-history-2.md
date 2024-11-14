---
title: "Remove Passwords From Git History"
description: ""
date: "2020-09-29"
draft: false
slug: "remove-passwords-from-git-history-2"
tags:
---

<!--kg-card-begin: html--><p><!--kg-card-begin: html--></p>
<p>Every once in a while you&#8217;ll check in a key or password into a git repository by mistake. Not to worry, there&#8217;s a great utility that can erase the values from history for you. You can delete the file or update the text from the current HEAD but the value still exists in your branch history so you need to go further to remove the value.</p>
<p>Luckily there&#8217;s the <a href="https://rtyley.github.io/bfg-repo-cleaner/" target="_blank" rel="noreferrer noopener">BFG Repo-Cleaner</a> utility tool that goes through your git history and updates all references of the private value and replaces it with <code>**Removed**</code>.</p>
<p>Here&#8217;s how to use the BFG Repo-Cleaner to remove passwords from your git history. The BFG Repo-Cleaner is a Java file that can be saved to your local machine and run against your cloned git repo.</p>
<ol>
<li>Start by downloading the <a href="https://rtyley.github.io/bfg-repo-cleaner/" target="_blank" rel="noreferrer noopener">BFG Repo-Cleaner</a> .jar file to your local machine. I saved mine to a folder called bfg on my D: drive &#8211; D:\code\bfg.</li>
<li>Create a passwords.txt file that lists the passwords/keys you want to remove from your git repository and save to the same folder as the BFG Cleaner. The passwords.txt file is just a list of different passwords/keys that you want to remove from the repository.<br /><code>password1<br />password2<br />keyvalue123</code></li>
<li>Clone your repository using the &#8211;mirror flag and make a <strong>full backup</strong> of the repo just in case something breaks. <br /><code>$ git clone --mirror https://github.com/clintmcmahon/delete-passwords.git</code></li>
<li>Open command prompt and run the following command <br /><code>$ java -jar d:\code\bfg\bfg-1.13.0.jar --replace-text d:\code\bfg\passwords.txt d:\code\delete-passwords.git</code></li>
<li>Move into the cloned git folder and run the following command<br /><code>$ cd delete-passwords.git<br />$ git reflog expire --expire=now --all &amp;&amp; git gc --prune=now --aggressive</code></li>
<li>And finally push your changes back to the remote repository<br /><code>$ git push</code></li>
</ol>
<p>That&#8217;s it. If you go into your repository now you will see that all references to your values from the passwords.txt file will now be replaced with <code>**Removed**</code>. This is just the tip of the utilities ability, there are more examples and other documentation on at the <a href="https://rtyley.github.io/bfg-repo-cleaner/" target="_blank" rel="noreferrer noopener">BFG Repo-Cleaner project Github page</a>. </p>
<p><!--kg-card-end: html--></p>
<!--kg-card-end: html-->
