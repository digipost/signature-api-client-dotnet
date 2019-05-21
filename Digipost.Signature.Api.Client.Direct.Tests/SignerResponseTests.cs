using System;
using Digipost.Signature.Api.Client.Core.Identifier;
using Schemas;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class SignerResponseTests
    {
        [Fact]
        public void Create_from_personal_identification_number()
        {
            var source = new directsignerresponse
            {
                redirecturl = "http://example.com/redirect",
                Item = "01013300001",
                ItemElementName = ItemChoiceType1.personalidentificationnumber
            };
            var expected = new SignerResponse(new PersonalIdentificationNumber("01013300001"), new Uri("http://example.com/redirect"));

            var actual = new SignerResponse(source);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_from_identifier()
        {
            var source = new directsignerresponse
            {
                redirecturl = "http://example.com/redirect",
                Item = "Some Custom Identifier",
                ItemElementName = ItemChoiceType1.signeridentifier
            };
            var expected = new SignerResponse(new CustomIdentifier("Some Custom Identifier"), new Uri("http://example.com/redirect"));

            var actual = new SignerResponse(source);

            Assert.Equal(expected, actual);
        }
    }
}
