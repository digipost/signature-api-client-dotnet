---
id: setup
title: Initial setup
layout: default
---

The client library is available as a nuget package. The client library (and associated nuget package) is updated regularly as new functionality is added. 

<blockquote> The version number will be a good indicator, as a change in the major or minor version will have breaking changes. </blockquote>

To install the nuget package, follow these steps in Visual Studio:

1. Select _TOOLS -> nuget Package Manager -> Manage nuget Packages Solution..._
2. Search for "_digipost-signature-api-client-dotnet_."
* If you would like pre-releases of this package, make sure _Include Prerelease_ is enabled. Please refer to documentation for your version of Visual Studio for detailed instructions.
3. Select _digipost-signature-api-client_ and click _Install_.

<h3 id="businesscertificate">Install business certificate in certificate store</h3>

<blockquote>SSL Certificates are small data files that digitally bind a cryptographic key to an organization's details. When installed on a web server, it activates the padlock and the https protocol (over port 443) and allows secure connections from a web server to a browser.</blockquote>

To communicate over HTTPS you need to sign your request with a business certificate. The following steps will install the certificate in the your certificate store. This should be done on the server where your application will run.

1.  Double-click on the actual certificate file (CertificateName.p12)
2.  Save the sertificate in _Current User_ and click _Next_ 
3.  USe the suggested filename. Click _Next_
4.  Enter password for private key and select _Mark this key as exportable ..._ Click _Next_
5.  Select _Automatically select the certificate store based on the type of certificate_
6.  Click _Next_ and _Finish_
7.  Accept the certificate if prompted.
8.  When prompted that the import was successful, click _Ok_.

<h3 id="find_businesscertificate">Use business certificate thumbprint</h3>

1. Start mmc.exe (Click windowsbutton and type mmc.exe)
2. Choose File -> Add/Remove Snap-in…(Ctrl + M)
3. Mark certificate and click Add >
4. Choose 'My user account' followed by Finish, then 'OK'.
5. Double-click on 'Certificates' 
6. Double-click on the installed certificate
7. Go to the 'Details' tab
8. Scroll down to 'Thumbprint'
9. Copy the thumbprint.