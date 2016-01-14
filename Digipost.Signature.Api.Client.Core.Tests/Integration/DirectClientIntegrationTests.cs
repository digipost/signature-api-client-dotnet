using System;
using System.Linq;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests.Integration
{
    [TestClass]
    public class DirectClientIntegrationTests
    {
        [TestClass]
        public class CreateMethod : DirectClientIntegrationTests
        {
            [TestMethod]
            public void RunsWithoutExceptions()
            {
                //Arrange
               var directClient = new DirectClient(new ClientConfiguration(new Uri("http://serviceRoot.no"), DomainUtility.GetSender(), DomainUtility.GetCertificate()));

                //Act
                directClient.Create(DomainUtility.GetDirectJob());


                //Assert

            }
        }

    }
}
