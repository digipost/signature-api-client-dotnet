using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digipost.Signature.Api.Client.Core.Asice
{
    internal abstract class AsiceGenerator
    {
        protected static AsiceArchive GetAsiceArchive(ISignatureJob signatureJobForMetadata, IAsiceConfiguration asiceConfiguration, params IAsiceAttachable[] asiceAttachables)
        {
            return asiceConfiguration == null
                ? new AsiceArchive(asiceAttachables)
                : new AsiceArchive(asiceConfiguration.DocumentBundleProcessors, signatureJobForMetadata, asiceAttachables);
        }
    }
}
