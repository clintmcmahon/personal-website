---
title: "Using JavaScript inline || and && logical operators together in React"
description: ""
date: "2022-07-14"
draft: false
slug: "using-javascript-inline-and-logical-operators-together-in-react"
tags:
---

<!--kg-card-begin: html-->
<p>In my React components I usually end up doing an inline comparison with conditional operators to determine what output to render. In these cases I can use <a href="https://reactjs.org/docs/conditional-rendering.html#inline-if-with-logical--operator" data-type="URL" data-id="https://reactjs.org/docs/conditional-rendering.html#inline-if-with-logical--operator" target="_blank" rel="noreferrer noopener">inline if with logical &amp;&amp; operator</a> because React allows <a href="https://reactjs.org/docs/introducing-jsx.html#embedding-expressions-in-jsx" data-type="URL" data-id="https://reactjs.org/docs/introducing-jsx.html#embedding-expressions-in-jsx" target="_blank" rel="noreferrer noopener">embedded expressions in JSX</a>. Embedded expressions are any valid JavaScript code wrapped in curly braces and any valid piece of JavaScript that is wrapped in curly braces will execute as expected. </p>

<p>Here is simple example of using an inline statement in React. All this code does is render the Loading&#8230; text if the state variable <strong>isLoading</strong> is set to true.</p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="javascript" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">const [isLoading, setIsLoading] = useState(true);
...
return (
    {isLoading &amp;&amp; 
        &lt;div>Loading...&lt;/div>
    }
)</pre></div>

<p>This works because in JavaScript a true condition will always render the expression follows the conditional operator (&amp;&amp;). In this case <strong>isLoading</strong> returns true so anything after <strong>&amp;&amp;</strong> is rendered. If <strong>isLoading</strong> returned false the rest of the expression would be ignored and therefore the <strong>Loading&#8230;</strong> statement is skipped. </p>

<p>You can also combine the &amp;&amp; operator with || together in the same statement  an AND with an OR conditional statement inline. All that&#8217;s need is to wrap the conditions in parenthesis followed by the &amp;&amp; operator. The same logic applies here as did with the above statement. If the condition within the parenthesis is true then the following expression is rendered. If the condition is false, the following expression is then skipped.  </p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="javascript" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">const [isLoading, setIsLoading] = useState(true);
const [isStillLoading, setIsStillLoading] = useState(true)

useEffect(()=>{
setIsLoading(false);
setIsStillLoading(true);
})

return (
{(isLoading || setIsStillLoading) &amp;&amp;
&lt;div>Loading...&lt;/div>
}
)</pre></div>

<p>This is all pretty basic after you understand how embedded expressions work in React and JavaScript. It took diving into JavaScript conditional logic to understand why the double ampersand (&amp;&amp;) was needed after the conditional logic. But, kind of like all language syntax, the more you try it out and learn the ideas the easier it becomes to work with. </p>
<!--kg-card-end: html-->
