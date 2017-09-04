using System;
using System.Collections.Generic;
using Digipost.Signature.Api.Client.Core;
using Digipost.Signature.Api.Client.Core.Identifier;
using Digipost.Signature.Api.Client.Portal.Enums;
using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests
{
    public class JobStatusChangedTests
    {
        public class ConstructorMethod : JobStatusChangedTests
        {
            [Fact]
            public void Simple_constructor()
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
                        Identifier = new PersonalIdentificationNumber("123456789"),
                        XadesReference = new XadesReference(new Uri("http://xadesuri1.no"))
                    },
                    new Signature
                    {
                        SignatureStatus = SignatureStatus.Waiting,
                        Identifier = new PersonalIdentificationNumber("123456789"),
                        XadesReference = null
                    }
                };

                //Act
                var portalJobStatusChangeResponse = new JobStatusChanged(
                    jobId,
                    jobStatus,
                    confirmationReference,
                    signatures);

                //Assert
                Assert.Equal(jobId, portalJobStatusChangeResponse.JobId);
                Assert.Equal(jobStatus, portalJobStatusChangeResponse.Status);
                Assert.Equal(confirmationReference, portalJobStatusChangeResponse.ConfirmationReference);
                Assert.Equal(signatures, portalJobStatusChangeResponse.Signatures);
            }
        }
    }
}