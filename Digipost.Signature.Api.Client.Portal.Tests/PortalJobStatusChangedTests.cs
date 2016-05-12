using System;
using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class PortalJobStatusChangedTests
    {
        [TestClass]
        public class ConstructorMethod : PortalJobStatusChangedTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var jobId = 123456789;
                var jobStatus = JobStatus.InProgress;
                var confirmationReference = new ConfirmationReference(new Uri("http://confirmationreference.no"));
                var signatures = new List<Signature>
                {
                    new Signature
                    {
                        SignatureStatus = SignatureStatus.Signed,
                        Signer = new PortalSigner("123456789", new NotificationsUsingLookup()), //TODO: Skal ikke bruke lookup
                        XadesReference = new XadesReference(new Uri("http://xadesuri1.no"))
                    },
                    new Signature
                    {
                        SignatureStatus = SignatureStatus.Waiting,
                        Signer = new PortalSigner("123456789", new NotificationsUsingLookup()), //TODO: Skal ikke bruke lookup
                        XadesReference = null
                    }
                };

                //Act
                var portalJobStatusChangeResponse = new PortalJobStatusChanged(
                    jobId,
                    jobStatus,
                    confirmationReference,
                    signatures);

                //Assert
                Assert.AreEqual(jobId, portalJobStatusChangeResponse.JobId);
                Assert.AreEqual(jobStatus, portalJobStatusChangeResponse.Status);
                Assert.AreEqual(confirmationReference, portalJobStatusChangeResponse.ConfirmationReference);
                Assert.AreEqual(signatures, portalJobStatusChangeResponse.Signatures);
            }
        }
    }
}