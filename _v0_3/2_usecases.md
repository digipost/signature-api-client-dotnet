---
title: Use cases
id: usecases
layout: default
isHome: false
---

<h3 id="uc01">Create Client Configuration</h3>

{% highlight csharp %}

var organizationNumber = "012345678910";
var certificateThumbprint = "3k 7f 30 dd 05 d3 b7 fc...";

var clientConfiguration = new ClientConfiguration(
    signatureServiceRoot: new Uri("http://serviceroot.digipost.no"), 
    sender: new Sender(organizationNumber),
    certificateThumbprint: certificateThumbprint);

{% endhighlight %}

<h3 id="uc02">Create signature job</h3>

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);

var documentToSign = new Document(
    subject: "Subject of Message",
    message: "This is the content",
    fileName: "TheFileName.pdf",
    fileType: FileType.Pdf,
    documentPath: @"C:\Path\ToDocument\File.pdf");

var exitUrls = new ExitUrls(
    completionUrl: new Uri("http://redirectUrl.no/onCompletion"),
    cancellationUrl: new Uri("http://redirectUrl.no/onCancellation"),
    errorUrl: new Uri("http://redirectUrl.no/onError")
    );

var directJob = new DirectJob(
    sender: clientConfiguration.Sender, 
    document: documentToSign, 
    signer: new Signer(personalIdentificationNumber: "01013302201"), 
    reference: "SendersReferenceToSignatureJob", 
    exitUrls: exitUrls
    );

DirectJobResponse directJobResponse = await directClient.Create(directJob);

{% endhighlight %}

<h3 id="uc03">Get signature job status</h3>

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);
DirectJobResponse directJobResponse = null; //As initialized when creating signature job

var directJobStatusResponse = 
    await directClient.GetStatus(directJobResponse.StatusReference);

var jobStatus = directJobStatusResponse.JobStatus;


{% endhighlight %}

<h3 id="uc04">Get Xades And Pades</h3>

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);
DirectJobStatusResponse directJobStatusResponse = null; // Result of requesting job status

switch (directJobStatusResponse.JobStatus)
{
    case JobStatus.Created:
        //Signature job is not signed, Xades and Pades cannot be requested.
        break;
    case JobStatus.Cancelled:
        //Signature job was cancelled, Xades and Pades cannot be requested.
        break;
    case JobStatus.Signed:
        var xadesByteStream = await directClient.GetXades(directJobStatusResponse.JobReferences.Xades);
        var padesByteStream = await directClient.GetPades(directJobStatusResponse.JobReferences.Pades);
        break;
}

{% endhighlight %}

<h3 id="uc05">Confirm received signature job</h3>

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);
DirectJobStatusResponse directJobStatusResponse = null; // Result of requesting job status

var confirm = await directClient.Confirm(directJobStatusResponse.JobReferences.Confirmation);

{% endhighlight %}