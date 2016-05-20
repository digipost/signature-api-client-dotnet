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
                var signers = DomainUtility.GetSigners(2);
                var reference = "reference";
                var source = new Job(document, signers, reference);

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
                var source = new Manifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(), DomainUtility.GetSigners(2));

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new portaldocument
                    {
                        title = source.Document.Title,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new portalsigner
                        {
                            personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber.Value,
                            Item = new notificationsusinglookup() {email = new enabled()}
                        },
                        new portalsigner
                        {
                            personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber.Value,
                            Item = new notificationsusinglookup() {email = new enabled()}
                        }
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

                var source = new Manifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(), new List<Signer> {new Signer(new PersonalIdentificationNumber("00000000000"), new NotificationsUsingLookup()) {Order = 1}, new Signer(new PersonalIdentificationNumber("99999999999"), new NotificationsUsingLookup()) {Order = 2}}); //TODO: Skal ikke bruke lookup

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new portaldocument
                    {
                        title = source.Document.Title,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new portalsigner
                        {
                            personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber.Value, order = 1,
                            Item = new notificationsusinglookup() {email = new enabled()}
                        },
                        new portalsigner
                        {
                            personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber.Value, order = 2,
                            Item = new notificationsusinglookup() {email = new enabled()}
                        }
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

                var source = new Manifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(), DomainUtility.GetSigners(2))
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
                        title = source.Document.Title,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new portalsigner
                        {
                            personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber.Value,
                            Item = new notificationsusinglookup() {email = new enabled()}
                        },
                        new portalsigner
                        {
                            personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber.Value,
                            Item = new notificationsusinglookup() {email = new enabled()}
                        }
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

                var source = new Manifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(), DomainUtility.GetSigners(2))
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
                        title = source.Document.Title,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new portalsigner
                        {
                            personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber.Value, 
                            Item = new notificationsusinglookup() {email = new enabled()}
                        },
                        new portalsigner
                        {
                            personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber.Value,
                            Item = new notificationsusinglookup() {email = new enabled()}
                        }
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

                var source = new Manifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(), DomainUtility.GetSigners(2))
                {
                    Availability = DomainUtility.GetAvailability()
                };

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender {organizationnumber = organizationNumberSender},
                    document = new portaldocument
                    {
                        title = source.Document.Title,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new portalsigner
                        {
                            personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber.Value,
                            Item = new notificationsusinglookup() {email = new enabled()}
                        },
                        new portalsigner
                        {
                            personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber.Value,
                            Item = new notificationsusinglookup() {email = new enabled()}
                        }
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

            [TestMethod]
            public void ConvertsPortalDocumentSuccessfully()
            {
                //Arrange
                var source = new Document("subject", "message", "fileName", FileType.Pdf, new byte[] {0xb2, 0xb3}) {NonsensitiveTitle = "NonsensitiveTitle"};
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
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsNotificationsWithEmailSuccessfully()
            {
                //Arrange
                var email = new Email("test@mail.no");

                var source = new Notifications(email);
                var expected = new notifications
                {
                    Items = new object[] {new email {address = email.Address}}
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsNotificationsWithSmsSuccessfully()
            {
                //Arrange
                var sms = new Sms("99999999");

                var source = new Notifications(sms);
                var expected = new notifications
                {
                    Items = new object[] {new sms {number = sms.Number}}
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsNotificationsWithSmsAndEmailSuccessfully()
            {
                //Arrange
                var sms = new Sms("99999999");
                var email = new Email("test@mail.no");

                var source = new Notifications(sms, email);
                var expected = new notifications
                {
                    Items = new object[]
                    {
                        new sms {number = sms.Number},
                        new email {address = email.Address}
                    }
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsNotificationsUsingLookupSuccessfully()
            {
                //Arrange
                var source = new NotificationsUsingLookup() { SmsIfAvailable = true };
                var expected = new notificationsusinglookup() { email = new enabled(), sms = new enabled()};

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsSignerWithNotificationsSuccessfully()
            {
                //Arrange
                var source = new Signer(
                    new PersonalIdentificationNumber("11111111111"),
                    new Notifications(new Email("test@mail.no"))
                    );
                var expected = new portalsigner()
                {
                    personalidentificationnumber = source.PersonalIdentificationNumber.Value,
                    Item = new notifications()
                    {
                        Items = new object[]
                        {
                            new email() { address = source.Notifications.Email.Address }
                        }
                    }
                
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.AreEqual(0, differences.Count());
            }

            [TestMethod]
            public void ConvertsSignerWithNotificationsUsingLookupSuccessfully()
            {
                //Arrange
                var source = new Signer(
                    new PersonalIdentificationNumber("11111111111"),
                    new NotificationsUsingLookup()
                    );
                var expected = new portalsigner()
                {
                    personalidentificationnumber = source.PersonalIdentificationNumber.Value,
                    Item = new notificationsusinglookup() { email = new enabled()}
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

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
                var expected = new JobResponse(signaturejobid, new Uri(httpCancellationurl));

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
                        Signer = new PersonalIdentificationNumber(signature1.personalidentificationnumber),
                        XadesReference = new XadesReference(new Uri(signature1.xadesurl))
                    },
                    new Signature
                    {
                        SignatureStatus = new SignatureStatus(signature2.status),
                        Signer = new PersonalIdentificationNumber(signature2.personalidentificationnumber),
                        XadesReference = new XadesReference(new Uri(signature2.xadesurl))
                    }
                };

                var expected = new JobStatusChanged(
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