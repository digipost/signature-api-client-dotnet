---
id: directusecases
title: Direct use cases
layout: default
---

<h3 id="uc01">Create Client Configuration</h3>

{% highlight csharp %}

const string organizationNumber = "123456789";
const string certificateThumbprint = "3k 7f 30 dd 05 d3 b7 fc...";

var clientConfiguration = new ClientConfiguration(
    Environment.DifiQa,
    certificateThumbprint,
    new Sender(organizationNumber));

{% endhighlight %}

<blockquote>
Note: If the sender changes per signature job created, the sender can be set on the job itself. The sender of the job will always take precedence over the sender in <code>ClientConfiguration</code>. This means that a default sender can be set in <code>ClientConfiguration</code> and, when required, on a specific job.   
</blockquote>

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
    document: documentToSign, 
    signer: new Signer(personalIdentificationNumber: "12345678910"), 
    reference: "SendersReferenceToSignatureJob", 
    exitUrls: exitUrls
    );

var directJobResponse = await directClient.Create(directJob);

{% endhighlight %}

<h3 id="uc03">Get direct job status</h3>

The signing process is a synchrounous operation in the direct use case. There is no need to poll for changes to a signature job, as the status is well known to the sender of the job. As soon as the signer cancels, completes or an error occurs, the user is redirected to the respective Urls set in `ExitUrls`. A `status_query_token` parameter has been added to the url. Use this when requesting a status change.

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);
DirectJobResponse directJobResponse = null; //As initialized when creating signature job
var statusQueryToken = "0A3BQ54C...";

var directJobStatusResponse =
    await directClient.GetStatus(directJobResponse.ResponseUrls.Status(statusQueryToken));

var jobStatus = directJobStatusResponse.Status;

{% endhighlight %}

<h3 id="uc04">Get Xades And Pades</h3>

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);
DirectJobStatusResponse directJobStatusResponse = null; // Result of requesting job status

switch (directJobStatusResponse.Status)
{
    case JobStatus.Rejected:
        //Signature job was rejected by the signer. Xades and Pades cannot be requested.
        break;
    case JobStatus.Cancelled:
        //Signature job was cancelled, Xades and Pades cannot be requested.
        break;
    case JobStatus.Signed:
        var xadesByteStream = await directClient.GetXades(directJobStatusResponse.References.Xades);
        var padesByteStream = await directClient.GetPades(directJobStatusResponse.References.Pades);
        break;
}

{% endhighlight %}

<h3 id="uc05">Confirm received signature job</h3>

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);
DirectJobStatusResponse directJobStatusResponse = null; // Result of requesting job status

await directClient.Confirm(directJobStatusResponse.JobReferences.Confirmation);

{% endhighlight %}