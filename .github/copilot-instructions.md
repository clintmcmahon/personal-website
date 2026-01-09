# Copilot Instructions for Personal Website

## Project Overview
ASP.NET Core 8 MVC personal website with file-based content management. Blog posts and photos are stored as Markdown files with YAML front matter—no database required.

## Architecture

### Content Flow
```
wwwroot/posts/{year}/*.md → PostRepository → BlogController → Views/Blog/
wwwroot/photos/{year}/*.md → PhotoRepository → PhotoService → PhotosController → Views/Photos/
```

### Key Patterns
- **Repository Pattern**: `IPostRepository`/`PostRepository` for posts, `PhotoRepository` for photos
- **Service Layer**: `PhotoService` wraps repository with navigation logic (previous/next)
- **Middleware**: `RedirectMiddleware` redirects root-level slugs (e.g., `/my-post`) to `/blog/my-post`

### DI Registration (Program.cs)
```csharp
builder.Services.AddTransient<IPostRepository, PostRepository>();  // Posts: transient
builder.Services.AddSingleton<PhotoRepository>(...);               // Photos: singleton
builder.Services.AddSingleton<PhotoService>();
```

## Content Conventions

### Blog Posts
Location: `wwwroot/posts/{year}/{date}-{slug}.md`

Front matter format:
```yaml
---
title: "Post Title"
description: "SEO description"
date: "2025-01-10"
draft: false
slug: "post-slug"
tags: tag1, tag2
---
```

- Filename pattern: `YYYY-MM-DD-slug.md` (e.g., `2025-01-10-primary-constructors.md`)
- Posts with `draft: true` are filtered from public views
- Content supports HTML within Markdown (uses Markdig with advanced extensions)

### Photos
Location: `wwwroot/photos/{year}/*.md`

Required front matter: `title`, `date`, `image`, optional `slug` and `tags`

### Image Organization
- `wwwroot/photos/{year}/` - Photo post markdown files (dedicated photo gallery entries)
- `wwwroot/images/{year}/` - Regular website images and images embedded in standard blog posts

## Views & Controllers

### Controller → View Mapping
- Controllers in `Controllers/` follow standard MVC naming
- Views in `Views/{ControllerName}/`
- Shared layouts/partials in `Views/Shared/`
- Partials prefixed with underscore: `_PostSummary.cshtml`, `_BlogPostFooter.cshtml`

### Common ViewData Keys
- `ViewData["Title"]` - Page title (used in `<title>` and OG tags)
- `ViewData["Description"]` - Meta description

## Development Commands
```bash
dotnet restore    # Restore packages
dotnet build      # Build project
dotnet run        # Run at http://localhost:5000
dotnet watch      # Hot reload during development
```

## Key Dependencies
- **Markdig** (v0.38.0): Markdown parsing with YAML front matter support
- **Bootstrap 5**: CSS framework (via `wwwroot/lib/bootstrap/`)
- **GLightbox**: Photo gallery lightbox (via `wwwroot/lib/glightbox/`)
- **Prism.js**: Code syntax highlighting (CDN in `_Layout.cshtml`)

## Writing Voice

When writing or editing blog posts:

- **Tone**: Conversational and approachable—like explaining something to a fellow developer over coffee
- **POV**: First person for personal experiences and opinions; second person for tutorials and how-tos
- **Audience**: Developers familiar with .NET, web development, and modern software practices
- **Technical depth**: Include working code examples; explain the "why" not just the "how"
- **Style**: Keep paragraphs short, use straight forward context, use headers to break up content, prefer paragraphs over bullet points for lists
- **Personality**: Authentic and practical—share what worked, what didn't, and lessons learned

## Adding New Content

### New Blog Post
1. Create `wwwroot/posts/{year}/{date}-{slug}.md`
2. Add front matter with required fields
3. No code changes needed—PostRepository auto-discovers `.md` files

### New Static Page
1. Create controller action with `[Route]` attribute if custom URL needed
2. Create view in `Views/{Controller}/`
3. Update navigation in `Views/Shared/_Layout.cshtml` if needed

### New Service Page
Copy pattern from existing service pages in `Views/Services/` (e.g., `CustomSoftware.cshtml`)
