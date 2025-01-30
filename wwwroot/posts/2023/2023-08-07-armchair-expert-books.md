---
title: "Armchair Expert Books"
description: "Or how to get the cover for a book programmatically. In the Armchair Expert Media Explorer is a website I built to track all the book, movies, documentaries, podcasts and shows that are discussed on Armchair Expert between the hosts, Dax and Monica, and their guests. "
date: "2023-08-07"
draft: false
slug: "armchair-expert-books"
tags:
---

<p>Or how to get the cover image of a book programmatically. The <a href="https://armchairmediaexplorer.com">Armchair Expert Media Explorer</a> is a website I built to track all the book, movies, documentaries, podcasts and shows that are discussed on the Armchair Expert podcast between the hosts, Dax and Monica, and their guests. For each book that is entered into the database, I make a call to an API to get the book cover image. This blog post will show how to get the cover image of a book programmatically using C# and a third party API.</p><h2 id="call-the-getbook-method">Call the GetBook method</h2><p>I'm using the <a href="https://openlibrary.org/developers/api">OpenLibrary API</a> to retrieve book covers for newly added books. The process to return a book cover image is two steps. Firstk call the search endpoint to search for a book by its title and author. The object returned will contain a variable called <code>CoverI</code> which is identifies the cover image for the book. </p><p>The second step in the process is to call the covers.openlibrary.org endpoint by passing the <code>CoverI</code> property returned from the first step in the url. You're able to tell the API what size cover image you want (ie S, M, L, XL) by appending the size to the end of the CoverI property - like <code>{book.CoverI}-M</code>. In my case I'm asking for size medium. </p><p>Here you can see the starting point to get the book object. I first make a call to `GetBook` by passing in the title and the author as string variables.   </p><pre><code class="language-csharp">var book = await GetBook(title, author);
    if (book != null &amp;&amp; book?.CoverI != 0)
    {
        var thumbnailUrl = $"https://covers.openlibrary.org/b/id/{book.CoverI}-M.jpg";
    }</code></pre><h2 id="getbook-method">GetBook method</h2><p>The <code>GetBook</code> method takes title and author to make a <code>GET</code> request to the search endpoint of the OpenLibrary API. I'm casting the response as <code>OpenLibraryBookResponse</code> which allows me to look at the returned list of documents. Note that <code>Docs</code> is a <code>List&lt;T&gt;</code> because the search returns books that match the search parameters. It's highly likely you will return multiple results from your searches.</p><p>In this case, if the title and author search comes up empty, I make another call to just search by the title. This pattern works for 99.99% of all books I'm searching for.</p><pre><code class="language-csharp"> public async Task&lt;OpenLibraryBook&gt; GetBook(string title, string author)
    {
        using var httpClient = new HttpClient();
        var url = $"https://openlibrary.org/search.json?author={HttpUtility.UrlEncode(author)}&amp;title={HttpUtility.UrlEncode(title)}";
        var response = await httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var bookResponse = JsonConvert.DeserializeObject&lt;OpenLibraryBookResponse&gt;(content);

        if (bookResponse.Docs == null || bookResponse.Docs.Count() == 0)
        {
            Console.WriteLine(url);
            url = $"https://openlibrary.org/search.json?q={HttpUtility.UrlEncode(title)}&amp;mode=everything";
            response = await httpClient.GetAsync(url);
            content = await response.Content.ReadAsStringAsync();
            bookResponse = JsonConvert.DeserializeObject&lt;OpenLibraryBookResponse&gt;(content);
            if (bookResponse.Docs == null || bookResponse.Docs.Count() == 0)
            {
                Console.WriteLine(url);
            }
        }
        return bookResponse.Docs.FirstOrDefault();
    }

</code></pre><h2 id="openlibrarybook-class">OpenLibraryBook class</h2><p>This is the class representation of the OpenLibrary API response and book search results. Using <code>JsonConvert.DesrializeObject</code> will convert the JSON response into an object based on the classes below. Once the results has been deserialized into an object you can freely explore all of the properties.</p><pre><code class="language-csharp">using Newtonsoft.Json;

public class OpenLibraryBookResponse
{
public int NumFound { get; set; }
public int Start { get; set; }
public bool NumFoundExact { get; set; }
public OpenLibraryBook[] Docs { get; set; }
}

public class OpenLibraryBook
{
[JsonProperty("key")]
public string Key { get; set; }

    [JsonProperty("seed")]
    public string[] Seed { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("title_suggest")]
    public string TitleSuggest { get; set; }

    [JsonProperty("title_sort")]
    public string TitleSort { get; set; }

    [JsonProperty("edition_count")]
    public int EditionCount { get; set; }

    [JsonProperty("edition_key")]
    public string[] EditionKey { get; set; }

    [JsonProperty("publish_date")]
    public string[] PublishDate { get; set; }

    [JsonProperty("publish_year")]
    public int[] PublishYear { get; set; }

    [JsonProperty("first_publish_year")]
    public int FirstPublishYear { get; set; }

    [JsonProperty("number_of_pages_median")]
    public int NumberOfPagesMedian { get; set; }

    [JsonProperty("lcc")]
    public string[] Lcc { get; set; }

    [JsonProperty("isbn")]
    public string[] Isbn { get; set; }

    [JsonProperty("last_modified_i")]
    public long LastModified { get; set; }

    [JsonProperty("ebook_count_i")]
    public int EbookCount { get; set; }

    [JsonProperty("ebook_access")]
    public string EbookAccess { get; set; }

    [JsonProperty("has_fulltext")]
    public bool HasFulltext { get; set; }

    [JsonProperty("public_scan_b")]
    public bool PublicScan { get; set; }

    [JsonProperty("ratings_count_1")]
    public int RatingsCount1 { get; set; }

    [JsonProperty("ratings_count_2")]
    public int RatingsCount2 { get; set; }

    [JsonProperty("ratings_count_3")]
    public int RatingsCount3 { get; set; }

    [JsonProperty("ratings_count_4")]
    public int RatingsCount4 { get; set; }

    [JsonProperty("ratings_count_5")]
    public int RatingsCount5 { get; set; }

    [JsonProperty("ratings_average")]
    public double RatingsAverage { get; set; }

    [JsonProperty("ratings_sortable")]
    public double RatingsSortable { get; set; }

    [JsonProperty("ratings_count")]
    public int RatingsCount { get; set; }

    [JsonProperty("readinglog_count")]
    public int ReadinglogCount { get; set; }

    [JsonProperty("want_to_read_count")]
    public int WantToReadCount { get; set; }

    [JsonProperty("currently_reading_count")]
    public int CurrentlyReadingCount { get; set; }

    [JsonProperty("already_read_count")]
    public int AlreadyReadCount { get; set; }

    [JsonProperty("cover_edition_key")]
    public string CoverEditionKey { get; set; }

    [JsonProperty("cover_i")]
    public int CoverI { get; set; }

    [JsonProperty("publisher")]
    public string[] Publisher { get; set; }

    [JsonProperty("language")]
    public string[] Language { get; set; }

    [JsonProperty("author_key")]
    public string[] AuthorKey { get; set; }

    [JsonProperty("author_name")]
    public string[] AuthorName { get; set; }

    [JsonProperty("author_alternative_name")]
    public string[] AuthorAlternativeName { get; set; }

    [JsonProperty("subject")]
    public string[] Subject { get; set; }
    [JsonProperty("publisher_facet")]

    public string[] PublisherFacet { get; set; }
    [JsonProperty("subject_facet")]

    public string[] SubjectFacet { get; set; }
    [JsonProperty("_version_")]

    public long Version { get; set; }
    [JsonProperty("LccSort")]

    public string LccSort { get; set; }
    [JsonProperty("author_facet")]

    public string[] AuthorFacet { get; set; }
    [JsonProperty("subject_key")]

    public string[] SubjectKey { get; set; }

}</code></pre><p>And that is it. If you use the <code>GetBook</code> method along with the supporting classes you should be able to programmatically get cover images of books from the OpenLibrary API.</p>
