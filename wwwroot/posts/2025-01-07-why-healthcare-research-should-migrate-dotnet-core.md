---
title: "Why Healthcare Research Organizations Should Migrate to .Net Core"
description: "Discover why healthcare research organizations should migrate from .Net Framework to .Net Core. Learn about the benefits, challenges, and successful strategies for seamless migration."
date: "2025-01-07"
draft: false
slug: "why-healthcare-research-should-migrate-dotNet-core"
tags:
---

 <section>
    <p>Healthcare research, and healthcare as an entire industry, depends on technology that needs to keep up with the demand that it's under. For the sake of this blog post, I'm going to focus on the <a href="/portfolio">healthcare research industry</a> because that is where my <a href="/services">experience as a developer is</a>. The demand I'm talking about is managing large datasets, integrating complex systems and being able to display data to end users faster than the day before. Older implementations of the .Net Framework need to be updated to .Net as they are increasingly unable to meet these requirements - not to mention added security risks since the old .Net Framework is no longer being supported by Microsoft. 
    </p>
    <p>
    Migrating to .Net Core isn't just about doing an upgrade for upgrade sake, but rather a much needed step toward a more resilient, scalable, and maintainable code base for future developers to work with. I'm writing this blog post for anyone who is on the fence about if they should leave their code as is or upgrade to .Net Core. Here is a list of the reason I think you should upgrade to .Net Core sooner than later.</p>
    
    <h2 class="mt-4">The Case for .Net Core</h2>
    <p>.Net Core represents a significant advancement in development frameworks. With the previous version of .Net you had to run only on Windows, but with .Net core you can run the software just about anywhere. Its cross-platform support, cloud ready, and has performance improvements that make the upgrade worth it in the end. For healthcare reasearch, we're working with huge amounts of data. Data that needs to be loaded, filtered, stored and retrieved again. The performance improvements alone make upgrading worth the effort.</p>
    <p>
        .Net Core is picking up steam in the development world, but not just by old Microsoft developers. There's a growing community of folks who are advocating for .Net and C# development that keeps <a href="https://news.ycombinator.com/item?id=32219836">getting bigger</a> and <a href="https://news.ycombinator.com/item?id=30654114">bigger</a>.
    </p>

    <h3 class="mt-3">Cross-Platform Flexibility</h3>
    <p>Before .Net Core came along you needed to have a Windows machine to do development and a Windows machine to deploy your product to. Not anymore, with .Net Core you can <a href="/blog/develop-net-core-apps-with-a-sql-server-database-on-a-mac-2">develop on a Macbook</a> and deploy to a Linux server for the same application that can run entirly on a Windows Server machine. In the healthcare research world the server environments are usually pretty uniform. If the organization is a Microsoft shop then there should be a Windows server environment you can deploy to. But, researchers can work across multiple systems—Windows, Linux, and macOS—sometimes simultaneously. Applications built on .Net Core can operate seamlessly across these platforms, reducing compatibility headaches and opening the door to broader collaboration.</p>

    <h3 class="mt-3">Codebases are Up To Date</h3>
    <p>.Net Core has been out since June 2016, that's close to a decade. Applications running the old .Net Framework are harder to maintain as the number of developers that can support it are around. Upgrading now gets you on track for easier future upgrades. I'm currently <a href="/services/legacy-systems">consulting on an a monolith of an application</a> that was developed in .Net Framework for <strong>$100,000</strong>. The underlying technology is out of date, however the application still runs fine. The issue is the dependencies that application relys on. It interacts with Quickbooks and Constant Contact through their SDK APIs, but those client versions are outdated and no longer supported. The longer we wait to upgrade the application the worse the eventual upgrade is going to be - and cost. The last thing my client wants to do is spend another 100k on software. And why should they? The software they bought should have been upgraded and maintained so they were not put into this difficult position.</p>

    <h3 class="mt-3">Enhanced Performance</h3>
    <p>.Net Core outperforms its predecessor in virtually every metric. Whether you're running real-time data analysis or rendering complex visualizations, the framework's optimized runtime reduces processing times and supports high-demand scenarios with ease. </p>
    <p>
    That's also a positive when running complex data visualizations that depend on large datasets. The faster the API returns data the faster users are going to see their data.
    </p>

    <h3 class="mt-3">Future-Proofing</h3>
    <p>Microsoft's investment in .Net Core ensures it will continue to evolve alongside technological advancements. Healthcare research organizations that adopt this framework position themselves to take advantage of ongoing maintenance and updates. Staying up to date helps your organization from running into that problem I talked about earlier. Since most research is grant based, it's important to save as much money as possible. By investing in a .Net Core application you are future proofing yourself down the road by staying up to date. Staying up to date in the .Net ecosystem will keep the organization protected from another costly upgrade down the line.</p>

    <h2 class="mt-4">Addressing Migration Concerns</h2>
    <p>Transitioning from .Net Framework to .Net Core involves more than rewriting code. It requires understanding the existing architecture, prioritizing essential components, and minimizing disruption to ongoing operations.</p>

    <h3 class="mt-3">Evaluation and Planning</h3>
    <p>A clear inventory of existing systems and their dependencies is essential. Prioritizing critical applications for migration ensures the process delivers immediate value and gives the team a clear objective.</p>

    <h3 class="mt-3">Incremental Implementation</h3>
    <p>Migrating entire systems in one sweep sounds like a quick and easy way to get this done, but it can be disruptive as well. Focus on modular updates—migrating components like APIs or individual services first -you reduce downtime and maintain continuity.</p>

    <h3 class="mt-3">Stakeholder Engagement</h3>
    <p>Since the healthcare research world is collaborative by nature. Engaging researchers, doctors, product owners, IT, and leadership to ensure the migration aligns with operational goals and user needs.</p>

    <h2 class="mt-4 ">Conclusion</h2>
    <p>Organizations can't afford to be tethered to outdated technology and the cost that comes with maintaining that tech down the road. .Net Core is flexible, powerful, and ready for the future. The move to modern frameworks isn't just about technology; it's about empowering the people who rely on that technology to do their best work.</p>

    <p>If you're considering a migration or need advice on how to get started, I'm available to discuss how we can create solutions that drive meaningful results for your organization. <a href="/contact">Contact me</a> to get started.</p>
</section>
