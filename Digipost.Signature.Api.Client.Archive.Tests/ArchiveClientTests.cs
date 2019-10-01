using Xunit;
using static Digipost.Signature.Api.Client.Core.Tests.Utilities.CoreDomainUtility;


namespace Digipost.Signature.Api.Client.Archive.Tests
{
    public class ArchiveClientTests
    {
        public class ConstructorMethod : ArchiveClientTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var clientConfiguration = GetClientConfiguration();

                //Act
                var client = new ArchiveClient(clientConfiguration);

                //Assert
                Assert.Equal(clientConfiguration, client.ClientConfiguration);
                Assert.NotNull(client.HttpClient);
            }

        }
    }
}
