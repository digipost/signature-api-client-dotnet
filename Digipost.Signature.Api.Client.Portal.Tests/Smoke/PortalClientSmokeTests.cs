using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.Enums;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

namespace Digipost.Signature.Api.Client.Portal.Tests.Smoke
{
    [TestClass]
    public class PortalClientSmokeTests : SmokeTests
    {
        private static XadesReference _xadesReference;
        private static PadesReference _padesReference;
        private static ConfirmationReference _confirmationReference;

        private static PortalClient _portalClient;

        protected static PortalClient GetPortalClient()
        {
            if (_portalClient != null)
            {
                return _portalClient;
            }

            switch (ClientType)
            {
                case Client.Localhost:
                    _portalClient = GetPortalClient(Environment.Localhost);
                    break;
                case Client.DifiTest:
                    _portalClient = GetPortalClient(Environment.DifiTest);
                    break;
                case Client.DifiQa:
                    _portalClient = GetPortalClient(Environment.DifiQa);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _portalClient;
        }

        private static PortalClient GetPortalClient(Environment environment)
        {
            var sender = new Sender("988015814");
            var clientConfig = new ClientConfiguration(environment, CoreDomainUtility.GetTestIntegrasjonSertifikat(), sender);
            var client = new PortalClient(clientConfig);
            return client;
        }

        [TestClass]
        public class RunsEndpointCallsSuccessfully : PortalClientSmokeTests
        {
            [ClassInitialize]
            public static void CreateAndGetStatus(TestContext context)
            {
                var portalClient = GetPortalClient();
                var portalJob = DomainUtility.GetPortalJob(1);

                var portalJobResponse = portalClient.Create(portalJob).Result;

                var signer = portalJob.Signers.ElementAt(0);
                portalClient.AutoSign((int) portalJobResponse.JobId, signer.PersonalIdentificationNumber).Wait();

                var jobStatusChangeResponse = GetCurrentReceipt(portalJobResponse.JobId, portalClient);

                Assert.AreEqual(JobStatus.Completed, jobStatusChangeResponse.Status);

                _xadesReference = new XadesReference(GetUriFromRelativePath(jobStatusChangeResponse.Signatures.ElementAt(0).XadesReference.Url.AbsolutePath));
                _padesReference = new PadesReference(GetUriFromRelativePath(jobStatusChangeResponse.PadesReference.Url.AbsolutePath));
                _confirmationReference = new ConfirmationReference(GetUriFromRelativePath(jobStatusChangeResponse.ConfirmationReference.Url.AbsolutePath));
            }

            private static PortalJobStatusChanged GetCurrentReceipt(long jobId, PortalClient portalClient)
            {
                PortalJobStatusChanged portalJobStatusChanged = null;
                while (portalJobStatusChanged == null)
                {
                    var statusChange = portalClient.GetStatusChange().Result;
                    if (statusChange.JobId == jobId)
                    {
                        portalJobStatusChanged = statusChange;
                    }
                    else
                    {
                        var uri = GetUriFromRelativePath(statusChange.ConfirmationReference.Url.AbsolutePath);
                        portalClient.Confirm(new ConfirmationReference(uri)).Wait();
                    }
                }

                return portalJobStatusChanged;
            }

            [TestMethod]
            public async Task GetsXadesSuccessfully()
            {
                //Arrange
                var portalClient = GetPortalClient();

                //Act
                await portalClient.GetXades(_xadesReference);
                //await WriteXadesToFile(portalClient, xadesReference);

                //Assert
            }

            private static async Task WriteXadesToFile(PortalClient portalClient, XadesReference xadesReference)
            {
                using (var xadesStream = await portalClient.GetXades(xadesReference))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        xadesStream.CopyTo(memoryStream);
                        File.WriteAllBytes(@"C:\Users\aas\Downloads\xades.xml", memoryStream.ToArray());
                    }
                }
            }

            [TestMethod]
            public async Task GetsPadesSuccessfully()
            {
                //Arrange
                var portalClient = GetPortalClient();

                //Act
                await portalClient.GetPades(_padesReference);
                //await WritePadesToFile(portalClient, padesReference);

                //Assert
            }

            private static async Task WritePadesToFile(PortalClient portalClient, PadesReference padesReference)
            {
                using (var padesStream = await portalClient.GetPades(padesReference))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        padesStream.CopyTo(memoryStream);
                        File.WriteAllBytes(@"C:\Users\aas\Downloads\pades.pdf", memoryStream.ToArray());
                    }
                }
            }

            [TestMethod]
            public async Task CancelsSuccessfully()
            {
                //Arrange
                var portalJob = new PortalJob(CoreDomainUtility.GetDocument(), CoreDomainUtility.GetSigners(1), "aReference");
                var portalClient = GetPortalClient();

                var portalJobResponse = await portalClient.Create(portalJob);
                var cancellationReference = new CancellationReference(GetUriFromRelativePath(portalJobResponse.CancellationReference.Url.AbsolutePath));

                //Act
                portalClient.Cancel(cancellationReference).Wait();

                var changeResponse = await portalClient.GetStatusChange();

                //Assert
                Assert.AreEqual(SignatureStatus.Cancelled, changeResponse.Signatures.ElementAt(0).SignatureStatus);
            }

            [TestMethod]
            public async Task ConfirmsSuccessfully()
            {
                //Arrange
                var portalClient = GetPortalClient();

                //Act
                await portalClient.Confirm(_confirmationReference);

                //Assert
            }
        }
    }
}