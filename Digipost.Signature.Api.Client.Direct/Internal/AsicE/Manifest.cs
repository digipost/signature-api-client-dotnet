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
        public Manifest(string title, Sender sender, IEnumerable<Document> documents, IEnumerable<AbstractSigner> signers)
        {
            Title = title;
            Sender = sender;
            Documents = documents;
            Signers = signers;
        }

        public string Title { get; }
        
        public string Description { get; set; }
        
        public Sender Sender { get; }

        public IEnumerable<Document> Documents { get; }

        public IEnumerable<AbstractSigner> Signers { get; }

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
