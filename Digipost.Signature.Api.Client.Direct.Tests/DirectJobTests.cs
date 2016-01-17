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
                var sender = new Sender("012345678910");
                var id = "IdDirectJob";
                var signer = new Signer("01013300001");
                var document = DomainUtility.GetDocument();
                var exitUrls = new ExitUrls(
                    new Uri("http://localhost.completed"), 
                    new Uri("http://localhost.cancelled"),
                    new Uri("http://localhost.error")
                    );

                //Act
                DirectJob directJob = new DirectJob(sender, document, signer, id, exitUrls);
                
                //Assert
                Assert.AreEqual(sender, directJob.Sender);
                Assert.AreEqual(id, directJob.Reference);
                Assert.AreEqual(signer, directJob.Signer);
                Assert.AreEqual(document, directJob.Document);
                Assert.AreEqual(exitUrls, directJob.ExitUrls);
            }
        }

    }
}