---
title: "Primary Constructors in C# 12"
description: "I’ve come across Primary Constructors, a powerful new feature that simplifies how we define and initialize classes. This feature enables us to declare constructor parameters directly within the class definition, streamlining code and reducing boilerplate"
date: "2025-01-10"
draft: false
slug: "primary-constructors-in-csharp"
tags:
---

 <section>
    <p>I just discovered Primary Constructors in C#. A new feature that simplifies how to define and initialize variables in classes. Instead of creating an entire constructor method and initializing variables inside of it, this feature enables us to declare constructor parameters directly within the class definition and replacing the need to write that boilerplate constructor method.
</p>
<p>
    In the <a href="/blog/how-to-execute-multiple-sql-server-views-from-entity-framework">old way of C# class design</a>, constructors and their associated properties often required you to write repetitive code to populate variables or wire up the class properties. But now Primary Constructors, you can define constructor parameters directly in the class declaration, saving you the extra effort of manually declaring properties and writing constructors. Bazinga!
</p>
<p>
    This makes your classes cleaner and easier to read, especially for data models or classes with straightforward initialization logic.
</p>
    <p>
        Old way:<br/>
        <pre class="language-csharp">
        <code class="language-csharp">
            public class WeatherRecord
            {
                public string City { get; }
                public DateTime Date { get; }
                public int Temperature { get; }

                public WeatherRecord(string city, DateTime date, int temperature)
                {
                    City = city;
                    Date = date;
                    Temperature = temperature;
                }
            }
        </code>
        </pre>
        <br />
        The new way with primary constructors gets you the same results but with much less code:<br />
        <pre class="language-csharp">
            <code class="language-csharp">
                public class WeatherRecord(string City, DateTime Date, int Temperature);
            </code>
        </pre>
        <br />
        That’s it! The constructor and the properties are all handled implicitly. This new syntax is perfect for classes where the primary purpose is to store data.
    </p>

</section>
