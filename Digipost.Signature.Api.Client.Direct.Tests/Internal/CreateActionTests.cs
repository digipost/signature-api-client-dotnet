using Digipost.Signature.Api.Client.Core.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Internal;
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
                var sender = DomainUtility.GetSender();
                var document = DomainUtility.GetDocument();
                var enumerable = DomainUtility.GetSigners(1);
                var businessCertificate = DomainUtility.GetTestCertificate();
                var signatureServiceRoot = DomainUtility.GetSignatureServiceRoot();
                var directJob = DomainUtility.GetDirectJob();
                var serializedDirectJob = SerializeUtility.Serialize(DataTransferObjectConverter.ToDataTransferObject(directJob));

                var asiceBundle = AsiceGenerator.CreateAsice(sender, document, enumerable, businessCertificate);

                //Act
                var action = new CreateAction(
                        directJob,
                        asiceBundle,
                        businessCertificate,
                        signatureServiceRoot
                    );

                //Assert
                Assert.AreEqual(directJob, action.RequestContent);
                Assert.AreEqual(businessCertificate, action.BusinessCertificate);
                Assert.AreEqual(signatureServiceRoot, action.SignatureServiceRoot);

                Assert.AreEqual(serializedDirectJob, action.RequestContentXml.InnerXml);
            }
        }
    }
}