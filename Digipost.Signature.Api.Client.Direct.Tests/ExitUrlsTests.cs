using System;
using Digipost.Signature.Api.Client.Direct;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.DirectTests
{
    [TestClass]
    public class ExitUrlsTests
    {
        [TestClass]
        public class ConstructorMethod : ExitUrlsTests
        {
            [TestMethod]
            public void SimpleContructor()
            {
                //Arrange
                var completionUrl = new Uri("http://localhost/completion");
                var cancellationUrl = new Uri("http://localhost/cancellation");
                var errorUrl = new Uri("http://localhost/error");

                //Act
                var exitUrls = new ExitUrls(completionUrl, cancellationUrl, errorUrl);
                
                //Assert
                Assert.AreEqual(completionUrl, exitUrls.CompletionUrl);
                Assert.AreEqual(cancellationUrl, exitUrls.CancellationUrl);
                Assert.AreEqual(errorUrl, exitUrls.ErrorUrl);
            }
        }
    }
}