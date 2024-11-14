---
title: "Filter DataTable By Column Value With Custom Dropdown Menu"
description: ""
date: "2021-02-17"
draft: false
slug: "add-a-custom-search-filter-to-datatables-header"
tags:
---

<!--kg-card-begin: html--><p>In addition default search box in DataTables sometimes it&#8217;s nice to have the ability to filter by a specific DataTable column. This example shows how to use a custom drop-down menu to filter a <a href="https://datatables.net/" target="_blank" rel="noopener">DataTable</a> by column value. I&#8217;m going to create a drop-down menu that displays the unique list of strings from a column called Category and when the user selects a category from the dropdown-menu, the datatable will be rendered with only records with the Category from the selected value.</p>
<p><img decoding="async" loading="lazy" class="alignnone size-large wp-image-946" src="/images/wordpress/2021/02/filterdatatable.png" alt="Filter DataTable Example" sizes="(max-width: 1024px) 100vw, 1024px" / sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></p>
<p><a href="https://clintmcmahon.github.io/add-filter-datatable/" target="_blank" rel="noopener">View full working example.</a><br />
<a href="https://github.com/clintmcmahon/add-filter-datatable" target="_blank" rel="noopener">Download full repo.</a></p>
<h2>HTML</h2>
<p>There are two main parts to the HTML, the category filter drop-down menu and the datatable. The values in the category filter will be the values that are to be filtered from the table when the user selects an item.</p>
<pre class="EnlighterJSRAW" data-enlighter-language="html">    &lt;!-- Create the dropdown filter --&gt;

&lt;div class="category-filter"&gt;
&lt;select id="categoryFilter" class="form-control"&gt;
&lt;option value=""&gt;Show All&lt;/option&gt;
&lt;option value="Classical"&gt;Classical&lt;/option&gt;
&lt;option value="Hip Hop"&gt;Hip Hop&lt;/option&gt;
&lt;option value="Jazz"&gt;Jazz&lt;/option&gt;
&lt;/select&gt;
&lt;/div&gt;

    &lt;!-- Set up the datatable --&gt;
    &lt;table class="table" id="filterTable"&gt;
      &lt;thead&gt;
        &lt;tr&gt;
          &lt;th scope="col"&gt;Artist&lt;/th&gt;
          &lt;th scope="col"&gt;Category&lt;/th&gt;
        &lt;/tr&gt;
      &lt;/thead&gt;
      &lt;tbody&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Public Enemy&lt;/td&gt;
          &lt;td scope="col"&gt;Hip Hop&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Chet Baker&lt;/td&gt;
          &lt;td scope="col"&gt;Jazz&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Billie Holiday&lt;/td&gt;
          &lt;td scope="col"&gt;Jazz&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Vivaldi&lt;/td&gt;
          &lt;td scope="col"&gt;Classical&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Jurrasic 5&lt;/td&gt;
          &lt;td scope="col"&gt;Hip Hop&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Onyx&lt;/td&gt;
          &lt;td scope="col"&gt;Hip Hop&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Tchaikovsky&lt;/td&gt;
          &lt;td scope="col"&gt;Classical&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Oscar Peterson&lt;/td&gt;
          &lt;td scope="col"&gt;Jazz&lt;/td&gt;
        &lt;/tr&gt;
      &lt;/tbody&gt;
    &lt;/table&gt;</pre>

<h2>JavaScript</h2>
<p>The JavaScript part relies on jQuery but can be modified to use vanilla javascript if you don&#8217;t have jQuery as part of the project. The code is commented below to give you an idea of what&#8217;s happening.</p>
<pre class="EnlighterJSRAW" data-enlighter-language="js">&lt;script&gt;
    $("document").ready(function () {

      $("#filterTable").dataTable({
        "searching": true
      });

      //Get a reference to the new datatable
      var table = $('#filterTable').DataTable();

      //Take the category filter drop down and append it to the datatables_filter div.
      //You can use this same idea to move the filter anywhere withing the datatable that you want.
      $("#filterTable_filter.dataTables_filter").append($("#categoryFilter"));

      //Get the column index for the Category column to be used in the method below ($.fn.dataTable.ext.search.push)
      //This tells datatables what column to filter on when a user selects a value from the dropdown.
      //It's important that the text used here (Category) is the same for used in the header of the column to filter
      var categoryIndex = 0;
      $("#filterTable th").each(function (i) {
        if ($($(this)).html() == "Category") {
          categoryIndex = i; return false;
        }
      });

      //Use the built in datatables API to filter the existing rows by the Category column
      $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
          var selectedItem = $('#categoryFilter').val()
          var category = data[categoryIndex];
          if (selectedItem === "" || category.includes(selectedItem)) {
            return true;
          }
          return false;
        }
      );

      //Set the change event for the Category Filter dropdown to redraw the datatable each time
      //a user selects a new filter.
      $("#categoryFilter").change(function (e) {
        table.draw();
      });

      table.draw();
    });

&lt;/script&gt;</pre>

<h2>Full HTML/JS/CSS</h2>
<p>Here is the full HTML with Javascript and a bit of CSS included.</p>
<pre class="EnlighterJSRAW" data-enlighter-language="html">&lt;!doctype html&gt;

&lt;html lang="en"&gt;

&lt;head&gt;
&lt;meta charset="utf-8"&gt;

&lt;title&gt;Add select drop-down filter to DataTable&lt;/title&gt;
&lt;meta name="description" content=""&gt;
&lt;link rel="stylesheet" href="https://cdn.datatables.net/1.10.23/css/jquery.dataTables.min.css" /&gt;
&lt;link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" /&gt;
&lt;style&gt;
select.form-control{
display: inline;
width: 200px;
margin-left: 25px;
}
&lt;/style&gt;
&lt;/head&gt;

&lt;body&gt;
&lt;div class="container mt-4"&gt;
&lt;!-- Create the drop down filter --&gt;
&lt;div class="category-filter"&gt;
&lt;select id="categoryFilter" class="form-control"&gt;
&lt;option value=""&gt;Show All&lt;/option&gt;
&lt;option value="Classical"&gt;Classical&lt;/option&gt;
&lt;option value="Hip Hop"&gt;Hip Hop&lt;/option&gt;
&lt;option value="Jazz"&gt;Jazz&lt;/option&gt;
&lt;/select&gt;
&lt;/div&gt;

    &lt;!-- Set up the datatable --&gt;
    &lt;table class="table" id="filterTable"&gt;
      &lt;thead&gt;
        &lt;tr&gt;
          &lt;th scope="col"&gt;Artist&lt;/th&gt;
          &lt;th scope="col"&gt;Category&lt;/th&gt;
        &lt;/tr&gt;
      &lt;/thead&gt;
      &lt;tbody&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Public Enemy&lt;/td&gt;
          &lt;td scope="col"&gt;Hip Hop&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Chet Baker&lt;/td&gt;
          &lt;td scope="col"&gt;Jazz&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Billie Holiday&lt;/td&gt;
          &lt;td scope="col"&gt;Jazz&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Vivaldi&lt;/td&gt;
          &lt;td scope="col"&gt;Classical&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Jurrasic 5&lt;/td&gt;
          &lt;td scope="col"&gt;Hip Hop&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Onyx&lt;/td&gt;
          &lt;td scope="col"&gt;Hip Hop&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Tchaikovsky&lt;/td&gt;
          &lt;td scope="col"&gt;Classical&lt;/td&gt;
        &lt;/tr&gt;
        &lt;tr&gt;
          &lt;td scope="col"&gt;Oscar Peterson&lt;/td&gt;
          &lt;td scope="col"&gt;Jazz&lt;/td&gt;
        &lt;/tr&gt;
      &lt;/tbody&gt;
    &lt;/table&gt;

&lt;/div&gt;

&lt;script src="https://code.jquery.com/jquery-3.5.1.min.js"
integrity="sha256-9/aliU8dGd2tb6OSsuzixeV4y/faTqgFtohetphbbj0=" crossorigin="anonymous"&gt;&lt;/script&gt;

&lt;script src="https://cdn.datatables.net/1.10.23/js/jquery.dataTables.min.js"&gt;&lt;/script&gt;

&lt;script&gt;
$("document").ready(function () {

      $("#filterTable").dataTable({
        "searching": true
      });

      //Get a reference to the new datatable
      var table = $('#filterTable').DataTable();

      //Take the category filter drop down and append it to the datatables_filter div.
      //You can use this same idea to move the filter anywhere withing the datatable that you want.
      $("#filterTable_filter.dataTables_filter").append($("#categoryFilter"));

      //Get the column index for the Category column to be used in the method below ($.fn.dataTable.ext.search.push)
      //This tells datatables what column to filter on when a user selects a value from the dropdown.
      //It's important that the text used here (Category) is the same for used in the header of the column to filter
      var categoryIndex = 0;
      $("#filterTable th").each(function (i) {
        if ($($(this)).html() == "Category") {
          categoryIndex = i; return false;
        }
      });

      //Use the built in datatables API to filter the existing rows by the Category column
      $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
          var selectedItem = $('#categoryFilter').val()
          var category = data[categoryIndex];
          if (selectedItem === "" || category.includes(selectedItem)) {
            return true;
          }
          return false;
        }
      );

      //Set the change event for the Category Filter dropdown to redraw the datatable each time
      //a user selects a new filter.
      $("#categoryFilter").change(function (e) {
        table.draw();
      });

      table.draw();
    });

&lt;/script&gt;
&lt;/body&gt;

&lt;/html&gt;</pre>

<p><a href="https://clintmcmahon.github.io/add-filter-datatable/" target="_blank" rel="noopener">View full working example.</a><br />
<a href="https://github.com/clintmcmahon/add-filter-datatable" target="_blank" rel="noopener">Download full repo.</a></p>
<!--kg-card-end: html-->
