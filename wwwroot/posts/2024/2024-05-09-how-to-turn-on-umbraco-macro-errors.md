---
title: "How to throw an error inside an Umbraco Macro"
description: "This is a post about how to throw Umbraco Macro errors or how to turn off the generic error "Error loading Partial View script(....)" when loading Umbraco Macros in your Razor view."
date: "2024-05-09"
draft: false
slug: "/how-to-turn-on-umbraco-macro-errors"
tags:

---

<p>This is a post about how to throw Umbraco Macro errors or how to turn off the generic error "Error loading Partial View script(....)" when loading Umbraco Macros in your Razor view.</p><p>In Umbraco 13, there is a MacroErrors property that defines how the errors inside Macro Partial Views are displayed and handeled. By default, the property is set to <code>Inline</code> to display a generic message that something went wrong with your file. The message reads: "Error loading Partial View script". I'm upgrading an Umbraco instance for a client and needed to know what the actual error is but was banging my head trying to figure out why the error wouldn't display normally. </p><p>After some digging I found that throwing the stack trace error behavior is not standard and I need to configure the ability to throw the error in the <code>appsettings.json</code> file. To do this, open the <code>appsettings.json</code> file and update the <code>Umbraco\CMS\MacroErrors</code> node to include the <code>MacroErrors</code> property with the value <code>Throw</code> like so:</p><pre><code class="language-json ">"Umbraco": {
  "CMS": {
    "Global": {
      "Id": "",
      "SanitizeTimyMce": true
    },
    "Content": {
      "AllowEditInvariantFromNonDefault": true,
      "ContentVersionCleanupPolicy": {
        "EnableCleanup": true
      },
      "MacroErrors": "Throw"
    },
    "Unattended": {
      "UpgradeUnattended": true
    },
    "Security": {
      "AllowConcurrentLogins": false
    }
  }
},</code></pre><p>Everyone's <code>Umbraco</code> node will be slightly different, but the main thing to update here is the <code>MacroErrors</code> node to be set to <code>Throw</code>. </p><p>Out of the box Umbraco doesn't include <code>MacroErrors</code> in the <code>appsettings.json</code> file, so chances are you need to add this node yourself. I hope this saves someone some debugging time. Happy coding!</p>
