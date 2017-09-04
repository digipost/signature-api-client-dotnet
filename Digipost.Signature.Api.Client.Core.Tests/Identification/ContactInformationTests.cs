using System;
using System.Linq;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Scripts.XsdToCode.Code;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests.Identification
{
    public class ContactInformationTests
    {

        [Fact]
        public void FromXmlObject()
        {
            const string number = "11111111";
            const string address = "email@example.com";
            var source = new notifications
            {
                Items = new object[]
                {
                    new sms {number = number},
                    new email {address = address}
                }
            };

            var actual = new ContactInformation(source);

            Assert.Equal(number, actual.Sms.Number);
            Assert.Equal(address, actual.Email.Address);
        }

        [Fact]
        public void ThrowsExceptionOnIllegalNotifications()
        {
            const string address = "email@example.com";
            var source = new notifications
            {
                Items = new object[]
                {
                    new email {address = address},
                    new email {address = address}
                }
            };

            var argumentException = Assert.Throws<ArgumentException>(() => new ContactInformation(source));
            Assert.True(argumentException.Message.Contains("Unable to create ContactInformation"));
        }

    }
}
