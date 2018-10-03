using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Digipost.Signature.Api.Client.Portal.DataTransferObjects;
using Digipost.Signature.Api.Client.Portal.Extensions;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;
using Xunit;
using static Digipost.Signature.Api.Client.Core.Tests.Utilities.CoreDomainUtility;

namespace Digipost.Signature.Api.Client.Portal.Tests.DataTransferObjects
{
    public class DataTransferObjectConverterTests
    {
        public class ToDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [Fact]
            public void Converts_manifest_with_authentication_level_successfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";
                var source = new Manifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(),
                    new List<Signer>
                    {
                        new Signer(new PersonalIdentificationNumber("01043100358"), new NotificationsUsingLookup()),
                        new Signer(new PersonalIdentificationNumber("01043100319"), new NotificationsUsingLookup())
                    }) {AuthenticationLevel = AuthenticationLevel.Four};

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
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(0).Identifier).Value,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true,
                        },
                        new portalsigner
                        {
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(1).Identifier).Value,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true,
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
            public void Converts_manifest_with_availability_successfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";

                var source = new Manifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(),
                    new List<Signer>
                    {
                        new Signer(new PersonalIdentificationNumber("01043100358"), new NotificationsUsingLookup()),
                        new Signer(new PersonalIdentificationNumber("01043100319"), new NotificationsUsingLookup())
                    })
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
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(0).Identifier).Value,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true,
                        },
                        new portalsigner
                        {
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(1).Identifier).Value,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true,
                        }
                    },
                    availability = new availability
                    {
                        activationtime = source.Availability.Activation.Value,
                        availableseconds = source.Availability.AvailableSeconds.Value,
                        activationtimeSpecified = true,
                        availablesecondsSpecified = true
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
            public void Converts_manifest_with_only_activation_availability_successfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";

                var source = new Manifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(), new List<Signer>
                {
                    new Signer(new PersonalIdentificationNumber("01043100358"), new NotificationsUsingLookup()),
                    new Signer(new PersonalIdentificationNumber("01043100319"), new NotificationsUsingLookup())
                })
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
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(0).Identifier).Value,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true,
                        },
                        new portalsigner
                        {
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(1).Identifier).Value,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true,
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
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_manifest_with_only_available_seconds_successfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";

                var source = new Manifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(), new List<Signer>
                {
                    new Signer(new PersonalIdentificationNumber("01043100358"), new NotificationsUsingLookup()),
                    new Signer(new PersonalIdentificationNumber("01043100319"), new NotificationsUsingLookup())
                })
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
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(0).Identifier).Value,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true,
                        },
                        new portalsigner
                        {
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(1).Identifier).Value,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true,
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
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_manifest_with_ordered_signers_successfully()
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
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(0).Identifier).Value,
                            order = 1,
                            orderSpecified = true,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true
                        },
                        new portalsigner
                        {
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(1).Identifier).Value,
                            order = 2,
                            orderSpecified = true,
                            Item1 = new notificationsusinglookup {email = new enabled()},
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
            public void Converts_manifest_with_signature_type_successfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";
                var source = new Manifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(),
                    new List<Signer>
                    {
                        new Signer(new PersonalIdentificationNumber("01043100358"), new NotificationsUsingLookup())
                        {
                            SignatureType = SignatureType.AdvancedSignature
                        },
                        new Signer(new PersonalIdentificationNumber("01043100319"), new NotificationsUsingLookup())
                        {
                            SignatureType = SignatureType.AuthenticatedSignature
                        }
                    });

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
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(0).Identifier).Value,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            signaturetype = signaturetype.ADVANCED_ELECTRONIC_SIGNATURE,
                            signaturetypeSpecified = true,
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true
                        },
                        new portalsigner
                        {
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(1).Identifier).Value,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            signaturetype = signaturetype.AUTHENTICATED_ELECTRONIC_SIGNATURE,
                            signaturetypeSpecified = true,
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true,
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
            public void Converts_manifest_without_availability_successfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";
                var source = new Manifest(new Sender(organizationNumberSender), DomainUtility.GetPortalDocument(),
                    new List<Signer>
                    {
                        new Signer(new PersonalIdentificationNumber("01043100358"), new NotificationsUsingLookup()),
                        new Signer(new PersonalIdentificationNumber("01043100319"), new NotificationsUsingLookup())
                    });

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
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(0).Identifier).Value,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true,
                        },
                        new portalsigner
                        {
                            Item = ((PersonalIdentificationNumber) source.Signers.ElementAt(1).Identifier).Value,
                            Item1 = new notificationsusinglookup {email = new enabled()},
                            onbehalfof = signingonbehalfof.SELF,
                            onbehalfofSpecified = true,
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
            public void Converts_notifications_using_lookup_successfully()
            {
                //Arrange
                var source = new NotificationsUsingLookup {SmsIfAvailable = true};
                var expected = new notificationsusinglookup {email = new enabled(), sms = new enabled()};

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_notifications_with_email_successfully()
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
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_notifications_with_sms_and_email_successfully()
            {
                //Arrange
                var sms = new Sms("99999999");
                var email = new Email("test@mail.no");

                var source = new Notifications(sms, email);
                var expected = new notifications
                {
                    Items = new object[]
                    {
                        new email {address = email.Address},
                        new sms {number = sms.Number}
                    }
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_notifications_with_sms_successfully()
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
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_portal_document_successfully()
            {
                //Arrange
                var source = new Document("subject", "message", FileType.Pdf, new byte[] {0xb2, 0xb3}) {NonsensitiveTitle = "NonsensitiveTitle"};
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
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_portal_job_successfully()
            {
                //Arrange
                var document = DomainUtility.GetPortalDocument();
                var signers = DomainUtility.GetSigners(2);
                var reference = "reference";
                var source = new Job(document, signers, reference, new Sender(BringPublicOrganizationNumber));

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
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_portal_job_with_polling_queue_successfully()
            {
                //Arrange
                var document = DomainUtility.GetPortalDocument();
                var signers = DomainUtility.GetSigners(2);
                var reference = "reference";
                var custompollingqueue = "CustomPollingQueue";
                var sender = new Sender(BringPublicOrganizationNumber, new PollingQueue(custompollingqueue));
                var source = new Job(document, signers, reference, sender);

                var expected = new portalsignaturejobrequest
                {
                    reference = reference,
                    pollingqueue = custompollingqueue
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
            public void Converts_signer_identified_by_email()
            {
                //Arrange
                var source = new Signer(
                    new ContactInformation {Email = new Email("email@example.com")}
                );

                var expected = new portalsigner
                {
                    Item = new enabled(),
                    Item1 = new notifications
                    {
                        Items = new object[]
                        {
                            new email {address = "email@example.com"}
                        }
                    },
                    onbehalfofSpecified = true,
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_on_behalf_of()
            {
                //Arrange
                var source = new Signer(
                        new ContactInformation {Email = new Email("email@example.com")}
                    )
                    {OnBehalfOf = OnBehalfOf.Other};

                var expected = new portalsigner
                {
                    Item = new enabled(),
                    Item1 = new notifications
                    {
                        Items = new object[]
                        {
                            new email {address = "email@example.com"}
                        }
                    },
                    onbehalfofSpecified = true,
                    onbehalfof = signingonbehalfof.OTHER
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_signer_identified_by_email_and_sms()
            {
                //Arrange
                var source = new Signer(
                    new ContactInformation
                    {
                        Email = new Email("email@example.com"),
                        Sms = new Sms("11111111")
                    }
                );

                var expected = new portalsigner
                {
                    Item = new enabled(),
                    Item1 = new notifications
                    {
                        Items = new object[]
                        {
                            new email {address = "email@example.com"},
                            new sms {number = "11111111"},
                        }
                    },
                    onbehalfof = signingonbehalfof.SELF,
                    onbehalfofSpecified = true,
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_signer_identified_by_sms()
            {
                //Arrange
                var source = new Signer(
                    new ContactInformation {Sms = new Sms("11111111")}
                );

                var expected = new portalsigner
                {
                    Item = new enabled(),
                    Item1 = new notifications
                    {
                        Items = new object[]
                        {
                            new sms {number = "11111111"}
                        }
                    },
                    onbehalfof = signingonbehalfof.SELF,
                    onbehalfofSpecified = true,
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_signer_with_contact_information_identifier()
            {
                //Arrange
                var source = new Signer(new ContactInformation {Email = new Email("email@example.com")});

                var expected = new portalsigner
                {
                    Item = new enabled(),
                    Item1 = new notifications
                    {
                        Items = new object[] {new email {address = ((ContactInformation) source.Identifier).Email.Address}}
                    },
                    onbehalfof = signingonbehalfof.SELF,
                    onbehalfofSpecified = true,
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_signer_with_notifications_using_lookup_successfully()
            {
                //Arrange
                var source = new Signer(
                    new PersonalIdentificationNumber("11111111111"),
                    new NotificationsUsingLookup()
                );
                var expected = new portalsigner
                {
                    Item = ((PersonalIdentificationNumber) source.Identifier).Value,
                    Item1 = new notificationsusinglookup {email = new enabled()},
                    onbehalfof = signingonbehalfof.SELF,
                    onbehalfofSpecified = true,
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_signer_with_pin_and_notifications_successfully()
            {
                //Arrange
                var source = new Signer(
                    new PersonalIdentificationNumber("11111111111"),
                    new Notifications(new Email("email@example.com"))
                );
                var expected = new portalsigner
                {
                    Item = ((PersonalIdentificationNumber) source.Identifier).Value,
                    Item1 = new notifications
                    {
                        Items = new object[]
                        {
                            new email {address = source.Notifications.Email.Address}
                        }
                    },
                    onbehalfof = signingonbehalfof.SELF,
                    onbehalfofSpecified = true,
                };

                //Act
                var actual = DataTransferObjectConverter.ToDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.Empty(differences);
            }
        }

        public class FromDataTransferObjectMethod : DataTransferObjectConverterTests
        {
            [Fact]
            public void Converts_portal_job_response_successfully()
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
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_portal_job_status_change_response_successfully()
            {
                //Todo: Husk å sjekke om vi skal ha type, ettersom man kan få et fødselsnummer eller en identifier tilbake. Nå er det bare en string

                //Arrange
                var now = DateTime.Now;

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
                                Item = "01013300001",
                                status = new signaturestatus
                                {
                                    Value = SignatureStatus.Signed.Identifier,
                                    since = now
                                },
                                xadesurl = "http://xadesurl1.no"
                            },
                            new signature
                            {
                                Item = "01013300002",
                                status = new signaturestatus
                                {
                                    Value = SignatureStatus.Waiting.Identifier,
                                    since = now
                                },
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
                        SignatureStatus = new SignatureStatus(signature1.status.Value),
                        Identifier = new PersonalIdentificationNumber((string) signature1.Item),
                        XadesReference = new XadesReference(new Uri(signature1.xadesurl)),
                        DateTimeForStatus = now
                    },
                    new Signature
                    {
                        SignatureStatus = new SignatureStatus(signature2.status.Value),
                        Identifier = new PersonalIdentificationNumber((string) signature2.Item),
                        XadesReference = new XadesReference(new Uri(signature2.xadesurl)),
                        DateTimeForStatus = now
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
                    expected.PadesReference = new PadesReference(new Uri(padesUrl));

                //Act
                var actual = DataTransferObjectConverter.FromDataTransferObject(source);

                //Assert
                var comparator = new Comparator();
                IEnumerable<IDifference> differences;
                comparator.AreEqual(expected, actual, out differences);
                Assert.Empty(differences);
            }
        }
    }
}