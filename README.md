# .NET 8 MVC Personal Website Starter
This is the codebase for my own personal website. It can act as a portfolio, blog, and project showcase — all built with ASP.NET Core 8 MVC. I made this public because I want other developers and designers to have a solid, modern starting point for their own sites. Fork it, remix it, and make it your own!

## Why This Exists

I couldn't find a .NET-based personal site template that was clean, fast, and easy to customize—so I built one. If you want a site that's IndieWeb-friendly, easy to blog with, and looks great on any device, you're in the right place.

## Features

- **.NET 8 MVC**: Latest ASP.NET Core patterns, easy to extend.
- **Portfolio & Projects**: Show off your work with dedicated pages.
- **Blog**: Write in Markdown, with SEO-friendly slugs and year-based folders.
- **Photo Gallery**: Modern CSS masonry layout with GLightbox for beautiful photo posts.
- **IndieWeb & IndieAuth**: Microformats, h-card, and IndieAuth support (optional, but cool).
- **Service Pages**: Value-based, consulting-style service pages (easy to adapt for your own skills or offerings).
- **Responsive & Accessible**: Works on any device, with semantic HTML and accessible markup.
- **SEO Optimized**: Metadata, Open Graph, and internal linking best practices.

## Quick Start

1. **Clone the repo**
   ```sh
   git clone https://github.com/YOUR-USERNAME/personal-website.git
   cd personal-website
   ```
2. **Restore and build**
   ```sh
   dotnet restore
   dotnet build
   ```
3. **Run locally**
   ```sh
   dotnet run
   ```
4. **Visit**
   Open [http://localhost:5000](http://localhost:5000) in your browser.

## Make It Yours

### 1. Remove My Content
- **Posts**: Swap out or delete my markdown files in `wwwroot/posts/`.
- **Photos**: Replace images in `wwwroot/photos/` with your own.
- **Service Pages**: Update or rewrite the pages in `Views/Services/` for your own services or skills.
- **About/Contact**: Update `AboutController`, `ContactController`, and their views.
- **Branding**: Change the name, logo, favicon, and any references to "Clint McMahon" in `_Layout.cshtml` and shared views.

### 2. Update Metadata
- Edit `appsettings.json` and `appsettings.Development.json` for your site title, description, and social links.
- Update Open Graph and SEO meta tags in `_Layout.cshtml`.

### 3. IndieWeb & Auth (Optional)
- If you want IndieAuth, update the IndieAuth endpoints and h-card microformats in the layout and profile pages.
- Remove IndieWeb features if you don't need them.

### 4. Blog & Portfolio
- Add your own markdown posts to `wwwroot/posts/{year}/`.
- Add portfolio/project entries in the appropriate controllers and views.

### 5. Design & CSS
- Tweak or replace `wwwroot/css/site.css` and any custom styles.
- Update `_Layout.cshtml` for your preferred navigation and footer.

## Deploying
- Publish to any .NET 8-compatible host (Azure, Vercel, DigitalOcean, etc.).
- Use `dotnet publish` for production builds.
- Make sure your web server serves static files from `wwwroot/`.

## What’s "Clint McMahon"-Specific?
- All the content, posts, images, and service descriptions are mine.
- The default branding, favicon, and social links.
- Some IndieWeb and IndieAuth config (update or remove as needed).
- Any references to "clintmcmahon.com" in code or config.

## Questions? Ideas? PRs?
If you have questions, want to show off your fork, or have ideas for improvements, open an issue or pull request! I love seeing what people build with this.

## License
[MIT License](https://opensource.org/licenses/MIT) — open source and free to use.

## Credits
- [GLightbox](https://github.com/biati-digital/glightbox) for the photo gallery.
- [ASP.NET Core](https://dotnet.microsoft.com/) for the web framework.
- IndieWeb community for standards inspiration.
