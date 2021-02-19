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
                new Job(null, null, null, null);

                //Assert
            }

            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var documents = DomainUtility.GetPortalDocuments();
                var signers = DomainUtility.GetSigners(3);
                var reference = "PortalJobReference";
                var title = "Title";
                var portalJob = new Job(title, documents, signers, reference);

                //Act

                //Assert
                Assert.Equal(title, portalJob.Title);
                Assert.Equal(documents, portalJob.Documents);
                Assert.Equal(signers, portalJob.Signers);
                Assert.Equal(reference, portalJob.Reference);
            }
        }
    }
}
