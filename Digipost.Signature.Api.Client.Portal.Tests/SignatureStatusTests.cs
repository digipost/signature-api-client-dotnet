using System.Linq;
using System.Reflection;
using Digipost.Signature.Api.Client.Portal.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    [TestClass()]
    public class SignatureStatusTests
    {
        [TestClass]
        public class ConstructorMethod : SignatureStatusTests
        {
            [TestMethod]
            public void InitializesWithProperties()
            {
                //Arrange
                var identifier = "IDENTIFIER";

                //Act
                var signatureStatus = new SignatureStatus(identifier);
                
                //Assert
                Assert.AreEqual(signatureStatus.Identifier, identifier);
            }
        }
    }

    [TestClass]
    public class KnownStatusesProperty: SignatureStatusTests
    {
        [TestMethod]
        public void ContainsAllKnownStatuses()
        {
            //Arrange
            var knownStatusesInClass = new SignatureStatus("lolzor")
                .GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof (SignatureStatus))
                .Select(f =>(SignatureStatus) f.GetValue(null));

            //Act

            //Assert
            foreach (var status in knownStatusesInClass)
            {
                Assert.IsTrue(SignatureStatus.KnownStatuses.Contains(status));
            }
        }
    }
}