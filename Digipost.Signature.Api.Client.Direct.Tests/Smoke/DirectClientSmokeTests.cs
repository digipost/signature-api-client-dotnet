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
    public class DirectSmokeTestsFixture : SmokeTests
    {
        public readonly Sender Sender = new Sender(BringPublicOrganizationNumber);

        public DirectSmokeTestsFixture()
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
            var client = new DirectClient(clientConfig, serviceProvider.GetService<ILoggerFactory>());

            return client;
        }
    }

    public class DirectClientSmokeTests : IClassFixture<DirectSmokeTestsFixture>
    {
        public DirectClientSmokeTests(DirectSmokeTestsFixture fixture)
        {
            _fixture = fixture;
        }

        private readonly DirectSmokeTestsFixture _fixture;

        [Fact]
        public void Can_create_job_with_multiple_signers()
        {
            var signer1 = new PersonalIdentificationNumber("12345678910");
            var signer2 = new PersonalIdentificationNumber("10987654321");

            _fixture.TestHelper
                .Create_direct_job(signer1, signer2);
            _fixture.TestHelper
                .Sign_job(signer1)
                .Get_status()
                .Expect_job_to_have_status(
                    JobStatus.InProgress,
                    ExpectedSignerStatus(signer1, SignatureStatus.Signed),
                    ExpectedSignerStatus(signer2, SignatureStatus.Waiting)
                )
                .Get_XAdES(signer1)
                .Sign_job(signer2)
                .Get_status()
                .Expect_job_to_have_status(
                    JobStatus.CompletedSuccessfully,
                    ExpectedSignerStatus(signer1, SignatureStatus.Signed),
                    ExpectedSignerStatus(signer2, SignatureStatus.Signed)
                )
                .Get_XAdES(signer2)
                .Confirm_status();
        }

        [Fact]
        public void Can_create_job_with_one_signer()
        {
            var signer = new PersonalIdentificationNumber("01048200229");

            _fixture.TestHelper
                .Create_direct_job(signer)
                .Sign_job(signer)
                .Get_status()
                .Expect_job_to_have_status(
                    JobStatus.CompletedSuccessfully,
                    ExpectedSignerStatus(signer, SignatureStatus.Signed)
                )
                .Get_XAdES(signer)
                .Get_PAdES()
                .Confirm_status();
        }

        [Fact]
        public void Can_request_new_redirect_url()
        {
            var signer = new PersonalIdentificationNumber("01048200229");

            _fixture.TestHelper
                .Create_direct_job(signer)
                .Request_new_redirect_url(signer)
                .Sign_job(signer);
        }

        [Fact]
        public void Can_retrieve_status_by_polling()
        {
            var signer = new PersonalIdentificationNumber("12345678910");

            _fixture.TestHelper
                .Create_pollable_direct_job(_fixture.Sender, signer)
                .Sign_job(signer)
                .Get_status_by_polling(_fixture.Sender)
                .Expect_job_to_have_status(
                    JobStatus.CompletedSuccessfully,
                    ExpectedSignerStatus(signer, SignatureStatus.Signed)
                )
                .Get_XAdES(signer)
                .Get_PAdES()
                .Confirm_status();
        }

        [Fact]
        public void create_job_with_queue()
        {
            var signer = new PersonalIdentificationNumber("12345678910");
            var senderWithQueue = new Sender(BringPublicOrganizationNumber, new PollingQueue("CustomDirectPollingQueue"));

            _fixture.TestHelper
                .Create_pollable_direct_job(senderWithQueue, signer)
                .Sign_job(signer)
                .Get_status_by_polling(senderWithQueue)
                .Expect_job_to_have_status(JobStatus.CompletedSuccessfully)
                .Get_status_by_polling(senderWithQueue)
                .Expect_job_to_have_status(JobStatus.NoChanges);
        }
    }
}
