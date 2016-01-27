---
title: Use cases
id: usecases
layout: default
isHome: false
---

<h3 id="uc01">Create Client Configuration</h3>

{% highlight csharp %}

var organizationNumber = "012345678910";
var certificate = new X509Certificate2(); //Certificate loaded from store

var clientConfiguration = new ClientConfiguration(
    signatureServiceRoot: new Uri("http://serviceroot.digipost.no"), 
    sender: new Sender(organizationNumber),
    certificate: certificate);


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
    await directClient.GetStatus(directJobResponse.DirectJobReference);

var jobStatus = directJobStatusResponse.JobStatus;

{% endhighlight %}

<h3 id="uc04">Get Xades And Pades</h3>

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; 
var directClient = new DirectClient(clientConfiguration);
DirectJobStatusResponse directJobStatusResponse = null;

switch (directJobStatusResponse.JobStatus)
{
    case JobStatus.Created:
        //Signature job is not signed, Xades and Pades cannot be requested.
        break;
    case JobStatus.Cancelled:
        //Signature job was cancelled, Xades and Pades cannot be requested.
        break;
    case JobStatus.Signed:
        var xadesByteStream = directClient.GetXades(
            new XadesReference(directJobStatusResponse.StatusResponseUrls.Xades)
            );
        var padesByteStream = directClient.GetPades(
            new PadesReference(directJobStatusResponse.StatusResponseUrls.Pades)
            );
        break;
}

{% endhighlight %}