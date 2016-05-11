using System.Collections.Generic;
using System.Text;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Portal.DataTransferObjects;

namespace Digipost.Signature.Api.Client.Portal.Internal.AsicE
{
    internal class PortalManifest : IAsiceAttachable
    {
        public PortalManifest(Sender sender, PortalDocument portalDocument, IEnumerable<Signer> signers)
        {
            Sender = sender;
            PortalDocument = portalDocument;
            Signers = signers;
        }

        public Sender Sender { get; internal set; }

        public PortalDocument PortalDocument { get; internal set; }

        public IEnumerable<Signer> Signers { get; internal set; }

        public Availability Availability { get; set; }

        public byte[] Bytes
        {
            get
            {
                var manifestDataTranferObject = DataTransferObjectConverter.ToDataTransferObject(this);
                var serializedManifest = SerializeUtility.Serialize(manifestDataTranferObject);

                return Encoding.UTF8.GetBytes(serializedManifest);
            }
        }

        public string Id => "Id_1";

        public string FileName => "manifest.xml";

        public string MimeType => "application/xml";
    }
}