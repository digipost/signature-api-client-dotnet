using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Enums;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests.DataTransferObjects
{
    public class DataTransferObjectConverterTests
    {
        public class ToDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [Fact]
            public void Converts_direct_job_successfully()
            {
                var document = DomainUtility.GetDirectDocument();
                var signer = DomainUtility.GetSigner();
                var reference = "reference";
                var exitUrls = DomainUtility.GetExitUrls();
                var statusretrieval = statusretrievalmethod.WAIT_FOR_CALLBACK;

                var source = new Job(
                    document,
                    signer,
                    reference,
                    exitUrls);

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
                Assert.Equal(0, differences.Count());
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
                Assert.Equal(0, differences.Count());
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

                var expected = new JobStatusResponse(
                    source.signaturejobid,
                    JobStatus.Failed,
                    new JobReferences(new Uri("https://example.com/confirmation-url"), null),
                    new List<Signature>
                    {
                        new Signature(new SignerIdentifier("12345678910"), null, SignatureStatus.Rejected, now),
                        new Signature(new SignerIdentifier("10987654321"), new XadesReference(new Uri("https://example.com/xades-url")), SignatureStatus.Signed, now)
                    }
                );

                //Act
                var result = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Converts_direct_job_successfully()
            {
                //Arrange
                var redirecturl = "https://localhost:9000/redirect/#/c316e5b62df86a5d80e517b3ff4532738a9e7e43d4ae6075d427b1b58355bc63";
                var source = new directsignaturejobresponse
                {
                    signaturejobid = 1444,
                    redirecturl = new[] {new signerspecificurl {signer = "12345678910", Value = redirecturl}},
                    statusurl = "https://localhost:8443/api/signature-jobs/77/status"
                };

                var expected = new JobResponse(
                    source.signaturejobid,
                    new ResponseUrls(
                        new List<RedirectReference> {new RedirectReference(new Uri(redirecturl), new PersonalIdentificationNumber("12345678910"))},
                        new Uri(source.statusurl)
                    )
                );

                //Act
                var result = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Converts_direct_job_with_multiple_signers_successfully()
            {
                //Arrange
                var redirecturl = "https://localhost:9000/redirect/#/some-reference";
                var redirecturl2 = "https://localhost:9000/redirect/#/some-other-reference";
                var source = new directsignaturejobresponse
                {
                    signaturejobid = 1444,
                    redirecturl = new[]
                    {
                        new signerspecificurl {signer = "12345678910", Value = redirecturl},
                        new signerspecificurl {signer = "10987654321", Value = redirecturl2}
                    },
                    statusurl = "https://localhost:8443/api/signature-jobs/77/status"
                };

                var expected = new JobResponse(
                    source.signaturejobid,
                    new ResponseUrls(
                        new List<RedirectReference>
                        {
                            new RedirectReference(new Uri(redirecturl), new PersonalIdentificationNumber("12345678910")),
                            new RedirectReference(new Uri(redirecturl2), new PersonalIdentificationNumber("10987654321"))
                        },
                        new Uri(source.statusurl)
                    )
                );

                //Act
                var result = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Converts_document_successfully()
            {
                //Arrange
                const string subject = "Subject";
                const string message = "Message";
                const FileType fileType = FileType.Pdf;
                var documentBytes = new byte[] {0x21, 0x22};

                var source = new Document(
                    subject,
                    message,
                    fileType,
                    documentBytes
                );

                var expected = new directdocument
                {
                    title = subject,
                    description = message,
                    href = source.FileName,
                    mime = "application/pdf"
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Converts_manifest_successfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";
                const string documentSubject = "Subject";
                const string documentMessage = "Message";
                var pdfDocumentBytes = CoreDomainUtility.GetPdfDocumentBytes();
                var personalIdentificationNumber = "12345678901";
                var expectedMimeType = "application/pdf";

                var source = new Manifest(
                    new Sender(organizationNumberSender),
                    new Document(documentSubject, documentMessage, FileType.Pdf, pdfDocumentBytes),
                    new[] {new Signer(new PersonalIdentificationNumber(personalIdentificationNumber))}
                );

                var expected = new directsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new directdocument
                    {
                        title = documentSubject,
                        description = documentMessage,
                        href = source.Document.FileName,
                        mime = expectedMimeType
                    },
                    signer = new[] {new directsigner
                    {
                        ItemElementName = ItemChoiceType.personalidentificationnumber,
                        Item = personalIdentificationNumber,
                        onbehalfof = signingonbehalfof.SELF,
                        onbehalfofSpecified = true
                    }}
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Converts_manifest_with_authentication_level_successfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";
                const string documentSubject = "Subject";
                const string documentMessage = "Message";
                var pdfDocumentBytes = CoreDomainUtility.GetPdfDocumentBytes();
                var personalIdentificationNumber = "12345678901";
                var expectedMimeType = "application/pdf";

                var source = new Manifest(
                    new Sender(organizationNumberSender),
                    new Document(documentSubject, documentMessage, FileType.Pdf, pdfDocumentBytes),
                    new[] {new Signer(new PersonalIdentificationNumber(personalIdentificationNumber))}
                )
                {
                    AuthenticationLevel = AuthenticationLevel.Four
                };

                var expected = new directsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new directdocument
                    {
                        title = documentSubject,
                        description = documentMessage,
                        href = source.Document.FileName,
                        mime = expectedMimeType
                    },
                    signer = new[] {new directsigner
                    {
                        ItemElementName = ItemChoiceType.personalidentificationnumber,
                        Item = personalIdentificationNumber,
                        onbehalfof = signingonbehalfof.SELF,
                        onbehalfofSpecified = true
                    }},
                    requiredauthentication = authenticationlevel.Item4,
                    requiredauthenticationSpecified = true
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Converts_manifest_with_signature_type_successfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";
                const string documentSubject = "Subject";
                const string documentMessage = "Message";
                var pdfDocumentBytes = CoreDomainUtility.GetPdfDocumentBytes();
                var personalIdentificationNumber = "12345678901";
                var expectedMimeType = "application/pdf";

                var source = new Manifest(
                    new Sender(organizationNumberSender),
                    new Document(documentSubject, documentMessage, FileType.Pdf, pdfDocumentBytes),
                    new[]
                    {
                        new Signer(new PersonalIdentificationNumber(personalIdentificationNumber))
                        {
                            SignatureType = SignatureType.AdvancedSignature
                        }
                    }
                );

                var expected = new directsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new directdocument
                    {
                        title = documentSubject,
                        description = documentMessage,
                        href = source.Document.FileName,
                        mime = expectedMimeType
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
                Assert.Equal(0, differences.Count());
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
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Converts_signed_direct_job_status_successfully()
            {
                //Arrange
                var now = DateTime.Now;

                var source = new directsignaturejobstatusresponse
                {
                    signaturejobid = 77,
                    signaturejobstatus = directsignaturejobstatus.COMPLETED_SUCCESSFULLY,
                    status = new[] {new signerstatus {signer = "12345678910", Value = "SIGNED", since = now}},
                    confirmationurl = "http://signatureRoot.digipost.no/confirmation",
                    xadesurl = new[] {new signerspecificurl {signer = "12345678910", Value = "http://signatureRoot.digipost.no/xades"}},
                    padesurl = "http://signatureRoot.digipost.no/pades"
                };

                var expected = new JobStatusResponse(
                    source.signaturejobid,
                    JobStatus.CompletedSuccessfully,
                    new JobReferences(new Uri(source.confirmationurl), new Uri(source.padesurl)),
                    new List<Signature> {new Signature(new SignerIdentifier("12345678910"), new XadesReference(new Uri("http://signatureRoot.digipost.no/xades")), SignatureStatus.Signed, now)}
                );

                //Act
                var result = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Equal(0, differences.Count());
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
                Assert.Equal(0, differences.Count());
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
                    onbehalfof = signingonbehalfof.OTHER,
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Equal(0, differences.Count());
            }

            [Fact]
            public void Converts_unsigned_direct_job_status_successfully()
            {
                //Arrange
                var now = DateTime.Now;

                var source = new directsignaturejobstatusresponse
                {
                    signaturejobid = 77,
                    signaturejobstatus = directsignaturejobstatus.FAILED,
                    status = new[] {new signerstatus {signer = "12345678910", Value = "REJECTED", since = now}},
                    confirmationurl = "https://example.com/confirmation-url",
                };

                var expected = new JobStatusResponse(
                    source.signaturejobid,
                    JobStatus.Failed,
                    new JobReferences(new Uri("https://example.com/confirmation-url"), null),
                    new List<Signature> {new Signature(new SignerIdentifier("12345678910"), null, SignatureStatus.Rejected, now)}
                );

                //Act
                var result = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.Equal(0, differences.Count());
            }
        }
    }
}