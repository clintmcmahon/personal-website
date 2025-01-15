---
title: "Creating a WordPress Redirect Login Plugin"
description: "I'm developming a WordPress plug in that will automatically redirect a user to a website when they log into the WordPress site or visit the home page after logging in."
date: "2025-01-15"
draft: false
slug: "creating-redirect-wordpress-plugin"
tags:
---

 <section>
    <p>
        I started working on a WordPress plug in that will redirect WordPress users to a custom url when they log into a WordPress site. Administrators of the site can set a custom redirect url on the user object along with an enabled flag. If the URL and enabled flag is set to true, the plug in will redirect the user to whatever the redirect URL is set to on log in. The plug in also checks and redirects the user when the visit the site again.
    </p> 
    <p>
        The need for this plug in was to use an existing <a href="/services/wordpress-hosting">WordPress installation</a> as the authentication front end for websites that don't have any authentication enabled and I'm not able to add authentication to them. For example, I have a group of websites that have urls with random characters like this hosted on a domain I do not have access to:
        <pre><code class="language-bash">
            example.com/521c3ad5-b834-4dca-bc44-54d9cda72720
        </pre></code>
        If you really wanted to, you could navigate directly to <code>example.com/521c3ad5-b834-4dca-bc44-54d9cda72720</code> and render the website. But if the site is not indexed by Google there is a smaller chance someone is going to guess the guid in the url. Obviously this is possible, but not a major concern for the client. This plug in allows the client to send a single url to their users instead of an individualized URL to each of them. Then the users can log into the WordPress front end and their users are automatically redirected to the website instance my client has assigned to them.
    </p>
    <p>
         This is my need for the plug in, but you can use this functionality to redirect users within an WordPress authenticated website without redirected them to an external site. It's built in a way that both of these options will work just fine. Say for example, you have a finance or HR site that you want to make sure your users do not navigate way from, this plug in would keep them from doing so.
    </p>
    <p>
        I also developed the plug in for bulk importing of users in case you have a lot of users and do not want to manually create each one. The plug in takes a simple .csv file with the following fields: <code>user_login, user_email, first_name, last_name, redirect_url, redirect_enabled</code> to create the users and generate their passwords. This allows you to bulk import users, but also to bulk set the redirect_url and redirect_enabled fields to speed up your workflow.
    </p>
    <p>
        The plug in is being tested and activily developed on my <a href="https://parkasoftware.com">consulting business website</a> while I prepare to submit the plug in to the WordPress directory. I will post again when I submit the plug in to the directory with screen shots and how the submission process goes.
    </p>

</section>
