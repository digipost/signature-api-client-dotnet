using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Common.Logging;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Portal.Enums;
using Digipost.Signature.Api.Client.Portal.Tests.Utilities;
using Xunit;
using Environment = Digipost.Signature.Api.Client.Core.Environment;

namespace Digipost.Signature.Api.Client.Portal.Tests.Smoke
{
    public class PortalClientSmokeTests : SmokeTests
    {
        private static PortalClient _portalClient;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public class PortalSmokeTestsFixture : SmokeTests, IDisposable
        {
            public PortalSmokeTestsFixture()
            {
                var portalClient = GetPortalClient();
                Log.Debug($"Sending in PortalClient Class Initialize. {portalClient.ClientConfiguration}");
                var portalJob = DomainUtility.GetPortalJobWithSignerIdentifier();

                var portalJobResponse = portalClient.Create(portalJob).Result;
                Log.Debug($"Result of Create was: {portalJobResponse}");

                var signer = portalJob.Signers.ElementAt(0);
                var httpResponseMessage = portalClient.AutoSign((int) portalJobResponse.JobId, signer.Identifier.Value).Result;
                Log.Debug($"Trying to autosign. Status code: {httpResponseMessage.StatusCode}");

                var jobStatusChangeResponse = GetCurrentReceipt(portalJobResponse.JobId, portalClient);

                Assert.Equal(JobStatus.CompletedSuccessfully, jobStatusChangeResponse.Status);

                XadesReference = new XadesReference(GetUriFromRelativePath(jobStatusChangeResponse.Signatures.ElementAt(0).XadesReference.Url.AbsolutePath));
                PadesReference = new PadesReference(GetUriFromRelativePath(jobStatusChangeResponse.PadesReference.Url.AbsolutePath));
                ConfirmationReference = new ConfirmationReference(GetUriFromRelativePath(jobStatusChangeResponse.ConfirmationReference.Url.AbsolutePath));
            }

            public XadesReference XadesReference { get; }

            public PadesReference PadesReference { get; }

            public ConfirmationReference ConfirmationReference { get; }

            public void Dispose()
            {
            }

            private static JobStatusChanged GetCurrentReceipt(long jobId, PortalClient portalClient)
            {
                JobStatusChanged jobStatusChanged = null;
                while (jobStatusChanged == null)
                {
                    var statusChange = portalClient.GetStatusChange().Result;
                    if (statusChange.JobId == jobId)
                    {
                        jobStatusChanged = statusChange;
                    }
                    else if (statusChange.Status == JobStatus.NoChanges)
                    {
                        throw new Exception("Expected receipt, got emtpy queue.");
                    }
                    else
                    {
                        var uri = GetUriFromRelativePath(statusChange.ConfirmationReference.Url.AbsolutePath);
                        portalClient.Confirm(new ConfirmationReference(uri)).Wait();
                    }
                }

                return jobStatusChanged;
            }

            internal PortalClient GetPortalClient()
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
                    case Client.Test:
                        var testEnvironment = Environment.DifiTest;
                        testEnvironment.Url = new Uri(Environment.DifiQa.Url.AbsoluteUri.Replace("difiqa", "test"));
                        _portalClient = GetPortalClient(testEnvironment);
                        break;
                    case Client.Qa:
                        var qaTestEnvironment = Environment.DifiTest;
                        qaTestEnvironment.Url = new Uri(Environment.DifiQa.Url.AbsoluteUri.Replace("difiqa", "qa"));
                        _portalClient = GetPortalClient(qaTestEnvironment);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _portalClient.ClientConfiguration.LogRequestAndResponse = true;

                return _portalClient;
            }

            private static PortalClient GetPortalClient(Environment environment)
            {
                var sender = new Sender("988015814");
                var clientConfig = new ClientConfiguration(environment, CoreDomainUtility.GetBringCertificate(), sender) {HttpClientTimeoutInMilliseconds = 30000};
                var client = new PortalClient(clientConfig);
                return client;
            }
        }

        public class RunsEndpointCallsSuccessfully : IClassFixture<PortalSmokeTestsFixture>
        {
            public RunsEndpointCallsSuccessfully(PortalSmokeTestsFixture fixture)
            {
                this.fixture = fixture;
            }

            public PortalSmokeTestsFixture fixture { get; set; }

            private static async Task WriteXadesToFile(PortalClient portalClient, XadesReference xadesReference)
            {
                using (var xadesStream = await portalClient.GetXades(xadesReference).ConfigureAwait(false))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        xadesStream.CopyTo(memoryStream);
                        File.WriteAllBytes(@"C:\Users\aas\Downloads\xades.xml", memoryStream.ToArray());
                    }
                }
            }

            private static async Task WritePadesToFile(PortalClient portalClient, PadesReference padesReference)
            {
                using (var padesStream = await portalClient.GetPades(padesReference).ConfigureAwait(false))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        padesStream.CopyTo(memoryStream);
                        File.WriteAllBytes(@"C:\Users\aas\Downloads\pades.pdf", memoryStream.ToArray());
                    }
                }
            }

            [Fact]
            public async Task Cancels_successfully()
            {
                //Arrange
                var portalJob = new Job(DomainUtility.GetPortalDocument(), DomainUtility.GetSignerWithPersonalIdentificationNumber(), "aReference");
                var portalClient = fixture.GetPortalClient();

                var portalJobResponse = await portalClient.Create(portalJob).ConfigureAwait(false);
                var cancellationReference = new CancellationReference(GetUriFromRelativePath(portalJobResponse.CancellationReference.Url.AbsolutePath));

                //Act
                portalClient.Cancel(cancellationReference).Wait();

                var changeResponse = await portalClient.GetStatusChange().ConfigureAwait(false);

                await portalClient.Confirm(changeResponse.ConfirmationReference).ConfigureAwait(false);

                //Assert
                Assert.Equal(SignatureStatus.Cancelled, changeResponse.Signatures.ElementAt(0).SignatureStatus);
            }

            [Fact]
            public async Task Confirms_successfully()
            {
                //Arrange
                var portalClient = fixture.GetPortalClient();

                //Act
                await portalClient.Confirm(fixture.ConfirmationReference).ConfigureAwait(false);

                //Assert
            }

            [Fact]
            public async Task Gets_pades_successfully()
            {
                //Arrange
                var portalClient = fixture.GetPortalClient();

                //Act
                await portalClient.GetPades(fixture.PadesReference).ConfigureAwait(false);
                //await WritePadesToFile(portalClient, padesReference).ConfigureAwait(false);

                //Assert
            }

            [Fact]
            public async Task Gets_xades_successfully()
            {
                //Arrange
                var portalClient = fixture.GetPortalClient();

                //Act
                await portalClient.GetXades(fixture.XadesReference).ConfigureAwait(false);
                //await WriteXadesToFile(portalClient, xadesReference).ConfigureAwait(false);

                //Assert
            }
        }
    }
}