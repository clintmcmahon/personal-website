---
title: "How To Fix Missing 'sub' Claim an ASP.NET JWT"
description: "If you're using Azure AD B2C and can't find the 'sub' claim in your Web API, ASP.NET Core's default claim mapping is likely the culprit. Here's how to work with JWT claims directly."
date: "2025-06-12"
draft: false
slug: "aspnet-core-sub-claim-mapping"
tags: consulting
---

If you're using Azure AD B2C with ASP.NET Core and can't find the `sub` claim in your JWT response token, it's because ASP.NET remaps it by default to `namesidentifier`. Instead of `"sub"`, the claim shows up as:

```
http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier
```

This remapping behavior comes from legacy WS-Federation support and can be confusing when you're working with modern JWT tokens. So if you're checking for `"sub"` directly in your controller or middleware, it's not going to be there. This was frustrating for me. I could see the `"sub"` claim when I viewed the token in jwt.ms, but debugging in code it wasn't there.

## The Problem

When inspecting your token at [jwt.io](https://jwt.io) or [jwt.ms](https://jwt.ms), you’ll see a clean set of claims like:

```json
{
  "sub": "a3c897b1-73c4-4bfa-9373-4e2b6c9a1234",
  "email": "user@example.com",
  "name": "Jane Doe"
}
```

But in ASP.NET Core, accessing `sub` like this:

```csharp
User.FindFirst("sub")
```

...returns `null`. That’s because ASP.NET maps `sub` to a long-form URI:

```csharp
User.FindFirst(ClaimTypes.NameIdentifier)
```

This mapping happens automatically to align with older identity protocols, but it’s often not what you want.

## The Solution

To avoid the confusion and use the JWT claims exactly as they appear in the token, disable the default claim type mapping during startup:

```csharp
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
```

Place this before configuring JWT bearer authentication:

```csharp
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "roles"
        };
    });
```

Now, you can reliably access the `sub` claim like this:

```csharp
var userId = User.FindFirst("sub")?.Value;
```

This ensures consistency between the raw JWT and how you access claims in your API.

## Common JWT Claim Mappings

Here are some common JWT claims and their default .NET Core mappings:

| JWT Claim      | .NET Core Mapped Claim Type                                         |
|----------------|---------------------------------------------------------------------|
| `sub`          | `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier` |
| `email`        | `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress`  |
| `given_name`   | `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname`     |
| `family_name`  | `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname`       |
| `name`         | `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name`          |
| `role`         | `http://schemas.microsoft.com/ws/2008/06/identity/claims/role`        |

Disabling claim mapping gives you a direct, modern experience—exactly how your token was issued.

## Conclusion

This small configuration tweak can saved me hours of debugging and make authentication logic more predictable. If you're working with JWTs in ASP.NET Core, especially in Azure AD B2C, disabling claim mapping is a best practice that helps avoid issues like this and keeps claim handling straightforward.

