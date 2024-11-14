---
title: "How to set up column level encryption with Always Encrypted"
description: ""
date: "2022-03-08"
draft: false
slug: "set-up-column-level-encryption-always-encrypted"
tags:
---

<!--kg-card-begin: html-->
<p>In this post I&#8217;m going to detail how to set up column level encryption with Always Encrypted. <a href="https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/always-encrypted-database-engine?view=sql-server-ver15" target="_blank" rel="noopener noreferrer">SQL Server&#8217;s Always Encrypted</a> feature is a simple and effective way to setup column level encryption. Using Always Encryption I&#8217;m able to encrypt individual columns in my database without having to worry about encrypting the entire database. It&#8217;s also very easy to setup column level encryption after the initial configuration is done. Once a column is encrypted the data can only be read by a client that can decrypt the data using a generated certificate. Because of the level of encryption all the data is encrypted at rest as well as in transit.</p>

<p>Here&#8217;s how to configure Always Encryption on a single database column. I&#8217;m going to walk through encrypting a Social Security Number column named SSN. If you want to know more details about Always Encryption and more configuration details I recommend this <a href="https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/configure-always-encrypted-using-sql-server-management-studio?view=sql-server-2017" target="_blank" rel="noreferrer noopener">Configure Always Encrypted using SQL Server Management Studio</a> page.</p>

<h2 id="set-up-column-level-encryption-with-always-encrypted">Set up column level encryption with Always Encrypted</h2>

<ol><li>Run SQL Server Management Studio as an administrator. You will need to have admin rights on your machine in order to do this.</li><li>Create a new Column Master Key<ul><li>Navigate to your database &#8211;&gt; Security &#8211;&gt; Always Encrypted Keys </li><li>Right click <em>Column Master Keys</em></li><li>Select <em>New Column Master Key</em></li><li>Give your master key a name. I like to start mine with CMK followed by the objective. Something like CMK_SSN since I&#8217;m encrypting the social security number.</li><li>Set Key store to <em>Windows Certificate Store &#8211; Local Machine</em>. This part is key, you want the certificate to be available to any user that needs it on your machine. If you select <em>Windows Certificate Store &#8211; Local User</em> you can&#8217;t decrypt the values as any other user. Like if your IIS instance is running as a different user. You will also need to be running SQL Server Management Studio as an admin otherwise this option will not be available to you.</li><li>Click <em>Generate Certificate</em> at the bottom of the table of certificates. You should now see a new certificate in the table of certificates. Select the newly created certificate.</li><li>Click <em>OK</em></li></ul></li><li>Create a new Column Encryption Key<ul><li>Navigate to your database &#8211;&gt; Security &#8211;&gt; Always Encrypted Keys </li></ul><ul><li>Right click <em>Column Encryption Keys</em></li><li>Select <em>New Column Encryption Key</em></li><li>Enter a name for your column encryption key. I like to start with CEK followed by the objective. Something like CEK_SSN since I&#8217;m encrypting the social security number.</li><li>Select your column master key you created above from the drop down labeled <em>Column master key</em></li><li>Click <em>OK</em></li></ul></li><li><a href="https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/overview-of-key-management-for-always-encrypted?view=sql-server-ver15" data-type="URL" data-id="https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/overview-of-key-management-for-always-encrypted?view=sql-server-ver15" target="_blank" rel="noreferrer noopener nofollow">Learn the differences between Column Master Keys and Column Encryption Keys here</a></li><li>Now that the encryption keys are set, it&#8217;s time to encrypt the the column. Navigate to Tables &#8211;&gt; <em>Your Table</em> &#8211;&gt; Columns, right click your column to encrypt and select <em>Encrypt Column</em>. In my case the I&#8217;m selecting the column SSN.</li><li>Select the checkbox next to your column. In the <em>Encryption Type</em> column choose either <em>Deterministic </em> or <em>Randomized</em><ul><li>Select Deterministic if you plan to use this column in any look ups or queries such as you would in a where clause. This method creates the same encrypted value for same plaintext value for each value that is encrypted.</li><li>Select Randomized if you will won&#8217;t use this column in a where clause or by any look ups at the database level. This method creates a unique encrypted value for each plaintext value that is encrypted.</li></ul></li><li>Select the Encryption Key you created in the previous steps (CEK_SSN)</li><li>Click <em>Next &#8211;&gt; Next</em> &#8211;&gt; Next &#8211;&gt; <em>Finish</em></li><li>The encryption process will begin and you&#8217;ll see a message stating the operation has passed when it&#8217;s finished</li><li>Click <em>Close</em></li><li>Your column is now set up for Always Encryption.</li></ol>

<h2 id="configure-sql-management-studio-for-always-encryption">Configure SQL Management Studio for Always Encryption</h2>

<p>There are a couple tasks you have to complete in order to get SQL Server Management Studio to work with Always Encryption. </p>

<ol><li>The first is to disconnect your current connection and reconnect to the server instance but with the following options:<ul><li>Click <em>Options &gt;&gt;</em> in the lower right corner of the Connect to Server window</li><li>Select the <em>Always Encrypted</em> tab</li><li>Check the box next to <em>Enable Always Encrypted (column encryption)</em>. This will tell SQL Server that you want to try to decrypt any encrypted columns</li></ul><ul><li>Click OK</li></ul></li><li>The next task is to enable Parameterization for Always Encryption. This will allow you to send and use parameters in your queries from SSMS when working with encrypted columns.<ul><li>Navigate to Tools &#8211;&gt; Options &#8211;&gt; Execution &#8211;&gt; Advanced and enable Parameterization for Always Encryption</li></ul></li><li>You will now be able to insert data into an encrypted column. </li></ol>

<h2 id="example-insert-statement">Example insert statement</h2>

<p>Run the below query to insert a new encrypted record into the database. </p>

<div style="height: 250px; position:relative; margin-bottom: 50px;" class="wp-block-simple-code-block-ace"><pre class="wp-block-simple-code-block-ace" style="position:absolute;top:0;right:0;bottom:0;left:0" data-mode="sql" data-theme="github" data-fontsize="14" data-lines="Infinity" data-showlines="true" data-copy="false">declare @ssn nvarchar(max) = '123-46-5444'

insert into DemoTable
values (@ssn)

select \* from DemoTable</pre></div>

<p>You will now have data saved in the encrypted column. Because <em>Enable Always Encryption</em> was checked at the connection window the results are returned decrypted. </p>

<figure class="wp-block-image size-full is-resized"><img decoding="async" loading="lazy" src="/images/wordpress/2022/03/with.jpg" alt="" class="wp-image-1201"/ sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<p>In order to see the encrypted values, reconnect from the server instance and uncheck the <em>Enable Always Encryption</em> checkbox in the Always Encrypted tab. This checkbox tells SQL Server to try to decrypt the encrypted columns. You should now see the encrypted values in the database table.</p>

<figure class="wp-block-image size-full"><img decoding="async" loading="lazy" src="/images/wordpress/2022/03/encrypted.jpg" alt="" class="wp-image-1202" sizes="(max-width: 401px) 100vw, 401px" / sizes="(max-width: 720px) 100vw, 720px" class="kg-image" loading="lazy" style="width: 100%; height: auto; max-width: 100%;"></figure>

<p></p>

<h2>Errors</h2>

<p>After you&#8217;ve set up Always Encryption you might run into this error:</p>

<p><strong>Failed to decrypt a column encryption key using key store provider: ‘MSSQL_CERTIFICATE_STORE’. The last 10 bytes of the encrypted column encryption key are: ‘XX-XX-XX-XX-XX-XX-XX-XX-XX-XX’.</strong></p>

<p>This error is likely due to a permissions issue when your client tries to access the generated certificate. <a href="https://clintmcmahon.com/failed-to-decrypt-a-column-encryption-key/" data-type="URL" data-id="https://clintmcmahon.com/failed-to-decrypt-a-column-encryption-key/">Walking through these steps</a> should help you solve this issue.</p>

<h2 id="conclusion">Conclusion</h2>

<p>That covers how to enable and configure Always Encryption on a database column. There are a few hoops to run through to get it set up and running, but overall I really enjoy working with Always Encryption. Once it&#8217;s up and running there&#8217;s little maintenance that needs to be done so I find that it&#8217;s a good option when I need to encrypt data at the database level.</p>
<!--kg-card-end: html-->
