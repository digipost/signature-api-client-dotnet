using System;
using Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects;

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
    }
}
