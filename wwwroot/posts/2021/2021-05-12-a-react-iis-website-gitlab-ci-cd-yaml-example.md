---
title: "A React + IIS Website GitLab CI/CD YAML Example"
description: ""
date: "2021-05-12"
draft: false
slug: "a-react-iis-website-gitlab-ci-cd-yaml-example"
tags:
---

<!--kg-card-begin: html--><p><a href="https://clintmcmahon.com/my-top-5-reasons-to-start-using-ci-cd/">GitLab CI/CD is a pretty big part of my deployment strategy</a> with the React websites that I build. My current engagement has me pushing to a Windows IIS server via GitLab&#8217;s continuous integration/deployment, so I wanted to post an example of the YAML file that makes it all happen. Without going into the in&#8217;s and outs of how <a href="https://docs.gitlab.com/ee/ci/" target="_blank" rel="noopener">GitLab CI/CD works</a>, here&#8217;s an example of my GitLab YAML template that I use for React websites on IIS servers.</p>
<p>There are a couple prerequisites that need to be in place if you&#8217;re going to use this file:</p>
<ol>
<li>Starting off this build server is a windows machine, any other set up requires a few updates.</li>
<li>The build server needs to have <a href="https://www.npmjs.com/" target="_blank" rel="noopener">NPM</a>, <a href="https://nodejs.org/en/" target="_blank" rel="noopener">NodeJS</a> and <a href="https://www.microsoft.com/en-us/download/details.aspx?id=30436" target="_blank" rel="noopener">MS Web Deploy V3</a> installed. Install MS Web Deploy on the build and web server.</li>
<li>$CI_USERNAME and $CI_PASSWORD are GitLab secrets. CI_USERNAME should have access to the IIS server.</li>
<li>The IIS website is set up for Basic Authentication permissions.</li>
</ol>
<p>That&#8217;s it. Let me know if you run into any issues or have questions.</p>
<pre class="EnlighterJSRAW" data-enlighter-language="yaml">stages:
 - build
 - deploy

before_script:

- echo BEGINNING

#------------------------------------------------------------------------------------
#------------------------------------------------------------------------------------

#----------------------------

# Build Website Pieces

#----------------------------
build_web_production:
stage: build
script: - $Env:Path += ";C:\Program Files\nodejs"
    - npm install 
    - npm run build
  only:
    - main
  artifacts:
   paths: 
    - "$CI_PROJECT_DIR/build"
environment:
name: production
url: http://example.com
tags: - shell
#----------------------------

# End Build Website Pieces

#----------------------------

#------------------------------------------------------------------------------------
#------------------------------------------------------------------------------------

#----------------------------

# Deploy Website Pieces

#----------------------------
deploy_web_production:
stage: deploy
only:

- main
  script:
- $Env:Path += ";C:\Program Files (x86)\IIS\Microsoft Web Deploy V3\"
- msdeploy -verb:sync -source:contentPath="$CI_PROJECT_DIR/build" -dest:contentPath="IIS_WEBSITE_NAME",ComputerName="https://IISSERVER:8172/msdeploy.axd?site=IIS_WEBSITE_NAME",UserName=$CI_USERNAME,Password=$CI_PASSWORD,IncludeAcls='False',AuthType='Basic' -skip:objectName=filePath,absolutePath=.\*web\.config -enableRule:AppOffline -disableLink:AppPoolExtension -disableLink:ContentExtension -disableLink:CertificateExtension -allowUntrusted -userAgent="VSCmdLine:WTE1.0.0.0"
  dependencies:
- build_web_production
  environment:
  name: test
  url: https://example.com
  artifacts:
  paths:
- "$CI_PROJECT_DIR/build"
  tags:
- shell
</pre>
<p>&nbsp;</p>
<!--kg-card-end: html-->
