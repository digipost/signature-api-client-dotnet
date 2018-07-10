using Digipost.Signature.Api.Client.Portal.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class JobTests
    {
        public class ConstructorMethod : JobTests
        {
            [Fact]
            public void Constructor_without_sender_exists()
            {
                //Arrange

                //Act
                new Job(null, null, null);

                //Assert
            }

            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var document = DomainUtility.GetPortalDocument();
                var signers = DomainUtility.GetSigners(3);
                var reference = "PortalJobReference";
                var portalJob = new Job(document, signers, reference);

                //Act

                //Assert
                Assert.Equal(document, portalJob.Document);
                Assert.Equal(signers, portalJob.Signers);
                Assert.Equal(reference, portalJob.Reference);
            }
        }
    }
}