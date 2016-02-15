using System;
using System.IO;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests.Smoke
{
    [TestClass]
    public class PortalClientSmokeTests : SmokeTests
    {
        private const string RelativeCompleteUrl = "/api/988015814/portal/signature-jobs/48/complete";
        private const string RelativePadesUrl = "/api/988015814/portal/signature-jobs/50/pades";
        private const string RelativeXadesUrl = "/api/988015814/portal/signature-jobs/50/xades/57";
        private static readonly string DifitestSigneringPostenNoRelativeStatusUrl = "/api/signature-jobs/59/status";

        [TestClass]
        public class RunsEndpointCallsSuccessfully : PortalClientSmokeTests
        {
            [TestMethod]
            public async Task CreatesSuccessfully()
            {
                //Arrange
                var portalClient = GetPortalClient();
                var portalJob = DomainUtility.GetPortalJob(signers: 1);
                

                //Act
                var result = await portalClient.Create(portalJob);

                //Assert
                Assert.IsNotNull(result.JobId);
            }

            //[Ignore] //Because will only be tested when a change actually happens.
            [TestMethod]
            public async Task GetsStatusSuccessfully()
            {
                //Arrange
                var portalClient = GetPortalClient();

                //Act
                var status = await portalClient.GetStatusChange();

                //Assert
                Assert.AreEqual(JobStatus.Completed,status.Status);

            }

            [TestMethod]
            public async Task GetsXadesSuccessfully()
            {
                //Arrange
                var portalClient = GetPortalClient();
                var xadesReference = new XadesReference(GetUriFromRelativePath(RelativeXadesUrl));

                //Act
                var xades = await portalClient.GetXades(xadesReference);

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
                var padesReference = new PadesReference(GetUriFromRelativePath(RelativePadesUrl));

                var pades = portalClient.GetPades(padesReference);

                //Act
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
            public async Task ConfirmsSuccessfully()
            {
                //Arrange
                var portalClient = GetPortalClient();
                var confirmationReference = new ConfirmationReference(GetUriFromRelativePath(RelativeCompleteUrl));

                //Act
                await portalClient.Confirm(confirmationReference);

                //Assert
            }
        }
    }
}
