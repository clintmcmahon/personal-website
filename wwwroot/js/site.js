﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
  $(".pictures").tjGallery({
    selector: "img",
    row_min_height: 300,
  });
});
