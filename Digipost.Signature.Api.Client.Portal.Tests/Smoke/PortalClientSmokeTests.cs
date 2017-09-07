using System.Reflection;
using Common.Logging;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Xunit;
using static Digipost.Signature.Api.Client.Core.Tests.Utilities.CoreDomainUtility;

namespace Digipost.Signature.Api.Client.Portal.Tests.Smoke
{
    public class PortalSmokeTestsFixture : SmokeTests
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public PortalSmokeTestsFixture()
        {
            TestHelper = new TestHelper(GetPortalClient());
        }

        public TestHelper TestHelper { get; set; }

        private static PortalClient GetPortalClient()
        {
            var client = GetPortalClient(Endpoint);
            client.ClientConfiguration.LogRequestAndResponse = true;

            return client;
        }

        private static PortalClient GetPortalClient(Environment environment)
        {
            var sender = new Sender(BringPublicOrganizationNumber);
            var clientConfig = new ClientConfiguration(environment, GetBringCertificate(), sender) {HttpClientTimeoutInMilliseconds = 30000};
            var client = new PortalClient(clientConfig);
            return client;
        }
    }

    public class PortalClientSmokeTests : IClassFixture<PortalSmokeTestsFixture>
    {
        public PortalClientSmokeTests(PortalSmokeTestsFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly PortalSmokeTestsFixture _fixture;

        [Fact]
        public void Can_create_job_and_cancel()
        {
            var signer = new Signer(new PersonalIdentificationNumber("12345678910"), new Notifications(new Email("email@example.com")));

            _fixture.TestHelper
                .Create_job(signer)
                .Cancel_job()
                .GetJobStatusChanged()
                .Confirm_job();
        }

        [Fact]
        public void Can_create_job_and_confirm()
        {
            var signer = new Signer(new PersonalIdentificationNumber("12345678910"), new Notifications(new Email("email@example.com")));

            _fixture.TestHelper
                .Create_job(signer)
                .Sign_job()
                .GetJobStatusChanged()
                .GetSignatureForSigner()
                .GetXades()
                .GetPades()
                .Confirm_job();
        }

        [Fact(Skip = "Fordi vi skal være kule og shippe en betaversjon av klienten")]
        public void Can_create_open_portal_job()
        {
            var signer = new Signer(new ContactInformation {Email = new Email("email@example.com"), Sms = new Sms("11111111")});

            _fixture.TestHelper
                .Create_job(new Sender(BringPrivateOrganizationNumber), signer);
        }
    }
}