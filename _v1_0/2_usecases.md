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

<h3 id="uc06">Create portal signature client</h3>

{% highlight csharp %}

var organizationNumber = "012345678910";
var certificateThumbprint = "3k 7f 30 dd 05 d3 b7 fc...";

var clientConfiguration = new ClientConfiguration(
        signatureServiceRoot: new Uri("http://serviceroot.digipost.no"),
        sender: new Sender(organizationNumber),
        certificateThumbprint: certificateThumbprint
    );
            
var portalClient = new PortalClient(clientConfiguration);

{% endhighlight %}

<h3 id="uc07">Create and send portal signature job</h3>

The following example shows how to create a document and send it to two signers.

{% highlight csharp %}

ClientConfiguration clientConfiguration = null; //As initialized earlier
var directClient = new DirectClient(clientConfiguration);

var documentToSign = new Document(
        subject: "Subject of Message",
        message: "This is the content",
        fileName: "TheFileName.pdf",
        fileType: FileType.Pdf,
        documentPath: @"C:\Path\ToDocument\File.pdf"
        );


var signers = new List<Signer>
{
    new Signer("01013300001"),
    new Signer("01024403041")
};

var portalJob = new PortalJob(documentToSign, signers, "myReferenceToJob");
var portalJobResponse = await portalClient.Create(portalJob);

{% endhighlight %}


<h3 id="uc08">Get portal job status</h3>

All changes to signature jobs will be added to a queue. You can poll for these changes. If the queue is empty, then additional polling will give an exception. The following exception shows how this can be handled and examples of data to extract from a change response.

{% highlight csharp %}

var portalJobStatusChangeResponse = await portalClient.GetStatusChange();

if (portalJobStatusChangeResponse == null)
{
    //queue is empty. Additional polling will result in blocking for a defined period.
}
else
{
    var signatureJobStatus = portalJobStatusChangeResponse.Status;
    var signatures = portalJobStatusChangeResponse.Signatures;
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

<h3 id="uc09">Get Xades and Pades</h3>

{% highlight csharp %}

//Get Xades:
var xades = await portalClient.GetXades(portalJobStatusChangeResponse.Signatures.ElementAt(0).XadesReference);

//Get Pades:
var pades = await portalClient.GetPades(portalJobStatusChangeResponse.PadesReference);

{% endhighlight %}


<h3 id="uc10">Confirm portal job</h3>

{% highlight csharp %}

await portalClient.Confirm(portalJobStatusChangeResponse.ConfirmationReference);

{% endhighlight%}
