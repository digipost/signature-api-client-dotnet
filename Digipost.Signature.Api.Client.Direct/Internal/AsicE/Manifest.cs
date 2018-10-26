using System.Collections.Generic;
using System.Text;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;

namespace Digipost.Signature.Api.Client.Direct.Internal.AsicE
{
    internal class Manifest : IAsiceAttachable
    {
        public Manifest(Sender sender, Document document, IEnumerable<AbstractSigner> signers)
        {
            Sender = sender;
            Document = document;
            Signers = signers;
        }

        public Sender Sender { get; internal set; }

        public AbstractDocument Document { get; internal set; }

        public IEnumerable<AbstractSigner> Signers { get; internal set; }

        public AuthenticationLevel? AuthenticationLevel { get; internal set; }

        public IdentifierInSignedDocuments? IdentifierInSignedDocuments { get; set; }

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
