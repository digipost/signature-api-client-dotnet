using Digipost.Signature.Api.Client.Core.Tests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass]
    public class AbstractSignerTests
    {
        [TestClass]
        public class ConstructorMethod : AbstractSignerTests
        {
            [TestMethod]
            public void InitializesWithProperties()
            {
                //Arrange
                var personalIdentificationNumber = new PersonalIdentificationNumber("01013300001");

                //Act
                var signer = new SignerStub(personalIdentificationNumber);

                //Assert
                Assert.AreEqual(personalIdentificationNumber, signer.PersonalIdentificationNumber);
            }
        }
    }
}