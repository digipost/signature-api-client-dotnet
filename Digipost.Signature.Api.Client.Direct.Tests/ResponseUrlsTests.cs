using System;
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
                var redirectUrl = new Uri("http://responseurl.no");
                var statusUrl = new Uri("http://statusurl.no");
                
                //Act
                var responseUrls = new ResponseUrls(
                    redirectUrl,
                    statusUrl
                    );

                //Assert
                Assert.AreEqual(redirectUrl, responseUrls.Redirect);
                Assert.AreEqual(statusUrl, responseUrls.Status);
            } 
        }
    }
}