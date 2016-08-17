using System;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Digipost.Signature.Api.Client.Direct.Tests.Smoke.TestHelper;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

namespace Digipost.Signature.Api.Client.Direct.Tests.Smoke
{
    [TestClass]
    public class DirectClientSmokeTests : SmokeTests
    {
        private static DirectClient _directClient;

        private static TestHelper _t;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _t = new TestHelper(GetDirectClient());
        }

        [TestMethod]
        public void Can_create_job_with_one_signer()
        {
            var signer = new PersonalIdentificationNumber("12345678910");

            _t.Create_direct_job(signer)
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

        [TestMethod]
        public void Can_create_job_with_multiple_signers()
        {
            var signer1 = new PersonalIdentificationNumber("12345678910");
            var signer2 = new PersonalIdentificationNumber("10987654321");

            _t.Create_direct_job(signer1, signer2)
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

        [TestMethod]
        public void Can_retrieve_status_by_polling()
        {
            var signer = new PersonalIdentificationNumber("12345678910");

            _t.Create_pollable_direct_job(signer)
                .Sign_job(signer)
                .Get_status_by_polling()
                .Expect_job_to_have_status(
                    JobStatus.CompletedSuccessfully,
                    ExpectedSignerStatus(signer, SignatureStatus.Signed)
                )
                .Get_XAdES(signer)
                .Get_PAdES()
                .Confirm_status();
        }

        private static DirectClient GetDirectClient()
        {
            if (_directClient != null)
            {
                return _directClient;
            }

            switch (ClientType)
            {
                case Client.Localhost:
                    _directClient = DirectClient(Environment.Localhost);
                    break;
                case Client.DifiTest:
                    _directClient = DirectClient(Environment.DifiTest);
                    break;
                case Client.DifiQa:
                    _directClient = DirectClient(Environment.DifiQa);
                    break;
                case Client.Test:
                    var testEnvironment = Environment.DifiTest;
                    testEnvironment.Url = new Uri(Environment.DifiQa.Url.AbsoluteUri.Replace("difiqa", "test"));
                    _directClient = DirectClient(testEnvironment);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _directClient;
        }

        private static DirectClient DirectClient(Environment environment)
        {
            var sender = new Sender("988015814");
            var clientConfig = new ClientConfiguration(environment, CoreDomainUtility.GetTestIntegrasjonSertifikat(), sender);
            var client = new DirectClient(clientConfig);
            return client;
        }
    }
}