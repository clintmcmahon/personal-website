---
title: "Convert A Generic List To CSV File In C#"
description: ""
date: "2020-02-07"
draft: false
slug: "how-to-open-a-csv-file-in-the-browser"
tags:
---

<!--kg-card-begin: html-->
<p>This is an example of how to convert a generic list to a dynamic downloadable .csv file using C#, the <a href="https://joshclose.github.io/CsvHelper/" target="_blank" rel="noreferrer noopener">CsvHelper library</a> and the FileStreamResult.</p>
<p><a href="https://github.com/clintmcmahon/mvc-list-to-csv">Download the repo and run the full project solution from Github</a></p>

<p>In this example a request comes into the controller, we create a generic list of type ListItem, created a memory stream and using the CsvWriter library  return a dynamic .csv file to the browser.</p>

<p><strong>Step 1: Create the generic list class</strong><br />This class will hold our generic list items. This can be whatever data you need.</p>

<div class="wp-block-simple-code-block-ace" style="height: 250px; position: relative; margin-bottom: 50px;">
<pre class="wp-block-simple-code-block-ace" style="position: absolute; top: 0; right: 0; bottom: 0; left: 0;" data-mode="csharp" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">public class ListItem
{
    public int Id { get; set; }
    public string Name { get; set; }
}</pre>
</div>

<pre class="wp-block-code"><code></code></pre>

<p><strong>Step 2:</strong> <strong>Create the MVC action to return the CSV file</strong><br />This action method creates the generic list, writes it to memory with help from the CsvHelper library and returns the FileStreamResult back to the browser.</p>

<div class="wp-block-simple-code-block-ace" style="height: 250px; position: relative; margin-bottom: 50px;">
<pre class="wp-block-simple-code-block-ace" style="position: absolute; top: 0; right: 0; bottom: 0; left: 0;" data-mode="csharp" data-theme="monokai" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">//Action Method for your controller
[HttpPost]
public ActionResult ConvertToCSV()
{
    //Create the test list
    var list = new List&lt;ListItem&gt;()
    {
        new ListItem(){Id = 1, Name = "Jerry"},
        new ListItem(){Id = 2, Name="George"},
        new ListItem(){Id = 3, Name="Kramer"},
        new ListItem(){Id = 4, Name = "Elaine"}
     };

    byte[] result;
    using (var memoryStream = new MemoryStream())
    {
        using (var streamWriter = new StreamWriter(memoryStream))
        {
            using (var csvWriter = new CsvWriter(streamWriter))
            {
                csvWriter.WriteRecords(list);
                streamWriter.Flush();
                result = memoryStream.ToArray();
            }
         }
     }

     return new FileStreamResult(new MemoryStream(result), "text/csv") { FileDownloadName = "filename.csv" };

}

</pre>
</div>

<pre class="wp-block-code"><code></code></pre>

<p><a href="https://stackoverflow.com/a/21095080/118144" target="_blank" rel="noreferrer noopener"><em>h/t to this StackOverflow post</em></a></p>

<p><a href="https://github.com/clintmcmahon/mvc-list-to-csv">Download the repo and run the full project solution from Github</a></p>

<p>&nbsp;</p>
<!--kg-card-end: html-->
