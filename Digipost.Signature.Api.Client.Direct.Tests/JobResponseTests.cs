using System;
using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Schemas;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class JobResponseTests
    {
        public class ConstructorMethod : JobResponseTests
        {
            [Fact]
            public void Converts_direct_job_successfully()
            {
                //Arrange
                const string pin = "12345678910";
                const string redirectUrl = "https://api.signering.posten.no/redirect/#/c316e5b62df86a5d80e517b3ff4532738a9e7e43d4ae6075d427b1b58355bc63";
                const string signerUrl = "https://api.signering.posten.no/api/123456789/direct/signature-jobs/1/signers/1";
                const string statusUrl = "https://api.signering.posten.no/api/signature-jobs/1/status";
                var source = new directsignaturejobresponse
                {
                    signaturejobid = 1444,
                    reference = "senders-reference",
                    redirecturl = new[] {new signerspecificurl {signer = pin, Value = redirectUrl}},
                    statusurl = statusUrl,
                    signer = new[]
                    {
                        new directsignerresponse()
                        {
                            href = signerUrl,
                            ItemElementName = ItemChoiceType1.personalidentificationnumber,
                            redirecturl = redirectUrl,
                            Item = pin,
                        },
                    }
                };

                var expected = new JobResponse(
                    source.signaturejobid,
                    source.reference,
                    new List<SignerResponse>()
                    {
                        new SignerResponse(new PersonalIdentificationNumber(pin), new Uri(redirectUrl), new Uri(signerUrl))
                    },
                    new StatusUrl(source.statusurl)
                );

                //Act
                var actual = new JobResponse(source);

                //Assert
                var comparator = new Comparator();
                comparator.AreEqual(expected.Signers, actual.Signers, out var differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_direct_job_with_multiple_signers_successfully()
            {
                //Arrange
                const string pin = "12345678910";
                const string signerUrl = "https://api.signering.posten.no/api/123456789/direct/signature-jobs/1/signers/1";
                const string redirectUrl = "https://api.signering.posten.no/redirect/#/c316e5b62df86a5d80e517b3ff4532738a9e7e43d4ae6075d427b1b58355bc63";

                const string pin2 = "10987654321";
                const string signerUrl2 = "https://api.signering.posten.no/api/123456789/direct/signature-jobs/1/signers/2";
                const string redirectUrl2 = "https://api.signering.posten.no/redirect/#/sdflkj34kl2l34kjl234jkh2jhgj234gk5jln2n34gkj234hhjjlg234l34g234";

                var source = new directsignaturejobresponse
                {
                    signaturejobid = 1444,
                    reference = "senders-reference",
                    redirecturl = new[]
                    {
                        new signerspecificurl {signer = pin, Value = redirectUrl},
                        new signerspecificurl {signer = pin2, Value = redirectUrl2}
                    },
                    statusurl = "https://api.signering.posten.no/api/signature-jobs/1/status",
                    signer = new[]
                    {
                        new directsignerresponse()
                        {
                            href = signerUrl,
                            ItemElementName = ItemChoiceType1.personalidentificationnumber,
                            redirecturl = redirectUrl,
                            Item = pin,
                        },
                        new directsignerresponse()
                        {
                            href = signerUrl2,
                            ItemElementName = ItemChoiceType1.personalidentificationnumber,
                            redirecturl = redirectUrl2,
                            Item = pin2,
                        }
                    }
                };

                var expected = new JobResponse(
                    source.signaturejobid,
                    source.reference,
                    new List<SignerResponse>()
                    {
                        new SignerResponse(new PersonalIdentificationNumber(pin), new Uri(redirectUrl), new Uri(signerUrl)),
                        new SignerResponse(new PersonalIdentificationNumber(pin2), new Uri(redirectUrl2), new Uri(signerUrl2)),
                    },
                    new StatusUrl(source.statusurl)
                );

                //Act
                var result = new JobResponse(source);

                //Assert
                var comparator = new Comparator();
                comparator.AreEqual(expected, result, out var differences);
                Assert.Empty(differences);
            }
        }
    }
}
