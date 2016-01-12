using System;
using System.Collections.Generic;
using System.Linq;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;

namespace Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects
{
    public static class DataTransferObjectConverter
    {
        public static ManifestDataTranferObject ToDataTransferObject(Manifest manifest)
        {
            return new ManifestDataTranferObject()
            {
                SenderDataTransferObject = ToDataTransferObject(manifest.Sender),
                DocumentDataTransferObject = ToDataTransferObject(manifest.Document),
                SignersDataTransferObjects = ToDataTransferObject(manifest.Signers).ToList()
            };
        }

        public static SenderDataTransferObject ToDataTransferObject(Sender sender)
        {
            return new SenderDataTransferObject()
            {
                Organization = sender.OrganizationNumber
            };
        }

        public static DocumentDataTransferObject ToDataTransferObject(Document document)
        {
            return new DocumentDataTransferObject()
            {
                Title = document.Subject,
                Descritpion = document.Message,
                Href = document.FileName,
                Mime = document.MimeType
            };
        }

        public static IEnumerable<SignerDataTranferObject> ToDataTransferObject(IEnumerable<Signer> signers)
        {
            return signers.Select(signer => ToDataTransferObject(signer)).ToList();
        }

        public static SignerDataTranferObject ToDataTransferObject(Signer signer)
        {
            return new SignerDataTranferObject()
            {
                PersonalIdentificationNumber = signer.PersonalIdentificationNumber
            };
        }
    }
}
