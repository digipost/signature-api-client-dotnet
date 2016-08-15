using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    [TestClass]
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
    public class KnownStatusesProperty : SignatureStatusTests
    {
        [TestMethod]
        public void ContainsAllKnownStatuses()
        {
            //Arrange
            var knownStatusesInClass = new SignatureStatus("lolzor")
                .GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(SignatureStatus))
                .Select(f => (SignatureStatus) f.GetValue(null));

            //Act

            //Assert
            foreach (var status in knownStatusesInClass)
            {
                Assert.IsTrue(SignatureStatus.KnownStatuses.Contains(status), "Status not contained in known statuses");
            }
        }
    }

    [TestClass]
    public class EqualsMethod : SignatureStatusTests
    {
        [TestMethod]
        public void ReturnsTrueOnEquality()
        {
            //Arrange
            const string identifier = "IDENTIFIER";

            //Act
            var signatureStatus1 = new SignatureStatus(identifier);
            var signatureStatus2 = new SignatureStatus(identifier);

            //Assert
            Assert.IsTrue(signatureStatus1.Equals(signatureStatus2));
            Assert.IsTrue(signatureStatus2.Equals(signatureStatus1));
            Assert.IsTrue(signatureStatus1.Equals(signatureStatus1));
        }

        [TestMethod]
        public void ReturnsFalseOnDifference()
        {
            //Arrange
            const string identifier1 = "IDENTIFIER1";
            const string identifier2 = "IDENTIFIER2";

            //Act
            var signatureStatus1 = new SignatureStatus(identifier1);
            var signatureStatus2 = new SignatureStatus(identifier2);

            //Assert
            Assert.IsFalse(signatureStatus1.Equals(signatureStatus2));
            Assert.IsFalse(signatureStatus2.Equals(signatureStatus1));
            Assert.IsFalse(signatureStatus1.Equals(null));
        }
    }
}