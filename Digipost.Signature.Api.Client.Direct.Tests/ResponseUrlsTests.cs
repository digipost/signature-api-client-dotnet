using System;
using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    public class ResponseurlsTests
    {
        [TestClass]
        public class ConstructorMethod : ResponseurlsTests
        {
            [TestMethod]
            public void SimpleConstructor()
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
                Assert.AreEqual(redirectUrls, responseUrls.Redirect.Urls);
            }
        }

        [TestClass]
        public class StatusMethod : ResponseurlsTests
        {
            [TestMethod]
            public void ReturnsStatusNotNull()
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
                Assert.IsNotNull(responseUrls.Status(statusQueryToken));
            }
        }
    }
}