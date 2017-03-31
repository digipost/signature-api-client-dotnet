using Digipost.Signature.Api.Client.Portal.Enums;
using Digipost.Signature.Api.Client.Portal.Extensions;
using Digipost.Signature.Api.Client.Scripts.XsdToCode.Code;
using Xunit;

namespace Digipost.Signature.Api.Client.Portal.Tests.Extensions
{
    public class EnumExtensionsTests
    {
        public class ToPortalJobExtension : EnumExtensionsTests
        {
            [Fact]
            public void Converts_all_portal_job_enum_values()
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
                Assert.Equal(expectedPartiallyCompleted, partiallyCompleted);
                Assert.Equal(expectedCompleted, completed);
                Assert.Equal(expectedFailed, failed);
            }
        }
    }
}