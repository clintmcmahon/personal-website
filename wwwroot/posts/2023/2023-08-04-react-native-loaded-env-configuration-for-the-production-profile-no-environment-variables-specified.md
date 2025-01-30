---
title: "React Native: Loaded "env" configuration for the "production" profile: no environment variables specified."
description: "This is a post to describe a fix for an error in React Native, specifically an Expo Bare project, that looks like "Loaded "env" configuration for the "production" profile: no environment variables specified". "
date: "2023-08-04"
draft: false
slug: "/react-native-loaded-env-configuration-for-the-production-profile-no-environment-variables-specified"
tags:

---

<p>This is a post to describe a fix for an error in React Native, specifically an Expo Bare project, that looks like "Loaded "env" configuration for the "production" profile: no environment variables specified". </p><p>I ran into this bug trying to upgrade a client's React Native Android app. The app is built using Expo and I'm using <code>eas</code> to build and submit both iOS and Android apps to their app stores. After I made a code change, I started a build using <code>eas build</code> but after the build started I remembered I didn't was logged into Expo with the wrong user account. So I killed the build process, logged out of my Expo account, opened a new terminal and logged into the Expo CLI with the right credentials.</p><p>I ran a new <code>eas build</code> but the build failed with the following error:</p><pre><code class="language-bash">✔ Select platform › Android
Loaded "env" configuration for the "production" profile: no environment variables specified. Learn more: https://docs.expo.dev/build-reference/variables/
You don't have the required permissions to perform this operation.

Entity not authorized: AccountEntity[ID] (viewer = RegularUserViewerContext[ID], action = READ, ruleIndex = -1)
Request ID: ID
Error: GraphQL request failed.
</code></pre><p>I did some Googling and found <a href="https://github.com/expo/eas-cli/issues/1324">this helpful Github post</a> which made me realize my mistake. When I started the first build using my own Expo account instead of the client's Expo account, the build process wrote a JSON property called <code>extra.eas.projectId</code> to the <code>app.json</code> file of my project. Because I didn't have a project under my Expo account that matched what I was trying to build, Expo created a new project with a new id in my account using with the value written to <code>extra.eas.projectId</code>. As soon as I deleted the <code>extra</code> property from <code>app.json</code> I was able to build the project successfully again. </p><p>At first I didn't think the error made any sense, but the more I thought about it you can see that "Entity not authorized" actually makes sense here - the first part about the <code>env</code> configuration is a little bit of a red herring though. So what happened was Expo was trying to build my project under an Expo project that the current user didn't have access to. Which makes sense now. </p><p>So, in conclusion if you run into this error, delete this property from your <code>app.json</code> file and do a new <code>eas build</code> command. The new command will create a new <code>extra.eas.projectId</code> with the appropriate project id and you'll be golden.</p><pre><code class="language-json">"extra": {
"eas": {
"projectId": "12345678-1234-1234-be5b-f844590a0aad"
}
}</code></pre>
