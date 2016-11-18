---
identifier: portalusecases
title: Portal use cases
layout: default
---

<h3 id="uc07">Create Client Configuration</h3>

{% highlight csharp %}

const string organizationNumber = "123456789";
const string certificateThumbprint = "3k 7f 30 dd 05 d3 b7 fc...";

var clientConfiguration = new ClientConfiguration(
    Environment.DifiTest,
    certificateThumbprint,
    new Sender(organizationNumber));
{% endhighlight %}

<blockquote>
Note: If the sender changes per signature job created, the sender can be set on the job itself. The sender of the job will always take precedence over the sender in <code>ClientConfiguration</code>. This means that a default sender can be set in <code>ClientConfiguration</code> and, when required, on a specific job.   
</blockquote>

<h3 id="uc08">Create and send portal signature job</h3>

The following example shows how to create a document and send it to two signers.

{% highlight csharp %}

PortalClient portalClient = null; //As initialized earlier

var documentToSign = new Document(
    "Subject of Message",
    "This is the content",
    FileType.Pdf,
    @"C:\Path\ToDocument\File.pdf"
    );

var signers = new List<Signer>
{
    new Signer(new PersonalIdentificationNumber("00000000000"), new NotificationsUsingLookup()),
    new Signer(new PersonalIdentificationNumber("11111111111"), new Notifications(
        new Email("test@example.com"),
        new Sms("999999999")))
};

var portalJob = new Job(documentToSign, signers, "myReferenceToJob");

var portalJobResponse = await portalClient.Create(portalJob);


{% endhighlight %}


<h3 id="uc09">Get portal job status change</h3>

All changes to signature jobs will be added to a queue. You can poll for these changes. If the queue is empty, then additional polling will give an exception. The following example shows how this can be handled and examples of data to extract from a change response.

{% highlight csharp %}

PortalClient portalClient = null; //As initialized earlier

var jobStatusChanged = await portalClient.GetStatusChange();

if (jobStatusChanged.Status == JobStatus.NoChanges)
{
    //Queue is empty. Additional polling will result in blocking for a defined period.
}
else
{
    var signatureJobStatus = jobStatusChanged.Status;
    var signatures = jobStatusChanged.Signatures;
    var signatureOne = signatures.ElementAt(0);
    var signatureOneStatus = signatureOne.SignatureStatus;
}

//Polling again:
try
{
    var changeResponse2 = await portalClient.GetStatusChange();
}
catch (TooEagerPollingException eagerPollingException)
{
    var nextAvailablePollingTime = eagerPollingException.NextPermittedPollTime;
}

{% endhighlight %}

<h3 id="uc10">Get Xades and Pades</h3>

When getting Xades and Pades for a `PortalJob`, remember that the Xades is per signer, while there is only one Pades. 

{% highlight csharp %}

PortalClient portalClient = null; //As initialized earlier
var jobStatusChanged = await portalClient.GetStatusChange();

//Get Xades:
var xades = await portalClient.GetXades(jobStatusChanged.Signatures.ElementAt(0).XadesReference);

//Get Pades:
var pades = await portalClient.GetPades(jobStatusChanged.PadesReference);

{% endhighlight %}

<h3 id="uc11">Confirm portal job</h3>

{% highlight csharp %}

PortalClient portalClient = null; //As initialized earlier
var jobStatusChangeResponse = await portalClient.GetStatusChange();

await portalClient.Confirm(jobStatusChangeResponse.ConfirmationReference);

{% endhighlight %}