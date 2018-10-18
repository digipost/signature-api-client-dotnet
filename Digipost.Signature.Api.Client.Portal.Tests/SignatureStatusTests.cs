using System.Linq;
using System.Reflection;
using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class SignatureStatusTests
    {
        public class ConstructorMethod : SignatureStatusTests
        {
            [Fact]
            public void Initializes_with_properties()
            {
                //Arrange
                var identifier = "IDENTIFIER";

                //Act
                var signatureStatus = new SignatureStatus(identifier);

                //Assert
                Assert.Equal(signatureStatus.Identifier, identifier);
            }
        }
    }

    public class KnownStatusesProperty : SignatureStatusTests
    {
        [Fact]
        public void Contains_all_known_statuses()
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
                Assert.True(SignatureStatus.KnownStatuses.Contains(status), "Status not contained in known statuses");
            }
        }
    }

    public class EqualsMethod : SignatureStatusTests
    {
        [Fact]
        public void Returns_false_on_difference()
        {
            //Arrange
            const string identifier1 = "IDENTIFIER1";
            const string identifier2 = "IDENTIFIER2";

            //Act
            var signatureStatus1 = new SignatureStatus(identifier1);
            var signatureStatus2 = new SignatureStatus(identifier2);

            //Assert
            Assert.False(signatureStatus1.Equals(signatureStatus2));
            Assert.False(signatureStatus2.Equals(signatureStatus1));
            Assert.False(signatureStatus1.Equals(null));
        }

        [Fact]
        public void Returns_true_on_equality()
        {
            //Arrange
            const string identifier = "IDENTIFIER";

            //Act
            var signatureStatus1 = new SignatureStatus(identifier);
            var signatureStatus2 = new SignatureStatus(identifier);

            //Assert
            Assert.True(signatureStatus1.Equals(signatureStatus2));
            Assert.True(signatureStatus2.Equals(signatureStatus1));
            Assert.True(signatureStatus1.Equals(signatureStatus1));
        }
    }
}
