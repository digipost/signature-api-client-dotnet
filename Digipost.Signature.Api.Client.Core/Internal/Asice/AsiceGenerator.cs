using System.Collections.Generic;
using System.Linq;

namespace Digipost.Signature.Api.Client.Core.Internal.Asice
{
    internal abstract class AsiceGenerator
    {
        protected static AsiceArchive GetAsiceArchive(ISignatureJob signatureJobForMetadata, IAsiceConfiguration asiceConfiguration, IEnumerable<Document> documents ,params IAsiceAttachable[] asiceAttachables)
        {
            var asiceAttachableProcessors = ConvertDocumentBundleProcessorsToAsiceAttachableProcessors(signatureJobForMetadata, asiceConfiguration);

            return new AsiceArchive(asiceAttachableProcessors, documents.Concat(asiceAttachables).ToArray());
        }

        private static IEnumerable<AsiceAttachableProcessor> ConvertDocumentBundleProcessorsToAsiceAttachableProcessors(ISignatureJob signatureJobForMetadata, IAsiceConfiguration asiceConfiguration)
        {
            return asiceConfiguration.DocumentBundleProcessors.Select(p => new AsiceAttachableProcessor(signatureJobForMetadata, p));
        }
    }
}
