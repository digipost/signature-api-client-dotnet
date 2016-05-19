using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Digipost.Signature.Api.Client.Portal.DataTransferObjects;
using Digipost.Signature.Api.Client.Portal.Extensions;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests.DataTransferObjects
{
    [TestClass]
    public class DataTransferObjectConverterTests
    {
        [TestClass]
        public class ToDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [TestMethod]
            public void ConvertsPortalJobSuccessfully()
            {
                //Arrange
                var document = CoreDomainUtility.GetDocument();
                var signers = CoreDomainUtility.GetSigners(2);
                var reference = "reference";
                var source = new PortalJob(document, signers, reference);

                var expected = new portalsignaturejobrequest
                {
                    reference = reference
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsManifestWithoutAvailabilitySuccessfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";

                var source = new PortalManifest(new Sender(organizationNumberSender), CoreDomainUtility.GetDocument(), CoreDomainUtility.GetSigners(2));

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new document
                    {
                        title = source.Document.Subject,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new signer {personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber},
                        new signer {personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber}
                    }
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsManifestWithOrderedSignersSuccessfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";

                var source = new PortalManifest(new Sender(organizationNumberSender), CoreDomainUtility.GetDocument(), new List<Signer> {new Signer("00000000000") {Order = 1}, new Signer("99999999999") {Order = 2}});

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new document
                    {
                        title = source.Document.Subject,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new signer {personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber, order = 1},
                        new signer {personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber, order = 2}
                    }
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsManifestWithOnlyAvailableSecondsSuccessfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";

                var source = new PortalManifest(new Sender(organizationNumberSender), CoreDomainUtility.GetDocument(), CoreDomainUtility.GetSigners(2))
                {
                    Availability = new Availability
                    {
                        AvailableFor = new TimeSpan(0, 0, 1, 0)
                    }
                };

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new document
                    {
                        title = source.Document.Subject,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new signer {personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber},
                        new signer {personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber}
                    },
                    availability = new availability
                    {
                        availableseconds = source.Availability.AvailableSeconds.Value,
                        availablesecondsSpecified = true
                    }
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsManifestWithOnlyActivationAvailabilitySuccessfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";

                var source = new PortalManifest(new Sender(organizationNumberSender), CoreDomainUtility.GetDocument(), CoreDomainUtility.GetSigners(2))
                {
                    Availability = new Availability
                    {
                        Activation = DateTime.Now.AddHours(22)
                    }
                };

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new document
                    {
                        title = source.Document.Subject,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new signer {personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber},
                        new signer {personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber}
                    },
                    availability = new availability
                    {
                        activationtime = source.Availability.Activation.Value,
                        activationtimeSpecified = true
                    }
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsManifestWithAvailabilitySuccessfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";

                var source = new PortalManifest(new Sender(organizationNumberSender), CoreDomainUtility.GetDocument(), CoreDomainUtility.GetSigners(2))
                {
                    Availability = DomainUtility.GetAvailability()
                };

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new document
                    {
                        title = source.Document.Subject,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new signer {personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber},
                        new signer {personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber}
                    },
                    availability = new availability
                    {
                        activationtime = source.Availability.Activation.Value,
                        availableseconds = source.Availability.AvailableSeconds.Value,
                        activationtimeSpecified = true,
                        availablesecondsSpecified = true,
                    }
                };

                //Act
                var result = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, result, out differences);
                Assert.AreEqual(0, differences.Count());
            }
        }

        [TestClass]
        public class FromDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [TestMethod]
            public void ConvertsPortalJobResponseSuccessfully()
            {
                //Arrange
                var signaturejobid = 12345678910;
                var httpCancellationurl = "http://cancellationurl.no";
                var source = new portalsignaturejobresponse
                {
                    signaturejobid = signaturejobid,
                    cancellationurl = httpCancellationurl
                };
                var expected = new PortalJobResponse(signaturejobid, new Uri(httpCancellationurl));

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsPortalJobStatusChangeResponseSuccessfully()
            {
                //Arrange
                var source = new portalsignaturejobstatuschangeresponse
                {
                    confirmationurl = "http://confirmationurl.no",
                    signaturejobid = 12345678901011,
                    signatures = new signatures
                    {
                        padesurl = "http://padesurl.no",
                        signature = new[]
                        {
                            new signature
                            {
                                personalidentificationnumber = "01013300001",
                                status = signaturestatus.SIGNED,
                                xadesurl = "http://xadesurl1.no"
                            },
                            new signature
                            {
                                personalidentificationnumber = "01013300002",
                                status = signaturestatus.WAITING,
                                xadesurl = "http://xadesurl2.no"
                            }
                        }
                    },
                    status = portalsignaturejobstatus.PARTIALLY_COMPLETED
                };

                var jobStatus = source.status.ToJobStatus();
                var confirmationReference = new ConfirmationReference(new Uri(source.confirmationurl));
                var signature1 = source.signatures.signature[0];
                var signature2 = source.signatures.signature[1];
                var signatures = new List<Signature>
                {
                    new Signature
                    {
                        SignatureStatus = signature1.status.ToSignatureStatus(),
                        Signer = new Signer(signature1.personalidentificationnumber),
                        XadesReference = new XadesReference(new Uri(signature1.xadesurl))
                    },
                    new Signature
                    {
                        SignatureStatus = signature2.status.ToSignatureStatus(),
                        Signer = new Signer(signature2.personalidentificationnumber),
                        XadesReference = new XadesReference(new Uri(signature2.xadesurl))
                    }
                };

                var expected = new PortalJobStatusChanged(
                    source.signaturejobid,
                    jobStatus,
                    confirmationReference,
                    signatures
                    );

                var padesUrl = source.signatures.padesurl;
                if (padesUrl != null)
                {
                    expected.PadesReference = new PadesReference(new Uri(padesUrl));
                }

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0, differences.Count());
            }
        }
    }
}