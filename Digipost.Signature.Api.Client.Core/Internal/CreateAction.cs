using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace Digipost.Signature.Api.Client.Core.Internal
{
    internal class CreateAction : DigipostAction
    {
        public CreateAction(IRequestContent requestContent, ClientConfiguration clientConfig, X509Certificate2 businessCertificate, string uri) : base(requestContent, clientConfig, businessCertificate, uri)
        {
            throw new NotImplementedException();
        }

        protected override HttpContent Content(IRequestContent requestContent)
        {
            throw new NotImplementedException();
        }

        protected override string Serialize(IRequestContent requestContent)
        {
            throw new NotImplementedException();
        }
    }
}
