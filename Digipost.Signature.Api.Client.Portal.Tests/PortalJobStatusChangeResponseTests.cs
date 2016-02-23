using System;
using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Portal.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class PortalJobStatusChangeResponseTests
    {
        [TestClass]
        public class ConstructorMethod : PortalJobStatusChangeResponseTests
        {
            [TestMethod]
            public void SimpleConstructor()
            {
                //Arrange
                var jobId = 123456789;
                var jobStatus = JobStatus.PartiallyCompleted;
                var confirmationReference = new ConfirmationReference(new Uri("http://confirmationreference.no"));
                var signatures = new List<Signature>
                {
                    new Signature
                    {
                        SignatureStatus = SignatureStatus.Signed,
                        Signer = new Signer("123456789"),
                        XadesReference = new XadesReference(new Uri("http://xadesuri1.no"))
                    },
                    new Signature
                    {
                        SignatureStatus = SignatureStatus.Waiting,
                        Signer = new Signer("123456789"),
                        XadesReference = null
                    }
                };

                //Act
                var portalJobStatusChangeResponse = new PortalJobStatusChangeResponse(
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