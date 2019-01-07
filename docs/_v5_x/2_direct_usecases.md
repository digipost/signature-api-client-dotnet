---
identifier: directusecases
title: Direct use cases
layout: default
---

### Create signature job

``` csharp
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

var signers = new List<Signer>
{
    new Signer(new PersonalIdentificationNumber("12345678910")),
    new Signer(new PersonalIdentificationNumber("10987654321"))
};

var job = new Job(documentToSign, signers, "SendersReferenceToSignatureJob", exitUrls);

var directJobResponse = await directClient.Create(job);

```

#### Specify signature type and required authentication level

``` csharp

Document documentToSign = null; //As initialized earlier
ExitUrls exitUrls = null; //As initialized earlier
var signers = new List<Signer>
{
    new Signer(new PersonalIdentificationNumber("12345678910"))
    {
        SignatureType = SignatureType.AdvancedSignature
    }
};

var job = new Job(documentToSign, signers, "SendersReferenceToSignatureJob", exitUrls)
{
    AuthenticationLevel = AuthenticationLevel.Four
};

```

If signature type or required authentication level is omitted, default values as specified by [the functional documentation](http://digipost.github.io/signature-api-specification/v1.0/#signaturtype) will apply.

### Get direct job status

The signing process is a synchrounous operation in the direct use case. There is no need to poll for changes to a signature job, as the status is well known to the sender of the job. As soon as the signer cancels, completes or an error occurs, the user is redirected to the respective Urls set in `ExitUrls`. A `status_query_token` parameter has been added to the url. Use this when requesting a status change.

``` csharp

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);
JobResponse jobResponse = null; //As initialized when creating signature job
var statusQueryToken = "0A3BQ54C...";

var jobStatusResponse =
    await directClient.GetStatus(jobResponse.ResponseUrls.Status(statusQueryToken));

var jobStatus = jobStatusResponse.Status;

```

### Get direct job status by polling

If you, for any reason, are unable to retrieve status by using the status query token specified <a href="#uc03">above</a>, you may poll the service for any changes done to your organization's jobs. All changes must be confirmed after saving or handling them in your system.

> Note: For the job to be available in the polling queue, make sure to specify the job's <code>statusRetrievalMethod</code> as illustrated below.

``` csharp

ClientConfiguration clientConfiguration = null; // As initialized earlier
var directClient = new DirectClient(clientConfiguration);

Document documentToSign = null; // As initialized earlier
ExitUrls exitUrls = null; // As initialized earlier

var signer = new PersonalIdentificationNumber("00000000000");

var job = new Job(
    documentToSign,
    new List<Signer> {new Signer(signer)},
    "SendersReferenceToSignatureJob",
    exitUrls,
    statusRetrievalMethod: StatusRetrievalMethod.Polling
    );

await directClient.Create(job);

var changedJob = await directClient.GetStatusChange();

if (changedJob.Status == JobStatus.NoChanges)
{
    //Queue is empty. The status change includes next earliest permitted poll time.
}

//TODO: Persist job status change in your system, to ensure you have the latest status if anything crashes beyond this point.
    
// Confirm that you have received and persisted the status change
await directClient.Confirm(changedJob.References.Confirmation);


```

[comment]: <> (Using h3 with specific id to diff from the auto genereted one for portal use cases.)

<h3 id="get-xades-and-pades-direct"> Get XAdES And PAdES</h3>   

``` csharp

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);
JobStatusResponse jobStatusResponse = null; // Result of requesting job status

if (jobStatusResponse.Status == JobStatus.CompletedSuccessfully)
{
    var padesByteStream = await directClient.GetPades(jobStatusResponse.References.Pades);
}

var signature = jobStatusResponse.GetSignatureFor(new PersonalIdentificationNumber("00000000000"));

if (signature.Equals(SignatureStatus.Signed))
{
    var xadesByteStream = await directClient.GetXades(signature.XadesReference);
}

```

[comment]: <> (Using h3 with specific id to diff from the auto genereted one for portal use cases.)

<h3 id="specifying-portal-queue">Specifying queues</h3>

Specifies the queue that jobs and status changes for a signature job will occur in for signature jobs where `StatusRetrievalMethod == Polling` This is a feature aimed at organizations where it makes sense to retrieve status changes from several queues. This may be if the organization has more than one division, and each division has an application that create signature jobs through the API and want to retrieve status changes independent of the other division's actions.

To specify a queue, set `Sender.PollingQueue` through the constructor `Sender(string, PollingQueue)`. Please note that the same sender must be specified when polling to retrieve status changes. The `Sender` can be set globally in `ClientConfiguration` or on every `Job`.

``` csharp

ClientConfiguration clientConfiguration = null; // As initialized earlier
var directClient = new DirectClient(clientConfiguration);

String organizationNumber = "123456789";
var sender = new Sender(organizationNumber, new PollingQueue("CustomPollingQueue"));

Document documentToSign = null; // As initialized earlier
ExitUrls exitUrls = null; // As initialized earlier

var signer = new PersonalIdentificationNumber("00000000000");

var job = new Job(
    documentToSign,
    new List<Signer> { new Signer(signer) },
    "SendersReferenceToSignatureJob",
    exitUrls,
    sender,
    StatusRetrievalMethod.Polling
);

await directClient.Create(job);

var changedJob = await directClient.GetStatusChange(sender);

```

