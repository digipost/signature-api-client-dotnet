using System;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.Internal.AsicE;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests.Internal.AsicE
{
    public class PortalAsiceGeneratorTests
    {
        public class ToManifestMethod : PortalAsiceGeneratorTests
        {
            [Fact]
            public void Creates_manifest_with_expected_attributes()
            {
                // Arrange
                var job = new Job(
                    "Job title",
                    DomainUtility.GetSinglePortalDocument(),
                    DomainUtility.GetSigners(2),
                    "Reference",
                    CoreDomainUtility.GetSender())
                {
                    Description = "Job description",
                    NonSensitiveTitle = "Non-sensitive title",
                    Availability = new Availability
                    {
                        Activation = new DateTime(2025, 6, 6),
                        AvailableFor = TimeSpan.FromDays(7)
                    },
                    AuthenticationLevel = Core.Enums.AuthenticationLevel.Four,
                    IdentifierInSignedDocuments = Core.Enums.IdentifierInSignedDocuments.PersonalIdentificationNumberAndName
                };

                // Act
                var manifest = PortalAsiceGenerator.ToManifest(job);

                // Assert
                Assert.Equal(job.Title, manifest.Title);
                Assert.Equal(job.Documents, manifest.Documents);
                Assert.Equal(job.Signers, manifest.Signers);
                Assert.Equal(job.Sender, manifest.Sender);
                Assert.Equal(job.Description, manifest.Description);
                Assert.Equal(job.NonSensitiveTitle, manifest.NonSensitiveTitle);
                Assert.Equal(job.Availability, manifest.Availability);
                Assert.Equal(job.AuthenticationLevel, manifest.AuthenticationLevel);
                Assert.Equal(job.IdentifierInSignedDocuments, manifest.IdentifierInSignedDocuments);
            }
        }
    }

}
