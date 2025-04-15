// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

window.addEventListener('load', function () {
  new Masonry('#gallery', {
    itemSelector: 'a',
    columnWidth: 200,
    percentPosition: true
  });

  lightGallery(document.getElementById('gallery'), {
    plugins: [lgThumbnail, lgZoom],
    thumbnail: true,
    zoom: true
  });
});