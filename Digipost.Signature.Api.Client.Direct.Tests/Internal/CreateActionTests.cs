using Digipost.Signature.Api.Client.Core.Asice;
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
                var sender = CoreDomainUtility.GetSender();
                var document = CoreDomainUtility.GetDocument();
                var enumerable = CoreDomainUtility.GetSigner();
                var businessCertificate = CoreDomainUtility.GetTestCertificate();
                var directJob = DomainUtility.GetDirectJob();
                var serializedDirectJob = SerializeUtility.Serialize(DataTransferObjectConverter.ToDataTransferObject(directJob));

                var asiceBundle = AsiceGenerator.CreateAsice(sender, document, enumerable, businessCertificate);

                //Act
                var action = new DirectCreateAction(directJob, asiceBundle);
                
                //Assert
                Assert.AreEqual(directJob, action.RequestContent);
                Assert.AreEqual(serializedDirectJob, action.RequestContentXml.InnerXml);

                Assert.AreEqual(null, action.MultipartFormDataContent);
            }
        }
    }
}