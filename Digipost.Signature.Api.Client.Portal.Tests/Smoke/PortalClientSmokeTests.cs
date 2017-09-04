﻿using System;
using System.Reflection;
using Common.Logging;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Xunit;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

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
            var sender = new Sender("988015814");
            var clientConfig = new ClientConfiguration(environment, CoreDomainUtility.GetBringCertificate(), sender) {HttpClientTimeoutInMilliseconds = 30000};
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
            var signer = new PersonalIdentificationNumber("12345678910");

            _fixture.TestHelper
                .Create_portal_job(signer)
                .Cancel_job()
                .GetJobStatusChanged()
                .Confirm_job();
        }

        [Fact]
        public void Can_create_job_and_confirm()
        {
            var signer = new PersonalIdentificationNumber("12345678910");

            _fixture.TestHelper
                .Create_portal_job(signer)
                .Sign_job()
                .GetJobStatusChanged()
                .GetSignatureForSigner()
                .GetXades()
                .GetPades()
                .Confirm_job();
        }
    }
}