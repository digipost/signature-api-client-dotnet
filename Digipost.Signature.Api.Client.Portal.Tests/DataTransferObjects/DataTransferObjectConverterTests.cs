using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Digipost.Signature.Api.Client.DataTransferObjects.XsdToCode.Code;
using Digipost.Signature.Api.Client.Portal.DataTransferObjects;
using Digipost.Signature.Api.Client.Portal.Enums;
using Digipost.Signature.Api.Client.Portal.Extensions;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;
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
                var document = DomainUtility.GetDocument();
                var signers = DomainUtility.GetSigners(2);
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

                var source = new PortalManifest(new Sender(organizationNumberSender), DomainUtility.GetDocument(), DomainUtility.GetSigners(2));
                
                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender() { organizationnumber = organizationNumberSender },
                    document = new document
                    {
                        title = source.Document.Subject,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new signer { personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber},
                        new signer { personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber},
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
            public void ConvertsManifestWithOnlyExpirationAvailabilitySuccessfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";

                var source = new PortalManifest(new Sender(organizationNumberSender), DomainUtility.GetDocument(), DomainUtility.GetSigners(2))
                {
                    Availability = new Availability()
                    {
                        Expiration = DateTime.Now.AddHours(22)
                    }
                };

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender { organizationnumber = organizationNumberSender },
                    document = new document
                    {
                        title = source.Document.Subject,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new signer { personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber},
                        new signer { personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber},
                    },
                    availability = new availability()
                    {
                        expirationtime = source.Availability.Expiration.Value
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
            public void ConvertsManifestWithOnlyActiviationAvailabilitySuccessfully()
            {
                //Arrange
                const string organizationNumberSender = "12345678902";

                var source = new PortalManifest(new Sender(organizationNumberSender), DomainUtility.GetDocument(), DomainUtility.GetSigners(2))
                {
                    Availability = new Availability()
                    {
                        Activation = DateTime.Now.AddHours(22)
                    }
                };

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender { organizationnumber = organizationNumberSender },
                    document = new document
                    {
                        title = source.Document.Subject,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new[]
                    {
                        new signer { personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber},
                        new signer { personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber},
                    },
                    availability = new availability()
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
                
                var source = new PortalManifest(new Sender(organizationNumberSender), DomainUtility.GetDocument(), DomainUtility.GetSigners(2))
                {
                    Availability = DomainUtility.GetAvailability()
                };

                var expected = new portalsignaturejobmanifest
                {
                    sender = new sender { organizationnumber = organizationNumberSender },
                    document = new document
                    {
                        title = source.Document.Subject,
                        description = source.Document.Message,
                        href = source.Document.FileName,
                        mime = source.Document.MimeType
                    },
                    signers = new []
                    {
                        new signer { personalidentificationnumber = source.Signers.ElementAt(0).PersonalIdentificationNumber}, 
                        new signer { personalidentificationnumber = source.Signers.ElementAt(1).PersonalIdentificationNumber},
                    },
                    availability = new availability()
                    {
                        activationtime = source.Availability.Activation.Value,
                        expirationtime = source.Availability.Expiration.Value
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
                var source = new portalsignaturejobresponse() { signaturejobid = signaturejobid };
                var expected = new PortalJobResponse(signaturejobid);

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
                var source = new portalsignaturejobstatuschangeresponse()
                {
                    confirmationurl = "http://confirmationurl.no",
                    signaturejobid = 12345678901011,
                    signatures = new signatures()
                    {
                        padesurl = "http://padesurl.no",
                        signature = new[]{ new signature()
                        {
                            personalidentificationnumber = "01013300001",
                            status = signaturestatus.SIGNED,
                            xadesurl = "http://xadesurl1.no"
                        }, new signature()
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
                var signatures = new List<Signature>()
                {
                    new Signature
                    {
                        SignatureStatus = (SignatureStatus)Enum.Parse(typeof(SignatureStatus), signature1.status.ToString(), ignoreCase: true),
                        Signer = new Signer(signature1.personalidentificationnumber),
                        XadesReference = new XadesReference(new Uri(signature1.xadesurl))
                    },
                    new Signature
                    {
                        SignatureStatus = (SignatureStatus)Enum.Parse(typeof(SignatureStatus), signature2.status.ToString(), ignoreCase: true),
                        Signer = new Signer(signature2.personalidentificationnumber),
                        XadesReference = new XadesReference(new Uri(signature2.xadesurl))
                    }
                };

                var expected = new PortalJobStatusChangeResponse(
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