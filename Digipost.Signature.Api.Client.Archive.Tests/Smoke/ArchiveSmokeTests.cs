using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using static Digipost.Signature.Api.Client.Core.Tests.Utilities.CoreDomainUtility;


namespace Digipost.Signature.Api.Client.Archive.Tests.Smoke
{
    public class ArchiveSmokeTestsFixture : SmokeTests
    {
        public  ArchiveSmokeTestsFixture()
        {
            TestHelper = new TestHelper(ArchiveClient(Endpoint));
        }
        public TestHelper TestHelper { get; }
        private static ArchiveClient ArchiveClient(Environment environment)
        {
            var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();

            var clientConfig = new ClientConfiguration(environment, GetBringCertificate(), new Sender(BringPublicOrganizationNumber))
            {
                LogRequestAndResponse = true
            };
            var client = new ArchiveClient(clientConfig, serviceProvider.GetService<ILoggerFactory>());

            return client;
        }
    }

    public class ArchiveClientSmokeTests : IClassFixture<ArchiveSmokeTestsFixture>
    {
        public ArchiveClientSmokeTests(ArchiveSmokeTestsFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly ArchiveSmokeTestsFixture _fixture;

        [Fact]
        public void download_pades_and_expect_error()
        {
            _fixture.TestHelper
                .Download_pades_and_expect_client_error(GetClientConfiguration().GlobalSender.OrganizationNumber,"1234");
        }
    }
    
}
