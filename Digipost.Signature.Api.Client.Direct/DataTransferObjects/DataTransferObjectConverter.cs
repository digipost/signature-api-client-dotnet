using System;
using Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects;

namespace Digipost.Signature.Api.Client.Direct.DataTransferObjects
{
    public static class DataTransferObjectConverter

    {
        public static DirectJobDataTranferObject ToDataTransferObject(DirectJob directJob)
        {
            return new DirectJobDataTranferObject()
            {
                Reference = directJob.Reference,
                SenderDataTransferObject = Core.Asice.DataTransferObjects.DataTransferObjectConverter.ToDataTransferObject(directJob.Sender),
                DocumentDataTransferObject = Core.Asice.DataTransferObjects.DataTransferObjectConverter.ToDataTransferObject(directJob.Document),
                SignerDataTranferObject = Core.Asice.DataTransferObjects.DataTransferObjectConverter.ToDataTransferObject(directJob.Signer),
                ExitUrlsDataTranferObject = ToDataTransferObject(directJob.ExitUrls),

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
    }
}
