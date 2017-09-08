---
identifier: portalusecases
title: Portal use cases
layout: default
---

### Create Client Configuration

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

### Create and send portal signature job

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
        new Email("email1@example.com"),
        new Sms("999999999"))),
    new Signer(new ContactInformation{ Email = new Email("email2@example.com")}),
    new Signer(new ContactInformation{ Sms = new Sms("88888888")}),
    new Signer(new ContactInformation{ Email = new Email("email3@example.com"), Sms = new Sms("77777777")}),
};

var portalJob = new Job(documentToSign, signers, "myReferenceToJob");

var portalJobResponse = await portalClient.Create(portalJob);

{% endhighlight %}

> Note that only public organizations can do `NotificationsUsingLookup`.

#### Specify signature type and required authentication level

{% highlight csharp %}

Document documentToSign = null; //As initialized earlier
var signers = new List<Signer>
{
    new Signer(new PersonalIdentificationNumber("00000000000"), new NotificationsUsingLookup())
    {
        SignatureType = SignatureType.AdvancedSignature
    }
};

var job = new Job(documentToSign, signers, "myReferenceToJob")
{
    AuthenticationLevel = AuthenticationLevel.Four
};

{% endhighlight %}

If signature type or required authentication level is omitted, default values as specified by [the functional documentation](http://digipost.github.io/signature-api-specification/v1.0/#signaturtype) will apply.

### Get portal job status change

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

### Get XAdES and PAdES

When getting XAdES and PAdES for a `PortalJob`, remember that the XAdES is per signer, while there is only one PAdES. 

{% highlight csharp %}

PortalClient portalClient = null; //As initialized earlier
var jobStatusChanged = await portalClient.GetStatusChange();

//Get XAdES:
var xades = await portalClient.GetXades(jobStatusChanged.Signatures.ElementAt(0).XadesReference);

//Get PAdES:
var pades = await portalClient.GetPades(jobStatusChanged.PadesReference);

{% endhighlight %}

### Confirm portal job

{% highlight csharp %}

PortalClient portalClient = null; //As initialized earlier
var jobStatusChangeResponse = await portalClient.GetStatusChange();

await portalClient.Confirm(jobStatusChangeResponse.ConfirmationReference);

{% endhighlight %}