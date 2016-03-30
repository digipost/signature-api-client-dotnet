using System.Collections.Generic;

namespace Digipost.Signature.Api.Client.Core
{
    public interface IAsiceConfiguration
    {
        IEnumerable<IDocumentBundleProcessor> DocumentBundleProcessors { get; set; }
    }
}