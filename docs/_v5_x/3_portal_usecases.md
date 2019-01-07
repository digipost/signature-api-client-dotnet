---
identifier: portalusecases
title: Portal use cases
layout: default
---

### Create and send portal signature job

The following example shows how to create a document and send it to two signers.

``` csharp
ClientConfiguration clientConfiguration = null; //As initialized earlier
var portalClient = new PortalClient(clientConfiguration);

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
    new Signer(new ContactInformation {Email = new Email("email2@example.com")}),
    new Signer(new ContactInformation {Sms = new Sms("88888888")}),
    new Signer(new ContactInformation
    {
        Email = new Email("email3@example.com"),
        Sms = new Sms("77777777")
    })
};

var portalJob = new Job(documentToSign, signers, "myReferenceToJob");

var portalJobResponse = await portalClient.Create(portalJob);

```

> Note that only public organizations can do `NotificationsUsingLookup`.

#### Specify signature type and required authentication level

``` csharp

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

```

If signature type or required authentication level is omitted, default values as specified by [the functional documentation](http://digipost.github.io/signature-api-specification/v1.0/#signaturtype) will apply.

### Get portal job status change

All changes to signature jobs will be added to a queue. You can poll for these changes. All changes must be confirmed after saving or handling them in your system. The following example shows how this can be handled and examples of data to extract from a change response.
> Note: If you retrieve a status change, it will be temporarily removed from the queue. If not confirmed it will reappear after some time. 

``` csharp

PortalClient portalClient = null; //As initialized earlier

var jobStatusChanged = await portalClient.GetStatusChange();

if (jobStatusChanged.Status == JobStatus.NoChanges)
{
    //Queue is empty. The status change includes next earliest permitted poll time.
}
else
{
    var signatureJobStatus = jobStatusChanged.Status;
    var signatures = jobStatusChanged.Signatures;
    var signatureOne = signatures.ElementAt(0);
    var signatureOneStatus = signatureOne.SignatureStatus;

    //TODO: Persist job status change in your system, to ensure you have the latest status if anything crashes beyond this point.
    
    // Confirm that you have received and persisted the status change
    await portalClient.Confirm(jobStatusChangeResponse.ConfirmationReference);
        
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

```

### Get XAdES and PAdES

When getting XAdES and PAdES for a `PortalJob`, remember that the XAdES is per signer, while there is only one PAdES. 

``` csharp

PortalClient portalClient = null; //As initialized earlier
var jobStatusChanged = await portalClient.GetStatusChange();

//Get XAdES:
var xades = await portalClient.GetXades(jobStatusChanged.Signatures.ElementAt(0).XadesReference);

//Get PAdES:
var pades = await portalClient.GetPades(jobStatusChanged.PadesReference);

```

### Specifying queues

Specifies the queue that jobs and status changes for a signature job will occur in. This is a feature aimed at organizations where it makes sense to retrieve status changes from several queues. This may be if the organization has more than one division, and each division has an application that create signature jobs through the API and want to retrieve status changes independent of the other division's actions.

To specify a queue, set `Sender.PollingQueue` through the constructor `Sender(string, PollingQueue)`. Please note that the same sender must be specified when polling to retrieve status changes. The `Sender` can be set globally in `ClientConfiguration` or on every `Job`.

``` csharp

PortalClient portalClient = null; //As initialized earlier

var organizationNumber = "123456789";
var sender = new Sender(organizationNumber, new PollingQueue("CustomPollingQueue"));

var documentToSign = new Document(
    "Subject of Message",
    "This is the content",
    FileType.Pdf,
    @"C:\Path\ToDocument\File.pdf"
);

var signers = new List<Signer>
{
    new Signer(new PersonalIdentificationNumber("00000000000"), new NotificationsUsingLookup())
};

var portalJob = new Job(documentToSign, signers, "myReferenceToJob", sender);

var portalJobResponse = await portalClient.Create(portalJob);

var changedJob = await portalClient.GetStatusChange(sender);

```