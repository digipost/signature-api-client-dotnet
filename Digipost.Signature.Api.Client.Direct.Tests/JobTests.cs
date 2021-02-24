using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class JobTests
    {
        public class ConstructorMethod : JobTests
        {
            [Fact]
            public void Constructor_without_sender_exists()
            {
                //Act
                new Job(null, null, null, null, null);

                //Assert
            }

            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var id = "IdDirectJob";
                var signers = DomainUtility.GetSigner();
                var documents = DomainUtility.GetSingleDirectDocument();
                var exitUrls = DomainUtility.GetExitUrls();
                var sender = CoreDomainUtility.GetSender();

                //Act
                var directJob = new Job("Job title", documents, signers, id, exitUrls, sender);

                //Assert
                Assert.Equal(id, directJob.Reference);
                Assert.Equal(signers, directJob.Signers);
                Assert.Equal(documents, directJob.Documents);
                Assert.Equal(exitUrls, directJob.ExitUrls);
                Assert.Equal(sender, directJob.Sender);
            }
        }
    }
}
