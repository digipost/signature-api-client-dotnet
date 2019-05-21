using System;
using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Tests.Utilities.CompareObjects;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
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
                const string redirectUrl = "https://localhost:9000/redirect/#/c316e5b62df86a5d80e517b3ff4532738a9e7e43d4ae6075d427b1b58355bc63";
                var source = new directsignaturejobresponse
                {
                    signaturejobid = 1444,
                    reference = "senders-reference",
                    redirecturl = new[] {new signerspecificurl {signer = "12345678910", Value = redirectUrl}},
                    statusurl = "https://localhost:8443/api/signature-jobs/77/status"
                };

                var expected = new JobResponse(
                    source.signaturejobid,
                    source.reference,
                    new ResponseUrls(
                        new List<RedirectReference> {new RedirectReference(new Uri(redirectUrl), new PersonalIdentificationNumber("12345678910"))},
                        new Uri(source.statusurl)
                    )
                );

                //Act
                var result = new JobResponse(source);

                //Assert
                var comparator = new Comparator();
                comparator.AreEqual(expected, result, out var differences);
                Assert.Empty(differences);
            }

            [Fact]
            public void Converts_direct_job_with_multiple_signers_successfully()
            {
                //Arrange
                const string redirectUrl = "https://localhost:9000/redirect/#/some-reference";
                const string redirectUrl2 = "https://localhost:9000/redirect/#/some-other-reference";
                var source = new directsignaturejobresponse
                {
                    signaturejobid = 1444,
                    reference = "senders-reference",
                    redirecturl = new[]
                    {
                        new signerspecificurl {signer = "12345678910", Value = redirectUrl},
                        new signerspecificurl {signer = "10987654321", Value = redirectUrl2}
                    },
                    statusurl = "https://localhost:8443/api/signature-jobs/77/status"
                };

                var expected = new JobResponse(
                    source.signaturejobid,
                    source.reference,
                    new ResponseUrls(
                        new List<RedirectReference>
                        {
                            new RedirectReference(new Uri(redirectUrl), new PersonalIdentificationNumber("12345678910")),
                            new RedirectReference(new Uri(redirectUrl2), new PersonalIdentificationNumber("10987654321"))
                        },
                        new Uri(source.statusurl)
                    )
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
