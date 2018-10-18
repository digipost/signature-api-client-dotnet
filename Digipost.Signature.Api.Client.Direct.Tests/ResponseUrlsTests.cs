using System;
using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core.Identifier;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class ResponseurlsTests
    {
        public class ConstructorMethod : ResponseurlsTests
        {
            [Fact]
            public void Simple_constructor()
            {
                //Arrange
                var redirectUrls = new List<RedirectReference> {new RedirectReference(new Uri("http://responseurl.no"), new PersonalIdentificationNumber("12345678910"))};
                var statusUrl = new Uri("http://statusurl.no");

                //Act
                var responseUrls = new ResponseUrls(
                    redirectUrls,
                    statusUrl
                );

                //Assert
                Assert.Equal(redirectUrls, responseUrls.Redirect.Urls);
            }
        }

        public class StatusMethod : ResponseurlsTests
        {
            [Fact]
            public void Returns_status_not_null()
            {
                //Arrange
                var redirectUrl = new List<RedirectReference> {new RedirectReference(new Uri("http://responseurl.no"), new PersonalIdentificationNumber("12345678910"))};
                var statusUrl = new Uri("http://statusurl.no");
                var statusQueryToken = "StatusQueryToken";

                //Act
                var responseUrls = new ResponseUrls(
                    redirectUrl,
                    statusUrl
                );

                //Assert
                Assert.NotNull(responseUrls.Status(statusQueryToken));
            }
        }
    }
}
