using Digipost.Signature.Api.Client.Portal.Enums;
using Digipost.Signature.Api.Client.Portal.Extensions;
using Digipost.Signature.Api.Client.Scripts.XsdToCode.Code;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Portal.Tests.Extensions
{
    [TestClass]
    public class EnumExtensionsTests
    {
        [TestClass]
        public class ToPortalJobExtension : EnumExtensionsTests
        {
            [TestMethod]
            public void ConvertsAllPortalJobEnumValues()
            {
                //Arrange
                var partiallyCompletedSource = portalsignaturejobstatus.IN_PROGRESS;
                var completedSource = portalsignaturejobstatus.COMPLETED_SUCCESSFULLY;
                var failedSource = portalsignaturejobstatus.FAILED;

                var expectedPartiallyCompleted = JobStatus.InProgress;
                var expectedCompleted = JobStatus.CompletedSuccessfully;
                var expectedFailed = JobStatus.Failed;

                //Act
                var partiallyCompleted = partiallyCompletedSource.ToJobStatus();
                var completed = completedSource.ToJobStatus();
                var failed = failedSource.ToJobStatus();

                //Assert
                Assert.AreEqual(expectedPartiallyCompleted, partiallyCompleted);
                Assert.AreEqual(expectedCompleted, completed);
                Assert.AreEqual(expectedFailed, failed);
            }
        }
    }
}