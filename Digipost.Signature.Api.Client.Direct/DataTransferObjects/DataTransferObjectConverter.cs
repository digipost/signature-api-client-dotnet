using System;
using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct.DataTransferObjects
{
    public static class DataTransferObjectConverter

    {
        public static DirectJobDataTransferObject ToDataTransferObject(DirectJob directJob)
        {
            return new DirectJobDataTransferObject()
            {
                Reference = directJob.Reference,
                SenderDataTransferObject = Core.Asice.DataTransferObjects.DataTransferObjectConverter.ToDataTransferObject(directJob.Sender),
                SignerDataTranferObject = Core.Asice.DataTransferObjects.DataTransferObjectConverter.ToDataTransferObject(directJob.Signer),
                ExitUrlsDataTranferObject = ToDataTransferObject(directJob.ExitUrls)
            };
        }

        public static ExitUrlsDataTranferObject ToDataTransferObject(ExitUrls exitUrls)
        {
            return new ExitUrlsDataTranferObject()
            {
                CancellationUrl = exitUrls.CancellationUrl.AbsoluteUri,
                CompletionUrl = exitUrls.CompletionUrl.AbsoluteUri,
                ErrorUrl = exitUrls.ErrorUrl.AbsoluteUri
            };
        }

        public static DirectJobResponse FromDataTransferObject(
            DirectJobResponseDataTransferObject directJobResponseDataTransferObject)
        {
            return new DirectJobResponse(
                Int64.Parse(directJobResponseDataTransferObject.SignatureJobId),
                new ResponseUrls(
                    new Uri(directJobResponseDataTransferObject.RedirectUrl),
                    new Uri(directJobResponseDataTransferObject.StatusUrl)
                  )
               );
        }

        public static DirectJobStatusResponse FromDataTransferObject(DirectJobStatusResponseDataTransferObject directJobStatusResponseDataTransferObject)
        {
            var source = directJobStatusResponseDataTransferObject;

            var jobId = Int64.Parse(source.JobId);
            var jobStatus = (JobStatus) Enum.Parse(typeof (JobStatus), source.Status, ignoreCase: true);

            DirectJobStatusResponse directJobStatusResponse;
            var signedJob = source.ComfirmationUrl != null;

            if (signedJob)
            {
                directJobStatusResponse = new DirectJobStatusResponse(
                    jobId,
                    jobStatus,
                    new StatusResponseUrls(
                        confirmation: new Uri(source.ComfirmationUrl),
                        xades: new Uri(source.XadesUrl),
                        pades: new Uri(source.PadesUrl)
                        )
                    );
            }
            else
            {
                directJobStatusResponse = new DirectJobStatusResponse(
                    jobId,
                    jobStatus,
                    new StatusResponseUrls(
                        confirmation: null,
                        xades: null,
                        pades: null
                        )
                    );
            }

            return directJobStatusResponse;
        }
    }
}
