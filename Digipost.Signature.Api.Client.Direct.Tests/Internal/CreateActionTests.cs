using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Internal;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests.Internal
{
    [TestClass]
    public class CreateActionTests
    {
        [TestClass]
        public class ConstructorMethod : CreateActionTests
        {
            [TestMethod]
            public void InitializesClassAndParentProperties()
            {
                //Arrange
                var businessCertificate = CoreDomainUtility.GetTestCertificate();
                var clientConfiguration = CoreDomainUtility.GetClientConfiguration();
                var directJob = new Job(DomainUtility.GetDirectDocument(), DomainUtility.GetSigner(), "reference", DomainUtility.GetExitUrls(), CoreDomainUtility.GetSender());
                var serializedJob = SerializeUtility.Serialize(DataTransferObjectConverter.ToDataTransferObject(directJob));

                var asiceBundle = DirectAsiceGenerator.CreateAsice(directJob, businessCertificate, clientConfiguration);

                //Act
                var action = new DirectCreateAction(directJob, asiceBundle);

                //Assert
                Assert.AreEqual(directJob, action.RequestContent);
                Assert.AreEqual(serializedJob, action.RequestContentXml.InnerXml);

                Assert.AreEqual(null, action.MultipartFormDataContent);
            }
        }
    }
}