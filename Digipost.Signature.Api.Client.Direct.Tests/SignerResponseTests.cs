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
            var redirectUrl = "http://example.com/redirect";
            var personalIdentificationNumber = "01013300001";
            var signerUrl = "http://example.com/signer/1";
            var source = new directsignerresponse
            {
                redirecturl = redirectUrl,
                Item = personalIdentificationNumber,
                ItemElementName = ItemChoiceType1.personalidentificationnumber,
                href = signerUrl
            };
            var expected = new SignerResponse(new PersonalIdentificationNumber(personalIdentificationNumber), new Uri(redirectUrl), new Uri(signerUrl));

            var actual = new SignerResponse(source);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Create_from_identifier()
        {
            const string redirectUrl = "http://example.com/redirect";
            const string customIdentifier = "Some Custom Identifier";
            const string signerUrl = "http://example.com/signer/1";

            var source = new directsignerresponse
            {
                redirecturl = redirectUrl,
                Item = customIdentifier,
                ItemElementName = ItemChoiceType1.signeridentifier,
                href = signerUrl
            };
            var expected = new SignerResponse(new CustomIdentifier(customIdentifier), new Uri(redirectUrl), new Uri(signerUrl));

            var actual = new SignerResponse(source);

            Assert.Equal(expected, actual);
        }
    }
}
