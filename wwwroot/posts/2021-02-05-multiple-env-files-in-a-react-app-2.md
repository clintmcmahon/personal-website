---
title: "Create multiple .env files in a React app"
description: ""
date: "2021-02-05"
draft: false
slug: "multiple-env-files-in-a-react-app-2"
tags:
---

<!--kg-card-begin: html--><p><!--kg-card-begin: html--></p>
<p>Configuring multiple <a href="https://medium.com/chingu/an-introduction-to-environment-variables-and-how-to-use-them-f602f66d15fa" target="_blank" rel="noopener">environment files</a> for a React app is handy if you have different variables for each environment (local, dev, test and production) &#8211; API endpoints is a good example. Like the name suggests, a .env file allows you to create environment specific variables that you specify to be used in specific builds for each of your environments. My projects usually have the four different environments:</p>
<ol>
<li>Local &#8211; My local machine where I do all my development.</li>
<li>Development &#8211; This is the server environment where we can try new code outside our development machines. This is a server environment that is configured similar to test and production but is reserved to break things. Generally this environment is filled with dummy data and can become a mess pretty easily as we test out new features and debug code.</li>
<li>Test or staging &#8211; A copy of the production environment where end users, testers and other folks from the team do their testing. This environment usually has the same data (minus any PII/PHI data) that production has.</li>
<li>Production &#8211; This is the live site. You know what this is.</li>
</ol>
<p>With these four environments I have five environment files in my React app structure. Each with their own variables that relate to the specific environment.</p>
<ul>
<li>.env
<ul>
<li>This is a placeholder file that only shows the structure the other environment files should follow. For example:
<ul>
<li>REACT_APP_API_HOST=API_HOST<br />REACT_APP_WEB_HOST=WEB_HOST<br />REACT_APP_BUILD=BUILD</li>
</ul>
</li>
</ul>
</li>
<li>.env.development.local
<ul>
<li>This is the file that I use for my local development. I have this file added to .gitignore so it&#8217;s not checked into source control. Every developer should create this file locally and configure it with their local information.</li>
</ul>
</li>
<li>.env.development
<ul>
<li>Development server</li>
</ul>
</li>
<li>.env.staging
<ul>
<li>Staging server</li>
</ul>
</li>
<li>.env.production
<ul>
<li>Production server</li>
</ul>
</li>
</ul>
<p>Then by utilizing the <a href="https://www.npmjs.com/package/env-cmd" target="_blank" rel="noopener">env-cmd</a> package I&#8217;m able to run a command like <code class="EnlighterJSRAW" data-enlighter-language="shell">npm run build:production</code> to create my production build which uses the variables defined in the .env.production file. Same goes for local, development and testing/staging.</p>
<h3>How to configure multiple .env files</h3>
<p>Here&#8217;s how to set up a React application to utilize multiple .env files and variables. This example uses <a href="https://reactjs.org/docs/create-a-new-react-app.html" target="_blank" rel="noopener">Create-React-App</a>, so YMMV depending on what you bootstrapped your React app with.</p>
<p>1. Start off by creating a .env file at the root of your project for each of your environments.</p>
<p><img decoding="async" loading="lazy" class="alignnone size-full wp-image-937" src="http://clintmcmahon.com/content/images/wordpress/2021/02/Capture.png" alt="Multiple .env files in React" width="400" height="248" sizes="(max-width: 400px) 100vw, 400px" /></p>
<p>2. Install the <a href="https://www.npmjs.com/package/env-cmd" target="_blank" rel="noopener">env-cmd</a> package into your project <code class="EnlighterJSRAW" data-enlighter-language="ini">npm install env-cmd</code></p>
<p>3. Open your package.json file and inside the scripts node add a line for each environment you will be building. You should already have lines for start, build, test and eject. Each line is specific to the build and .env file. So, <strong>build:development</strong> is the command you&#8217;ll run to create your development build, <strong>build: staging</strong> for testing and <strong>build:production</strong> for production.</p>
<pre class="EnlighterJSRAW" data-enlighter-language="json" data-enlighter-theme="enlighter" data-enlighter-highlight="6-8">"scripts": {    "start": "react-scripts start",    "build": "react-scripts build",    "test": "react-scripts test",    "eject": "react-scripts eject",    "build:development": "env-cmd -f .env.development react-scripts build",    "build:staging": "env-cmd -f .env.staging react-scripts build",    "build:production": "env-cmd -f .env.production react-scripts build"  }</pre>
<p>4. Add the environment specific variables to your environment files.</p>
<pre class="EnlighterJSRAW" data-enlighter-language="json">REACT_APP_API_HOST=https://localhost:5001REACT_APP_WEB_HOST=http:localhost:3000REACT_APP_BUILD=Development.Local</pre>
<p>5. Run your build to create a build with your environment variables <code class="EnlighterJSRAW" data-enlighter-language="shell">npm run build:production</code></p>
<p>Now you should have the ability to build your React apps with environment specific variables. Hopefully this makes your deployments easier and more straightforward.</p>
<p><!--kg-card-end: html--></p>
<!--kg-card-end: html-->
