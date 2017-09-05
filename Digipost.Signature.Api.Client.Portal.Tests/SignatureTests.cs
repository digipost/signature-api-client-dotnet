using System;
using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class SignatureTests
    {
        [Fact]
        public void FromXmlObjectWithNotifications()
        {
            var dateTime = DateTime.Now;
            var source = new signature
            {
                Item = new notifications
                {
                    Items = new object[]
                    {
                        new sms {number = "11111111"},
                        new email {address = "email@example.com"}
                    }
                },
                status = new signaturestatus
                {
                    since = dateTime,
                    Value = "REJECTED"
                },
                xadesurl = "http://xadesurl.example.com"
            };

            var signer = new Signature(source);

            Assert.True(signer.Identifier.IsContactInformation());
            Assert.Equal(dateTime, signer.DateTimeForStatus);
            Assert.Equal(source.status.Value, signer.SignatureStatus.Identifier);
            Assert.Equal(source.xadesurl, signer.XadesReference.Url.OriginalString);
        }

        [Fact]
        public void FromXmlObjectWithPersonalIdentificationNumber()
        {
            var dateTime = DateTime.Now;
            var source = new signature
            {
                Item = "01013300001"
                ,
                status = new signaturestatus
                {
                    since = dateTime,
                    Value = "REJECTED"
                },
                xadesurl = "http://xadesurl.example.com"
            };

            var signer = new Signature(source);

            Assert.True(signer.Identifier.IsPersonalIdentificationNumber());
            Assert.Equal(dateTime, signer.DateTimeForStatus);
            Assert.Equal(source.status.Value, signer.SignatureStatus.Identifier);
            Assert.Equal(source.xadesurl, signer.XadesReference.Url.OriginalString);
        }
    }
}