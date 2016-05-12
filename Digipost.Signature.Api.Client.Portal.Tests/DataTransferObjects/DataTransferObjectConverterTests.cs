using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Digipost.Signature.Api.Client.Portal.DataTransferObjects;
using Digipost.Signature.Api.Client.Portal.Enums;
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
                var document = DomainUtility.GetPortalDocument();
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

                var source = new PortalManifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(), CoreDomainUtility.GetSigners(2));

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new portaldocument
                    {
                        title = source.PortalDocument.Title,
                        description = source.PortalDocument.Message,
                        href = source.PortalDocument.FileName,
                        mime = source.PortalDocument.MimeType
                    },
                    signers = new[]
                    {
                        new portalsigner {personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber.Value},
                        new portalsigner {personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber.Value}
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

                var source = new PortalManifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(), new List<Signer> {new PortalSigner(new PersonalIdentificationNumber("00000000000"), new NotificationsUsingLookup()) {Order = 1}, new PortalSigner(new PersonalIdentificationNumber("99999999999"), new NotificationsUsingLookup()) {Order = 2}}); //TODO: Skal ikke bruke lookup

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new portaldocument
                    {
                        title = source.PortalDocument.Title,
                        description = source.PortalDocument.Message,
                        href = source.PortalDocument.FileName,
                        mime = source.PortalDocument.MimeType
                    },
                    signers = new[]
                    {
                        new portalsigner {personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber.Value, order = 1},
                        new portalsigner {personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber.Value, order = 2}
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

                var source = new PortalManifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(), CoreDomainUtility.GetSigners(2))
                {
                    Availability = new Availability
                    {
                        AvailableFor = new TimeSpan(0, 0, 1, 0)
                    }
                };

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new portaldocument
                    {
                        title = source.PortalDocument.Title,
                        description = source.PortalDocument.Message,
                        href = source.PortalDocument.FileName,
                        mime = source.PortalDocument.MimeType
                    },
                    signers = new[]
                    {
                        new portalsigner {personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber.Value},
                        new portalsigner {personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber.Value}
                    },
                    availability = new availability
                    {
                        availableseconds = source.Availability.AvailableSeconds
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

                var source = new PortalManifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(), CoreDomainUtility.GetSigners(2))
                {
                    Availability = new Availability
                    {
                        Activation = DateTime.Now.AddHours(22)
                    }
                };

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new portaldocument
                    {
                        title = source.PortalDocument.Title,
                        description = source.PortalDocument.Message,
                        href = source.PortalDocument.FileName,
                        mime = source.PortalDocument.MimeType
                    },
                    signers = new[]
                    {
                        new portalsigner {personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber.Value},
                        new portalsigner {personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber.Value}
                    },
                    availability = new availability
                    {
                        activationtime = source.Availability.Activation.Value
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

                var source = new PortalManifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(), CoreDomainUtility.GetSigners(2))
                {
                    Availability = DomainUtility.GetAvailability()
                };

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new portaldocument
                    {
                        title = source.PortalDocument.Title,
                        description = source.PortalDocument.Message,
                        href = source.PortalDocument.FileName,
                        mime = source.PortalDocument.MimeType
                    },
                    signers = new[]
                    {
                        new portalsigner {personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber.Value},
                        new portalsigner {personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber.Value}
                    },
                    availability = new availability
                    {
                        activationtime = source.Availability.Activation.Value,
                        availableseconds = source.Availability.AvailableSeconds
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
            public void ConvertsPortalDocument()
            {
                //Arrange
                var source = new PortalDocument("subject", "message", "fileName", FileType.Pdf, new byte[] {0xb2, 0xb3});
                var expected = new portaldocument
                {
                    title = source.Title,
                    nonsensitivetitle = source.NonsensitiveTitle,
                    description = source.Message,
                    href = source.FileName,
                    mime = source.MimeType
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
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
                                status = SignatureStatus.Signed.Identifier,
                                xadesurl = "http://xadesurl1.no"
                            },
                            new signature
                            {
                                personalidentificationnumber = "01013300002",
                                status = SignatureStatus.Waiting.Identifier,
                                xadesurl = "http://xadesurl2.no"
                            }
                        }
                    },
                    status = portalsignaturejobstatus.IN_PROGRESS
                };

                var jobStatus = source.status.ToJobStatus();
                var confirmationReference = new ConfirmationReference(new Uri(source.confirmationurl));
                var signature1 = source.signatures.signature[0];
                var signature2 = source.signatures.signature[1];
                var signatures = new List<Signature>
                {
                    new Signature
                    {
                        SignatureStatus = new SignatureStatus(signature1.status),
                        Signer = new PortalSigner(new PersonalIdentificationNumber(signature1.personalidentificationnumber), new NotificationsUsingLookup()),//TODO: Skal ikke bruke lookup
                        XadesReference = new XadesReference(new Uri(signature1.xadesurl))
                    },
                    new Signature
                    {
                        SignatureStatus = new SignatureStatus(signature2.status),
                        Signer = new PortalSigner(new PersonalIdentificationNumber(signature2.personalidentificationnumber), new NotificationsUsingLookup()), //TODO: Skal ikke bruke lookup
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