using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Enums;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Direct.Enums;

namespace Digipost.Signature.Api.Client.Direct
{
    public class Job : IRequestContent, ISignatureJob
    {
        public Job(Document document, IEnumerable<Signer> signers, string reference, ExitUrls exitUrls, Sender sender = null, StatusRetrievalMethod statusRetrievalMethod = StatusRetrievalMethod.WaitForCallback)
        {
            Reference = reference;
            Signers = signers;
            Document = document;
            ExitUrls = exitUrls;
            Sender = sender;
            StatusRetrievalMethod = statusRetrievalMethod;
        }

        public IEnumerable<Signer> Signers { get; }

        public ExitUrls ExitUrls { get; }

        public StatusRetrievalMethod StatusRetrievalMethod { get; }

        public string Reference { get; }

        public AbstractDocument Document { get; }

        public Sender Sender { get; internal set; }

        public AuthenticationLevel? AuthenticationLevel { get; set; }

        public override string ToString()
        {
            return $"Signers: {Signers}, ExitUrls: {ExitUrls}, Reference: {Reference}, Document: {Document}, Sender: {Sender}, Retrieving status by {StatusRetrievalMethod}";
        }
    }
}