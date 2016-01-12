using System.Text;
using Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects;

namespace Digipost.Signature.Api.Client.Core.Asice.AsiceManifest
{
    public class ManifestGenerator
    {
        public static byte[] GenerateManifestBytes(Manifest manifest)
        {
            var manifestDataTranferObject = DataTransferObjectConverter.ToDataTransferObject(manifest);
            var serializedManifest = SerializeUtility.Serialize(manifestDataTranferObject);

            return Encoding.UTF8.GetBytes(serializedManifest);
        }
    }
}
