---
title: "Failed to decrypt a column encryption key"
description: ""
date: "2022-03-16"
draft: false
slug: "failed-to-decrypt-a-column-encryption-key"
tags:
---

<!--kg-card-begin: html-->
<p>If you set up <a href="https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/always-encrypted-database-engine?view=sql-server-ver15" data-type="URL" data-id="https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/always-encrypted-database-engine?view=sql-server-ver15" target="_blank" rel="noreferrer noopener">SQL Server Always Encrypted</a> and are now getting an error that says  <em><strong>Failed to decrypt a column encryption key using key store provider: &#8216;MSSQL_CERTIFICATE_STORE&#8217; </strong> </em>there&#8217;s a good chance it&#8217;s a permissions issue. All you need to do to fix this error is to give the user running the client accessing the data permissions to the Always Encrypted Certificate that was generated when you created the encryption keys. This user is different than the user connecting to the SQL Server, this is a Windows account that needs access to the local machine certificate store on the client machine.</p>

<p><a href="https://clintmcmahon.com/set-up-column-level-encryption-always-encrypted/" data-type="post" data-id="787">Always Encryption works</a> by allowing each client requesting the data to decrypt the data on the client side via a generated certificate. Therefore, all the data remains encrypted from at rest until it reaches the client. If the client requesting the data does not have the correct certificate or access to the certificate, the decryption fails and you will receive this error message. </p>

<p></p>

<h2>Failed to decrypt a column encryption key using key store provider: &#8216;MSSQL_CERTIFICATE_STORE&#8217;. The last 10 bytes of the encrypted column encryption key are: &#8216;XX-XX-XX-XX-XX-XX-XX-XX-XX-XX&#8217;.</h2>

<p></p>

<p></p>

<h2>Solution: Give client permissions to the generated certificate</h2>

<ol><li>Open the local machine certificate manager on the client machine. This can be either your local machine or a web server. Whichever computer you installed the certificate on. Either type <em>certlm </em>into the run command or type <em>Manage Computer Certificates</em> into the search bar and you&#8217;ll see the certificate manager appear.</li><li>Navigate to Personal &#8211;&gt; Certificates &#8211;&gt; Right click [Your Always Encrypted Certificate]</li><li>Select All Tasks &#8211;&gt; Manage Private Keys&#8230;</li><li>Add permissions for the user who is running the client application that decrypts the data. If this is a website then the user would be the user running the application pool. If it&#8217;s SQL Server Management Studio than most likely you will want to give yourself or the whoever is running SSMS access to this certificate.</li><li>Rerun your query and you will see that error is gone and decrypted data will be returned .</li></ol>

<h2></h2>
<!--kg-card-end: html-->
