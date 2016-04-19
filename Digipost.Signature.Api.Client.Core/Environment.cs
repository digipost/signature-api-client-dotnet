using System;
using Difi.Felles.Utility;
using Difi.Felles.Utility.Utilities;

namespace Digipost.Signature.Api.Client.Core
{
    public class Environment : AbstraktMiljø
    {
        private Environment(Sertifikatkjedevalidator certificateChainValidator, Uri url)
        {
            Url = url;
            Sertifikatkjedevalidator = certificateChainValidator;
        }

        internal static Environment Localhost => new Environment(
            new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater()),
            new Uri("https://172.16.91.1:8443")
            );
        
        public static Environment DifiTest => new Environment(
            new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater()),
            new Uri("https://api.difitest.signering.posten.no")
            );

        public static Environment DifiQa => new Environment(
            new Sertifikatkjedevalidator(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater()),
            new Uri("https://api.difiqa.signering.posten.no")
            );
        public static Environment Production => new Environment(
            new Sertifikatkjedevalidator(SertifikatkjedeUtility.ProduksjonsSertifikater()),
            new Uri("https://api.signering.posten.no")
            );
    }
}