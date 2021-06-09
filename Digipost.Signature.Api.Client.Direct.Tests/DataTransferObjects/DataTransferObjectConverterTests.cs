using System;
using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Schemas;
using Xunit;
using static Digipost.Signature.Api.Client.Core.Tests.Utilities.CoreDomainUtility;

namespace Digipost.Signature.Api.Client.Direct.Tests.DataTransferObjects
{
    public class DataTransferObjectConverterTests
    {
        public class ToDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [Fact]
            public void Converts_direct_job_successfully()
            {
                var documents = DomainUtility.GetSingleDirectDocument();
                var signer = DomainUtility.GetSigner();
                var jobTitle = "JobTitle";
                var reference = "reference";
                var exitUrls = DomainUtility.GetExitUrls();
                var statusretrieval = statusretrievalmethod.WAIT_FOR_CALLBACK;
                var sender = new Sender(BringPublicOrganizationNumber);

                var source = new Job(
                    jobTitle,
                    documents,
                    signer,
                    reference,
                    exitUrls,
                    sender);

                var expected = new directsignaturejobrequest
                {
                    reference = reference,
                    exiturls = new exiturls
                    {
                        completionurl = source.ExitUrls.CompletionUrl.AbsoluteUri,
                        rejectionurl = source.ExitUrls.RejectionUrl.AbsoluteUri,
                        errorurl = source.ExitUrls.ErrorUrl.AbsoluteUri
                    },
                    statusretrievalmethod = statusretrieval,
                    statusretrievalmethodSpecified = true
                };
                
                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_direct_job_successfully_with_polling_queue_successfully()
            {
                var documents = DomainUtility.GetSingleDirectDocument();
                var signer = DomainUtility.GetSigner();
                var reference = "reference";
                var exitUrls = DomainUtility.GetExitUrls();
                var statusretrieval = statusretrievalmethod.WAIT_FOR_CALLBACK;
                var pollingQueue = new PollingQueue("CustomPollingQueue");
                var sender = new Sender(BringPublicOrganizationNumber, pollingQueue);

                var source = new Job(
                    "jobTitle",
                    documents,
                    signer,
                    reference,
                    exitUrls,
                    sender);

                var expected = new directsignaturejobrequest
                {
                    reference = reference,
                    exiturls = new exiturls
                    {
                        completionurl = source.ExitUrls.CompletionUrl.AbsoluteUri,
                        rejectionurl = source.ExitUrls.RejectionUrl.AbsoluteUri,
                        errorurl = source.ExitUrls.ErrorUrl.AbsoluteUri
                    },
                    statusretrievalmethod = statusretrieval,
                    statusretrievalmethodSpecified = true,
                    pollingqueue = pollingQueue.Name
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_exit_urls_successfully()
            {
                //Arrange
                var source = DomainUtility.GetExitUrls();
                var expected = new exiturls
                {
                    completionurl = source.CompletionUrl.AbsoluteUri,
                    rejectionurl = source.RejectionUrl.AbsoluteUri,
                    errorurl = source.ErrorUrl.AbsoluteUri
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }
        }

        public class FromDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [Fact]
            public void Converts_direct_job_status_with_multiple_signers_successfully()
            {
                //Arrange
                var now = DateTime.Now;

                var source = new directsignaturejobstatusresponse
                {
                    signaturejobid = 77,
                    reference = null,
                    signaturejobstatus = directsignaturejobstatus.FAILED,
                    status = new[]
                    {
                        new signerstatus {signer = "12345678910", Value = "REJECTED", since = now},
                        new signerstatus {signer = "10987654321", Value = "SIGNED", since = now}
                    },
                    xadesurl = new[]
                    {
                        new signerspecificurl {signer = "10987654321", Value = "https://example.com/xades-url"}
                    },
                    confirmationurl = "https://example.com/confirmation-url"
                };

                var nextPermittedPollTime = DateTime.Now;

                var expected = new JobStatusResponse(
                    source.signaturejobid,
                    source.reference,
                    JobStatus.Failed,
                    new JobReferences(new Uri("https://example.com/confirmation-url"), null),
                    new List<Signature>
                    {
                        new Signature("12345678910", null, SignatureStatus.Rejected, now),
                        new Signature("10987654321", new XadesReference(new Uri("https://example.com/xades-url")), SignatureStatus.Signed, now)
                    },
                    nextPermittedPollTime
                );

                //Act
                var result = DataTransferObjectConverter.FromDataTransferObject(source, nextPermittedPollTime);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_document_successfully()
            {
                //Arrange
                const string title = "JobTitle";
                const string message = "Message";
                const FileType fileType = FileType.Pdf;
                var documentBytes = new byte[] {0x21, 0x22};

                var source = new Document(
                    title,
                    fileType,
                    documentBytes
                );

                var expected = new directdocument
                {
                    title = title,
                    href = source.FileName,
                    mime = "application/pdf"
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_manifest_successfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";
                const string jobTitle = "jobTitle";
                const string jobDescription = "Message";
                const string documentTitle = "documentTitle";
                var pdfDocumentBytes = GetPdfDocumentBytes();
                var personalIdentificationNumber = "12345678901";
                var expectedMimeType = "application/pdf";
                var document = new Document(documentTitle, FileType.Pdf, pdfDocumentBytes);
                
                var source = new Manifest(
                    jobTitle,
                    new Sender(organizationNumberSender),
                    new List<Document>{document}, 
                    new[] {new Signer(new PersonalIdentificationNumber(personalIdentificationNumber))}
                )
                {
                    Description = jobDescription
                };

                var expected = new directsignaturejobmanifest
                {
                    title = jobTitle,
                    description = jobDescription,
                    sender = new sender {organizationnumber = organizationNumberSender},
                    documents = new []{ new directdocument
                    {
                        title = documentTitle,
                        href = document.FileName,
                        mime = expectedMimeType
                    } },
                    signer = new[]
                    {
                        new directsigner
                        {
                            ItemElementName = ItemChoiceType.personalidentificationnumber,
                            Item = personalIdentificationNumber,
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true
                        }
                    }
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_manifest_with_authentication_level_successfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";
                const string jobTitle = "jobTitle";
                const string jobDescription = "Message";
                const string documentTitle = "documentTitle";
                var pdfDocumentBytes = GetPdfDocumentBytes();
                var personalIdentificationNumber = "12345678901";
                var expectedMimeType = "application/pdf";
                var document = new Document(documentTitle, FileType.Pdf, pdfDocumentBytes);

                var source = new Manifest(
                    jobTitle,
                    new Sender(organizationNumberSender),
                    new List<Document>{ document },
                    new[] {new Signer(new PersonalIdentificationNumber(personalIdentificationNumber))}
                )
                {
                    Description = jobDescription,
                    AuthenticationLevel = AuthenticationLevel.Four
                };

                var expected = new directsignaturejobmanifest
                {
                    title = jobTitle,
                    description = jobDescription,
                    sender = new sender {organizationnumber = organizationNumberSender},
                    documents = new []
                    { new directdocument {
                            title = documentTitle,
                            href = document.FileName,
                            mime = expectedMimeType
                        }
                    },
                    signer = new[]
                    {
                        new directsigner
                        {
                            ItemElementName = ItemChoiceType.personalidentificationnumber,
                            Item = personalIdentificationNumber,
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true
                        }
                    },
                    requiredauthentication = authenticationlevel.Item4,
                    requiredauthenticationSpecified = true
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_manifest_with_signature_type_successfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";
                const string jobTitle = "jobTitle";
                const string jobDescription = "Message";
                const string documentTitle = "documentTitle";
                var pdfDocumentBytes = GetPdfDocumentBytes();
                var personalIdentificationNumber = "12345678901";
                var expectedMimeType = "application/pdf";
                var document = new Document(documentTitle, FileType.Pdf, pdfDocumentBytes);

                var source = new Manifest(
                    jobTitle,
                    new Sender(organizationNumberSender),
                    new [] { document },
                    new[]
                    {
                        new Signer(new PersonalIdentificationNumber(personalIdentificationNumber))
                        {
                            SignatureType = SignatureType.AdvancedSignature
                        }
                    }
                )
                {
                    Description = jobDescription
                };

                var expected = new directsignaturejobmanifest
                {
                    title = jobTitle,
                    description = jobDescription,
                    sender = new sender {organizationnumber = organizationNumberSender},
                    documents = new []
                    { new directdocument
                        {
                            title = documentTitle,
                            href = document.FileName,
                            mime = expectedMimeType
                        }
                    },
                    signer = new[]
                    {
                        new directsigner
                        {
                            ItemElementName = ItemChoiceType.personalidentificationnumber,
                            Item = personalIdentificationNumber,
                            signaturetype = signaturetype.ADVANCED_ELECTRONIC_SIGNATURE,
                            signaturetypeSpecified = true,
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true
                        }
                    }
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_sender_successfully()
            {
                //Arrange
                const string organizationNumber = "123456789";

                var source = new Sender(organizationNumber);
                var expected = new sender
                {
                    organizationnumber = organizationNumber
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_signed_direct_job_status_successfully()
            {
                //Arrange
                var now = DateTime.Now;

                var source = new directsignaturejobstatusresponse
                {
                    signaturejobid = 77,
                    reference = "senders-reference",
                    signaturejobstatus = directsignaturejobstatus.COMPLETED_SUCCESSFULLY,
                    status = new[] {new signerstatus {signer = "12345678910", Value = "SIGNED", since = now}},
                    confirmationurl = "http://signatureRoot.digipost.no/confirmation",
                    xadesurl = new[] {new signerspecificurl {signer = "12345678910", Value = "http://signatureRoot.digipost.no/xades"}},
                    padesurl = "http://signatureRoot.digipost.no/pades"
                };

                var nextPermittedPollTime = DateTime.Now;

                var expected = new JobStatusResponse(
                    source.signaturejobid,
                    source.reference,
                    JobStatus.CompletedSuccessfully,
                    new JobReferences(new Uri(source.confirmationurl), new Uri(source.padesurl)),
                    new List<Signature> {new Signature("12345678910", new XadesReference(new Uri("http://signatureRoot.digipost.no/xades")), SignatureStatus.Signed, now)},
                    nextPermittedPollTime
                );

                //Act
                var result = DataTransferObjectConverter.FromDataTransferObject(source, nextPermittedPollTime);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_signer_successfully()
            {
                //Arrange
                const string personalIdentificationNumber = "0123456789";

                var source = new Signer(new PersonalIdentificationNumber(personalIdentificationNumber));
                var expected = new directsigner
                {
                    ItemElementName = ItemChoiceType.personalidentificationnumber,
                    Item = personalIdentificationNumber,
                    onbehalfof = signingonbehalfof.SELF,
                    onbehalfofSpecified = true
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_signer_with_signer_identifier_successfully()
            {
                //Arrange
                const string customIdentifier = "custom-identifier";

                var source = new Signer(new CustomIdentifier(customIdentifier)) {OnBehalfOf = OnBehalfOf.Other};
                var expected = new directsigner
                {
                    ItemElementName = ItemChoiceType.signeridentifier,
                    Item = customIdentifier,
                    onbehalfofSpecified = true,
                    onbehalfof = signingonbehalfof.OTHER
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_unsigned_direct_job_status_successfully()
            {
                //Arrange
                var now = DateTime.Now;
                var nextPermittedPollTime = DateTime.Now;

                var source = new directsignaturejobstatusresponse
                {
                    signaturejobid = 77,
                    reference = "senders-reference",
                    signaturejobstatus = directsignaturejobstatus.FAILED,
                    status = new[] {new signerstatus {signer = "12345678910", Value = "REJECTED", since = now}},
                    confirmationurl = "https://example.com/confirmation-url"
                };

                var expected = new JobStatusResponse(
                    source.signaturejobid,
                    source.reference,
                    JobStatus.Failed,
                    new JobReferences(new Uri("https://example.com/confirmation-url"), null),
                    new List<Signature> {new Signature("12345678910", null, SignatureStatus.Rejected, now)},
                    nextPermittedPollTime
                );

                //Act
                var result = DataTransferObjectConverter.FromDataTransferObject(source, nextPermittedPollTime);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Empty(differences);
            }
        }
    }
}
