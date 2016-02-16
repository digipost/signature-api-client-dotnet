using System;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct;
using Digipost.Signature.Api.Client.Portal;

namespace Digipost.Signature.Api.Client.Core.Tests.Smoke
{
    public class SmokeTests
    {
        protected static readonly Client ClientType = Client.Localhost;
        
        protected static readonly Uri Localhost = new Uri("https://172.16.91.1:8443");
        protected static readonly Uri DifitestSigneringPostenNo = new Uri("https://api.difitest.signering.posten.no");

        protected enum Client
        {
            Localhost,
            DifiTest
        }

        protected static Uri GetUriFromRelativePath(string relativeUri)
        {
            Uri result = null;

            switch (ClientType)
            {
                case Client.Localhost:
                    result = new Uri(Localhost, relativeUri);
                    break;
                case Client.DifiTest:
                    result = new Uri(DifitestSigneringPostenNo, relativeUri);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
        
        protected static DirectClient GetDirectClient()
        {
            DirectClient client;

            switch (ClientType)
            {
                case Client.Localhost:
                    client = DirectClient(Localhost);
                    break;
                case Client.DifiTest:
                    client = DirectClient(DifitestSigneringPostenNo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return client;
        }

        private static DirectClient DirectClient(Uri uri)
        {
            var sender = new Sender("988015814");
            var clientConfig = new ClientConfiguration(uri, sender, DomainUtility.GetTestIntegrasjonSertifikat());
            var client = new DirectClient(clientConfig);
            return client;
        }

        protected static PortalClient GetPortalClient()
        {
            PortalClient client;

            switch (ClientType)
            {
                case Client.Localhost:
                    client = GetPortalClient(Localhost);
                    break;
                case Client.DifiTest:
                    client = GetPortalClient(DifitestSigneringPostenNo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return client;
        }

        private static PortalClient GetPortalClient(Uri uri)
        {
            var sender = new Sender("988015814");
            var clientConfig = new ClientConfiguration(uri, sender, DomainUtility.GetTestIntegrasjonSertifikat());
            var client = new PortalClient(clientConfig);
            return client;
        }
    }
}