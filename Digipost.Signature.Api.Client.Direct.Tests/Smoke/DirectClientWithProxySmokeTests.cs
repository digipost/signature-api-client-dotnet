using System.Net;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Utilities;
using Digipost.Signature.Api.Client.Direct.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using static Digipost.Signature.Api.Client.Core.Tests.Utilities.CoreDomainUtility;
using static Digipost.Signature.Api.Client.Direct.Tests.Smoke.TestHelper;

namespace Digipost.Signature.Api.Client.Direct.Tests.Smoke
{
    public class DirectWithProxySmokeTestsFixture : SmokeTests
    {
        public readonly Sender Sender = new Sender(BringPublicOrganizationNumber);

        public DirectWithProxySmokeTestsFixture()
        {
            TestHelper = new TestHelper(DirectClient(Endpoint));
        }

        public TestHelper TestHelper { get; }

        private static DirectClient DirectClient(Environment environment)
        {
            var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();

            var clientConfig = new ClientConfiguration(environment, GetBringCertificate(), new Sender(BringPublicOrganizationNumber))
            {
                LogRequestAndResponse = true
            };
            
            WebProxy proxy = new WebProxy("http://127.0.0.1:8888");
            NetworkCredential credential = new NetworkCredential("user1","1234");
            var client = new DirectClient(clientConfig, serviceProvider.GetService<ILoggerFactory>(), proxy, credential);

            return client;
        }
    }

    public class DirectClientWithProxySmokeTests : IClassFixture<DirectWithProxySmokeTestsFixture>
    {
        public DirectClientWithProxySmokeTests(DirectWithProxySmokeTestsFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly DirectWithProxySmokeTestsFixture _fixture;
        
        [Fact]
        public void Can_create_job_with_one_signer()
        {
            var signer = new PersonalIdentificationNumber("01048200229");

            _fixture.TestHelper
                .Create_proxy_server()
                .Start_proxy_server()
                .Create_direct_job(signer)
                .Sign_job(signer)
                .Get_status()
                .Expect_job_to_have_status(
                    JobStatus.CompletedSuccessfully,
                    ExpectedSignerStatus(signer, SignatureStatus.Signed)
                )
                .Get_XAdES(signer)
                .Get_PAdES()
                .Confirm_status()
                .Assert_proxy_response()
                .Stop_proxy_server();
        }
    }
}
