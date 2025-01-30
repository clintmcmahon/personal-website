---
title: "Unsupported OS Version In Xcode"
description: ""
date: "2021-09-21"
draft: false
slug: "unsupported-os-version-in-xcode"
tags:
---

<!--kg-card-begin: html-->
<p>Last night I updated my iPhone OS to <a href="https://www.apple.com/ios/ios-15/" data-type="URL" data-id="https://www.apple.com/ios/ios-15/" target="_blank" rel="noreferrer noopener nofollow">iOS 15</a>. This morning I plugged my phone into my Macbook Air to work on an app but was unable to run the app on my phone because of the new operating system. The app is targeted to run on my iPhone under iOS Device the my phone&#8217;s iOS was listed as unsupported. Specifically <strong>Clint&#8217;s iPhone (unsupported OS version)</strong>. This makes sense since I updated the iOS to version 15 but haven&#8217;t made any changes locally to support the new version of the software.</p>

<p>Here are the steps I took to get iOS 15 support for my Mac without updating Xcode or the OS.</p>

<ol><li>Open the DeviceSupport folder at <code>Applications/xcode.app/contents/Developer/platform/iPhoneOS.platform/DeviceSupport</code><ol><li>Right click the Xcode app and select &#8220;Show Package Contents&#8221; to get into the XCode.app folder</li></ol></li><li>Download the <a href="https://github.com/iGhibli/iOS-DeviceSupport/blob/master/DeviceSupport/15.0(FromXcode_13_beta_xip).zip" target="_blank" data-type="URL" data-id="https://github.com/iGhibli/iOS-DeviceSupport/blob/master/DeviceSupport/15.0(FromXcode_13_beta_xip).zip" rel="noreferrer noopener nofollow">DeviceSupport folder for iOS 15</a> (or any other iOS version you might be needing) and place inside the DeviceSupport folder alongside the other supported operating systems</li><li>Clean Xcode build folder (Product -> Clean Build Folder)</li><li>Delete the DerivedData folder at <code>/Users/[user]/Library/Developer/Xcode/DerivedData</code></li><li>Close Xcode</li><li>Unplug and restart your phone</li><li>After your phone has restarted, plug it back in and open Xcode. You should now be able to see your phone listed as a supported device with iOS 15.</li></ol>

<p>Many thanks to these <a href="https://stackoverflow.com/questions/67863355/xcode-12-4-unsupported-os-version-after-iphone-ios-update-14-7" target="_blank" data-type="URL" data-id="https://stackoverflow.com/questions/67863355/xcode-12-4-unsupported-os-version-after-iphone-ios-update-14-7" rel="noreferrer noopener">Stack Overflow</a> and <a href="https://developer.apple.com/forums/thread/673131" target="_blank" data-type="URL" data-id="https://developer.apple.com/forums/thread/673131" rel="noreferrer noopener">Apple Developer Forum posts</a> for guiding me through the process.</p>
<!--kg-card-end: html-->
