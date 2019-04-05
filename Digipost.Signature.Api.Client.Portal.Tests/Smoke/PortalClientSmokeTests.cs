using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using static Digipost.Signature.Api.Client.Core.Tests.Utilities.CoreDomainUtility;
using static Digipost.Signature.Api.Client.Portal.Enums.JobStatus;

namespace Digipost.Signature.Api.Client.Portal.Tests.Smoke
{
    public class PortalSmokeTestsFixture : SmokeTests
    {
        public PortalSmokeTestsFixture()
        {
            TestHelper = new TestHelper(GetPortalClient());
        }

        public TestHelper TestHelper { get; }

        private static PortalClient GetPortalClient()
        {
            var client = GetPortalClient(Endpoint);
            client.ClientConfiguration.LogRequestAndResponse = true;

            return client;
        }

        private static PortalClient GetPortalClient(Environment environment)
        {
            var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();
            var sender = new Sender(BringPublicOrganizationNumber);
            var clientConfig = new ClientConfiguration(environment, GetBringCertificate(), sender) {HttpClientTimeoutInMilliseconds = 30000, LogRequestAndResponse = true};
            var client = new PortalClient(clientConfig, serviceProvider.GetService<ILoggerFactory>());
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
        public void Verify_tls_setup()
        {
            _fixture.TestHelper
                .Verify_tls_setup(new Sender(BringPublicOrganizationNumber));
        }

        [Fact]
        public void Can_create_job_and_cancel()
        {
            var signer = new Signer(new PersonalIdentificationNumber("01048200229"), new Notifications(new Email("email@example.com"))) {OnBehalfOf = OnBehalfOf.Other};

            _fixture.TestHelper
                .Create_job(signer)
                .Cancel_job()
                .ExpectJobStatusForSenderIs(Failed)
                .Confirm_job();
        }

        [Fact]
        public void Can_create_job_and_confirm()
        {
            var signer = new Signer(new PersonalIdentificationNumber("12345678910"), new Notifications(new Email("email@example.com"))) {OnBehalfOf = OnBehalfOf.Other};

            _fixture.TestHelper
                .Create_job(signer)
                .Sign_job()
                .ExpectJobStatusForSenderIs(CompletedSuccessfully)
                .GetSignatureForSigner()
                .GetXades()
                .GetPades()
                .Confirm_job();
        }

        [Fact]
        public void Can_create_open_portal_job()
        {
            var signer = new Signer(new ContactInformation {Email = new Email("email@example.com"), Sms = new Sms("11111111")});

            _fixture.TestHelper
                .Create_job(new Sender(BringPrivateOrganizationNumber), signer);
        }

        [Fact]
        public void create_job_with_queue_and_verify_excessive_polling_is_queue_dependent()
        {
            var signer = new Signer(new PersonalIdentificationNumber("12345678910"), new Notifications(new Email("email@example.com"))) {OnBehalfOf = OnBehalfOf.Other};
            var senderWithQueue = new Sender(BringPrivateOrganizationNumber, new PollingQueue("CustomPortalPollingQueue"));
            var senderWithoutQueue = new Sender(BringPrivateOrganizationNumber);

            _fixture.TestHelper
                .Create_job(senderWithQueue, signer)
                .Sign_job()
                .ExpectJobStatusForSenderIs(NoChanges, senderWithoutQueue)
                .ExpectJobStatusForSenderIs(CompletedSuccessfully, senderWithQueue)
                .ExpectJobStatusForSenderIs(NoChanges, senderWithQueue);
        }
    }
}
