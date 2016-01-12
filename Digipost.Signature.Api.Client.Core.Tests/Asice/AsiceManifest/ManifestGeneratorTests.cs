using System.Text;
using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Asice.AsiceManifest;
using Digipost.Signature.Api.Client.Core.Asice.DataTransferObjects;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Asice.AsiceManifest
{
    [TestClass]
    public class ManifestGeneratorTests
    {
        [TestClass]
        public class GenerateManifestMethod : ManifestGeneratorTests
        {
            [TestMethod]
            public void SuccessfulManifestToBytes()
            {
                //Arrange
                var manifest = DomainUtility.GetManifest();
                var manifestDataTranferObject = DataTransferObjectConverter.ToDataTransferObject(manifest);
                var expectedResult = SerializeUtility.Serialize(manifestDataTranferObject);


                //Act
                var bytes = ManifestGenerator.GenerateManifestBytes(manifest);
                var actualResult = Encoding.UTF8.GetString(bytes);

                //Assert
                Assert.AreEqual(expectedResult, actualResult);
            } 
        }
    }
}