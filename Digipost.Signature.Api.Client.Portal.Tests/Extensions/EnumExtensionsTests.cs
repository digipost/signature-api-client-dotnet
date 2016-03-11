using Digipost.Signature.Api.Client.Portal.Enums;
using Digipost.Signature.Api.Client.Portal.Extensions;
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
                var partiallyCompletedSource = portalsignaturejobstatus.PARTIALLY_COMPLETED;
                var completedSource = portalsignaturejobstatus.COMPLETED;

                var expectedPartiallyCompleted = JobStatus.PartiallyCompleted;
                var expectedCompleted = JobStatus.Completed;

                //Act
                var partiallyCompleted = partiallyCompletedSource.ToJobStatus();
                var completed = completedSource.ToJobStatus();

                //Assert
                Assert.AreEqual(expectedPartiallyCompleted, partiallyCompleted);
                Assert.AreEqual(expectedCompleted, completed);
            }
        }
    }
}