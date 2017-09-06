using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Logging;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Core.Tests.Smoke;
using Digipost.Signature.Api.Client.Portal.Enums;
using Xunit;
using static Digipost.Signature.Api.Client.Portal.Tests.Utilities.DomainUtility;

namespace Digipost.Signature.Api.Client.Portal.Tests.Smoke
{
    public class TestHelper : TestHelperBase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly PortalClient _client;
        private CancellationReference _cancellationReference;
        private ConfirmationReference _confirmationReference;
        private Job _job;
        private JobResponse _jobResponse;
        private JobStatusChanged _jobStatusChanged;
        private PadesReference _padesReference;
        private XadesReference _xadesReference;

        public TestHelper(PortalClient client)
        {
            _client = client;
            Log.Debug($"Sending in PortalClient Class Initialize. {_client.ClientConfiguration}");
        }

        public TestHelper Create_job(Sender sender, params Signer[] signers)
        {
            return Create(GetPortalJob(signers), sender);
        }

        public TestHelper Create_job(params Signer[] signers)
        {
            return Create(GetPortalJob(signers));
        }

        private TestHelper Create(Job job, Sender optionalSender = null)
        {
            if (optionalSender != null)
            {
                job.Sender = optionalSender;
            }

            _job = job;
            _jobResponse = _client.Create(_job).Result;
            _cancellationReference = new CancellationReference(TransformReferenceToCorrectEnvironment(_jobResponse.CancellationReference.Url));

            Log.Debug($"Result of Create was: {_jobResponse}");

            return this;
        }

        public TestHelper Sign_job()
        {
            var signer = _job.Signers.First();

            if (signer.Identifier is ContactInformation)
            {
                throw new Exception("Unable to sign a contact identified by contact information, because this is not implemented.");
            }

            var httpResponseMessage = _client.AutoSign((int) _jobResponse.JobId, ((PersonalIdentificationNumber) signer.Identifier).Value).Result;
            Assert.True(httpResponseMessage.IsSuccessStatusCode, "Signing through API failed. Are you trying to sign a job type " +
                                                                 "that we do not offer API signing for? Remember that this is not " +
                                                                 "possible in production!");

            return this;
        }

        public TestHelper GetJobStatusChanged()
        {
            Assert_state(_jobResponse);

            _jobStatusChanged = GetCurrentReceipt(_jobResponse.JobId, _client);
            _confirmationReference = new ConfirmationReference(TransformReferenceToCorrectEnvironment(_jobStatusChanged.ConfirmationReference.Url));

            if (_jobStatusChanged.Status == JobStatus.CompletedSuccessfully)
            {
                _xadesReference = new XadesReference(TransformReferenceToCorrectEnvironment(_jobStatusChanged.Signatures.First().XadesReference.Url));
                _padesReference = new PadesReference(TransformReferenceToCorrectEnvironment(_jobStatusChanged.PadesReference.Url));
            }

            return this;
        }

        public TestHelper GetSignatureForSigner()
        {
            var signature = _jobStatusChanged.GetSignatureFor(_job.Signers.FirstOrDefault());

            Assert.NotNull(signature);

            return this;
        }

        public TestHelper GetXades()
        {
            Assert_state(_xadesReference);

            _xadesReference = new XadesReference(TransformReferenceToCorrectEnvironment(_jobStatusChanged.Signatures.ElementAt(0).XadesReference.Url));
            _client.GetXades(_xadesReference).ConfigureAwait(false).GetAwaiter().GetResult();

            return this;
        }

        public TestHelper GetPades()
        {
            Assert_state(_padesReference);

            _padesReference = new PadesReference(TransformReferenceToCorrectEnvironment(_jobStatusChanged.PadesReference.Url));
            _client.GetPades(_padesReference).ConfigureAwait(false).GetAwaiter().GetResult();

            return this;
        }

        public TestHelper Persist_xades_to_file()
        {
            Assert_state(_xadesReference);

            using (var xadesStream = _client.GetXades(_xadesReference).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                using (var memoryStream = new MemoryStream())
                {
                    xadesStream.CopyTo(memoryStream);
                    File.WriteAllBytes(@"C:\Users\<User>\Downloads\xades.xml", memoryStream.ToArray());
                }
            }

            return this;
        }

        public TestHelper Persist_pades_to_file()
        {
            Assert_state(_padesReference);

            using (var padesStream = _client.GetPades(_padesReference).ConfigureAwait(false).GetAwaiter().GetResult())
            {
                using (var memoryStream = new MemoryStream())
                {
                    padesStream.CopyTo(memoryStream);
                    File.WriteAllBytes(@"C:\Users\<User>\Downloads\pades.pdf", memoryStream.ToArray());
                }
            }

            return this;
        }

        public TestHelper Confirm_job()
        {
            Assert_state(_confirmationReference);

            _client.Confirm(_confirmationReference).ConfigureAwait(false).GetAwaiter().GetResult();

            return this;
        }

        public TestHelper Cancel_job()
        {
            _client.Cancel(_cancellationReference).ConfigureAwait(false).GetAwaiter().GetResult();

            return this;
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
                    var uri = TransformReferenceToCorrectEnvironment(statusChange.ConfirmationReference.Url);
                    portalClient.Confirm(new ConfirmationReference(uri)).Wait();
                }
            }

            return jobStatusChanged;
        }

        private static void Assert_state(object obj)
        {
            if (obj == null)
            {
                throw new InvalidOperationException("Requires gradually built state. Make sure you use functions in the correct order");
            }
        }
    }
}