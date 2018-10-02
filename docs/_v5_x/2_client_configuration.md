---
identifier: clientconfiguration
title: Client configuration
layout: default
---

## Based on user secrets

```csharp
const string organizationNumber = "123456789";

var clientConfiguration = new ClientConfiguration(
    Environment.DifiTest,
    CertificateReader.ReadCertificate(),
    new Sender(organizationNumber));
```

## Based on certificate thumbprint

```csharp
const string organizationNumber = "123456789";
const string certificateThumbprint = "3k 7f 30 dd 05 d3 b7 fc...";

var clientConfiguration = new ClientConfiguration(
    Environment.DifiTest,
    certificateThumbprint,
    new Sender(organizationNumber));
```

## Overriding the configured sender

<blockquote>
Note: If the sender changes per signature job created, the sender can be set on the job itself. The sender of the job will always take precedence over the sender in <code>ClientConfiguration</code>. This means that a default sender can be set in <code>ClientConfiguration</code> and, when required, on a specific job.   
</blockquote>
