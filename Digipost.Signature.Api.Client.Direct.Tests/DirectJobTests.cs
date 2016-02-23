using System;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Direct.Tests
{
    [TestClass]
    public class DirectJobTests
    {
        [TestClass]
        public class ConstructorMethod : DirectJobTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var id = "IdDirectJob";
                var signer = new Signer("01013300001");
                var document = CoreDomainUtility.GetDocument();
                var exitUrls = new ExitUrls(
                    new Uri("http://localhost.completed"),
                    new Uri("http://localhost.cancelled"),
                    new Uri("http://localhost.error")
                    );

                //Act
                var directJob = new DirectJob(document, signer, id, exitUrls);

                //Assert
                Assert.AreEqual(id, directJob.Reference);
                Assert.AreEqual(signer, directJob.Signer);
                Assert.AreEqual(document, directJob.Document);
                Assert.AreEqual(exitUrls, directJob.ExitUrls);
            }
        }
    }
}