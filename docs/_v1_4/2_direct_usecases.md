---
identifier: directusecases
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
    "Subject of Message", 
    "This is the content", 
    FileType.Pdf, 
    @"C:\Path\ToDocument\File.pdf");

var exitUrls = new ExitUrls(
    new Uri("http://redirectUrl.no/onCompletion"), 
    new Uri("http://redirectUrl.no/onCancellation"), 
    new Uri("http://redirectUrl.no/onError")
    );

var job = new Job(
    documentToSign, 
    new Signer(new PersonalIdentificationNumber("12345678910")), 
    "SendersReferenceToSignatureJob", 
    exitUrls
    );

var directJobResponse = await directClient.Create(job);

{% endhighlight %}

<h3 id="uc03">Get direct job status</h3>

The signing process is a synchrounous operation in the direct use case. There is no need to poll for changes to a signature job, as the status is well known to the sender of the job. As soon as the signer cancels, completes or an error occurs, the user is redirected to the respective Urls set in `ExitUrls`. A `status_query_token` parameter has been added to the url. Use this when requesting a status change.

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);
JobResponse jobResponse = null; //As initialized when creating signature job
var statusQueryToken = "0A3BQ54C...";

var jobStatusResponse =
    await directClient.GetStatus(jobResponse.ResponseUrls.Status(statusQueryToken));

var jobStatus = jobStatusResponse.Status;

{% endhighlight %}

<h3 id="uc04">Get direct job status by polling</h3>

If you, for any reason, are unable to retrieve status by using the status query token specified <a href="#uc03">above</a>, you may poll the service for any changes done to your organization's jobs. If the queue is empty, additional polling will give an exception.

<blockquote>Note: For the job to be available in the polling queue, make sure to specify the job's <code>statusRetrievalMethod</code> as illustrated below.</blockquote>

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; // As initialized earlier
var directClient = new DirectClient(clientConfiguration);

Document documentToSign = null; // As initialized earlier
ExitUrls exitUrls = null; // As initialized earlier

var job = new Job(
    documentToSign,
    new Signer(new PersonalIdentificationNumber("12345678910")),
    "SendersReferenceToSignatureJob",
    exitUrls,
    statusRetrievalMethod: StatusRetrievalMethod.Polling
    );

await directClient.Create(job);

var changedJob = await directClient.GetStatusChange();

if (changedJob.Status == JobStatus.NoChanges)
{
    // Queue is empty. Additional polling will result in blocking for a defined period.
}

// Repeat the above until signer signs the document

changedJob = await directClient.GetStatusChange();

if (changedJob.Status == JobStatus.Signed)
{
    // Get XAdES and/or PAdES
}

// Confirm status change to avoid receiving it again
await directClient.Confirm(changedJob.References.Confirmation);

{% endhighlight %}

<h3 id="uc05">Get Xades And Pades</h3>

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);
JobStatusResponse jobStatusResponse = null; // Result of requesting job status

switch (jobStatusResponse.Status)
{
    case JobStatus.Rejected:
        //Signature job was rejected by the signer. Xades and Pades cannot be requested.
        break;
    case JobStatus.Failed:
        //Signature job failed, Xades and Pades cannot be requested.
        break;
    case JobStatus.Signed:
        var xadesByteStream = await directClient.GetXades(jobStatusResponse.References.Xades);
        var padesByteStream = await directClient.GetPades(jobStatusResponse.References.Pades);
        break;
}

{% endhighlight %}

<h3 id="uc06">Confirm received signature job</h3>

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);
JobStatusResponse jobStatusResponse = null; // Result of requesting job status

await directClient.Confirm(jobStatusResponse.References.Confirmation);

{% endhighlight %}
