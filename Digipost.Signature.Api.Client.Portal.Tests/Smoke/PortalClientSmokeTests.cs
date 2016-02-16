using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests.Smoke
{
    [TestClass]
    public class PortalClientSmokeTests : SmokeTests
    {
        
        private static XadesReference _xadesReference;
        private static PadesReference _padesReference;
        private static ConfirmationReference _confirmationReference;
        
        [TestClass]
        public class RunsEndpointCallsSuccessfully : PortalClientSmokeTests
        {
            [ClassInitialize]
            public static void ClassInitialize(TestContext context)
            {
                var portalClient = GetPortalClient();
                var portalJob = DomainUtility.GetPortalJob(signers: 1);

                var portalJobResponse = portalClient.Create(portalJob).Result;

                var signer = portalJob.Signers.ElementAt(0);
                portalClient.AutoSign((int)portalJobResponse.JobId, signer.PersonalIdentificationNumber).Wait();

                var jobStatusChangeResponse = GetCurrentReceipt(portalJobResponse.JobId, portalClient);
                
                
                Assert.AreEqual(JobStatus.Completed, jobStatusChangeResponse.Status);

                _xadesReference = new XadesReference(GetUriFromRelativePath(jobStatusChangeResponse.Signatures.ElementAt(0).XadesReference.Url.AbsolutePath));
                _padesReference = new PadesReference(GetUriFromRelativePath(jobStatusChangeResponse.PadesReference.Url.AbsolutePath));
                _confirmationReference = new ConfirmationReference(GetUriFromRelativePath(jobStatusChangeResponse.ConfirmationReference.Url.AbsolutePath));
            }

            private static PortalJobStatusChangeResponse GetCurrentReceipt(long jobId,  PortalClient portalClient)
            {
                PortalJobStatusChangeResponse portalJobStatusChangeResponse = null;
                while (portalJobStatusChangeResponse == null)
                {
                    var statusChange = portalClient.GetStatusChange().Result;
                    if (statusChange.JobId == jobId)
                    {
                        portalJobStatusChangeResponse = statusChange;
                    }
                    else
                    {
                        var uri = GetUriFromRelativePath(statusChange.ConfirmationReference.Url.AbsolutePath);
                        portalClient.Confirm(new ConfirmationReference(uri)).Wait();
                    }
                }

                return portalJobStatusChangeResponse;
            }

            [TestMethod]
            public async Task GetsXadesSuccessfully()
            {
                //Arrange
                var portalClient = GetPortalClient();

                //Act
                var xades = await portalClient.GetXades(_xadesReference);
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
                var pades = portalClient.GetPades(_padesReference);
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
                
                //Act
                await portalClient.Confirm(_confirmationReference);

                //Assert
            }


        }
    }
}
