---
title: "Migrate from R Shiny to a modern web framework"
description: ""
date: "2022-12-20"
draft: false
slug: "migrate-from-r-shiny-to-a-modern-web-framework"
tags:
---

<!--kg-card-begin: html-->
<h2>Overview</h2>

<p>A request I get from clients that do heavy processing with R is they want to migrate from R Shiny to a modern web framework like React, Angular or Vue. Their setup usually consists of websites running in a R Shiny package that connects to a database and returns a result set on the Shiny framework back to the client. Your basic CRUD type application, essentially. Usually displaying the result set using a charting library like D3.js or HighCharts to display the results sets in nice graphs. Now that I&#8217;ve built multiple systems with these types of clients I have a good understanding of the problem, type of architectures involved and how to improve this system to take advantage of the flexibly gained from some of these modern web frameworks.</p>

<p><a href="https://shiny.rstudio.com/" data-type="URL" data-id="https://shiny.rstudio.com/" target="_blank" rel="noreferrer noopener">R Shiny</a> is a great package that allows researchers, statisticians and developers to build interactive web apps using the R programming language. In my experience it&#8217;s really good in situations where the person creating the app is a statistician or person who&#8217;s background is processing data with R but may not have a background in web development. R Shiny allows those users to build pleasant user interfaces within the R programing model without the need to learn a bunch of new web technologies or have full stake developer skills.</p>

<p>What it lacks is the robustness and freedom that comes with modern web frameworks with tools like React, Angular, jQuery and regular vanilla JavaScript apps. When you&#8217;re building a website in R Shiny you have to follow the patterns and practices of R and Shiny &#8211; I suppose you could argue the same is true for any application you&#8217;re building no matter what the tech stack might be. To some degree you&#8217;re having to play within the boundary of your chosen technology stack. However, it&#8217;s my opinion that you get more freedom when breaking away from R Shiny and building your websites in something like React for instance. I choose React because that is my front end framework of choice.</p>

<p>Here are a couple of different ways I&#8217;ve been successful migrating from pure R Shiny websites to a modern web frameworks like React. But in order to pull off any of the following options, you&#8217;ll need to have full stack experience working with front end layers, API layers and the database layer. This can be a single team member or an entire team. </p>

<h2>Distributed Option</h2>

<p>I like to break R Shiny apps into distributed parts consisting of a web front end and side car process. The side car process is the original R code that produces either .CSV files or dumps the results of the R code right into the database. The web framework delivers the data to the end user that was creating by the R code process.</p>

<p>Starting with a process where the R programmers continue to develop their programs using R but instead returning the data directly to the R Shiny app, I set up a SQL database to house the results of the R code. Like I mentioned, this process can either be a .CSV file that is then imported into a database or the R code can populate the database right from the program. Then a React front end will connect to a .Net WebAPI built in C# which retrieves data from SQL Server or other popular database system like MySQL or PostgreSQL. </p>

<figure class="wp-block-image aligncenter size-full"><img decoding="async" loading="lazy" src="/images/wordpress/2022/12/Screenshot-2022-12-16-at-4.53.10-PM.png" alt="Migrate from R Shiny to a modern web framework" class="wp-image-17218" sizes="(max-width: 675px) 100vw, 675px" / sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<p>This is a nice way to break things apart to give companies greater flexibility in their systems. By allowing React or Angular or Vue the organization has unlimited possibility in what they can do with their data. Integrations with an API, HighCharts, Mapbox or custom dashboards are simplified with this route. This also allows employees to focus on their specialty, be it writing R code or custom websites. In environments where a statistician is really good at producing rows of processed data, a web developer can easily grab the results and make them available on the web. Neither the statistician or the web developer need to spend time getting up to speed on a new technology.</p>

<h2>SQL Server Option</h2>

<p>Starting in <a href="https://learn.microsoft.com/en-us/sql/machine-learning/sql-server-machine-learning-services?view=sql-server-ver16" data-type="URL" data-id="https://learn.microsoft.com/en-us/sql/machine-learning/sql-server-machine-learning-services?view=sql-server-ver16" target="_blank" rel="noreferrer noopener">SQL Server 2016, you can write R code in SQL Server</a> using SQL Server Machine Learning Services. In short, it SQL MLS  allows you to run R or Python code on your SQL Server and interact with your relational data right at the database level. </p>

<blockquote class="wp-block-quote">
<p>Machine Learning Services is a feature in SQL Server that gives the ability to run Python and R scripts with relational data. You can use open-source packages and frameworks, and the <a href="https://learn.microsoft.com/en-us/sql/machine-learning/sql-server-machine-learning-services?view=sql-server-ver16#packages">Microsoft Python and R packages</a>, for predictive analytics and machine learning. The scripts are executed in-database without moving data outside SQL Server or over the network. This article explains the basics of SQL Server Machine Learning Services and how to get started.</p>
<cite>https://learn.microsoft.com/en-us/sql/machine-learning/sql-server-machine-learning-services?view=sql-server-ver16</cite></blockquote>

<p>Setting up SQL Server MLS is not complicated but can be somewhat complex. There are a few moving pieces that need to be put into place before you can start running with this option. You&#8217;ll need administrator server rights to the SQL Server(s) in order to <a href="https://learn.microsoft.com/en-us/sql/machine-learning/install/sql-machine-learning-services-windows-install-sql-2022?view=sql-server-ver16" data-type="URL" data-id="https://learn.microsoft.com/en-us/sql/machine-learning/install/sql-machine-learning-services-windows-install-sql-2022?view=sql-server-ver16" target="_blank" rel="noreferrer noopener">install the MLS package</a> to start. With the SQL Server option you can keep all the R scripts that were originally running inside R Shiny and migrate them to SQL Server. </p>

<p>Once SQL Machine Learning Services is set up on the SQL Server it&#8217;s just a matter of setting up the R script and query combination. You <a href="https://learn.microsoft.com/en-us/sql/machine-learning/tutorials/quickstart-r-create-script?view=sql-server-ver16" target="_blank" data-type="URL" data-id="https://learn.microsoft.com/en-us/sql/machine-learning/tutorials/quickstart-r-create-script?view=sql-server-ver16" rel="noreferrer noopener">execute the R code via a SQL Server stored procedure</a> with a relational data set input parameter and the R code to run. You don&#8217;t need to pass in the data set input parameter if your R code is not going to be processing any data. However, you get the most out of this integration if you processing data with R &#8211; otherwise using the SQL Server option can be a lot of overhead that&#8217;s not needed.</p>

<figure class="wp-block-image size-full"><img decoding="async" loading="lazy" src="/images/wordpress/2022/12/Screenshot-2022-12-13-at-8.55.23-AM.png" alt="React + C# Web API + SQL Server flow chart diagram" class="wp-image-16859" sizes="(max-width: 762px) 100vw, 762px" / sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<p>This architecture set up follows the standard web client &#8211;&gt; API &#8211;&gt; database model with the simple addition of Machine Learning Services. Machine Learning Services are a combination of separate executables that run on the SQL Server. The first is either R or Python and the other is a service which handles communication between SQL Server and the R/Python process running on the server. This is a flowchart diagram of how the process works from the moment the user kicks off the process the web client sends a request with some query parameters. Perhaps the type of process to run along with other variables. The API translates that request into a SQL Server request to run, the SQL Server takes that request to execute a MLS process which process the R script against any data set that is passed from SQL Server to the MLS process. Once the R processing is complete, MLS returns a data set back to SQL Server which can be further processed by additional queries or returned to the calling API. The API then returns a payload to the web browser to display to the user. </p>

<h2>Conclusion</h2>

<p>These are the two methods that I&#8217;ve used to help clients move from R Shiny to a React or other SPA web framework. In the end this architecture gives companies more flexibility for developing interactive websites or reports that rely on R to do hard core data analysis. By breaking the architecture into these other technologies you&#8217;re able to spread the work out throughout the team and allow the developers to do what they do best. The R programmers can focus on R and the web developers can focus on the web side of the application. </p>

<p>If you need help to migrate your apps from R Shiny to React, Angular or Vue I&#8217;d be more than happy to help. Just reach out via the form on <a href="https://clintmcmahon.com/contact/" data-type="page" data-id="450">my contact page</a>.</p>
<!--kg-card-end: html-->
