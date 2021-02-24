using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Core.Tests.Utilities;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Direct.Internal;
using Digipost.Signature.Api.Client.Direct.Internal.AsicE;
using Digipost.Signature.Api.Client.Direct.Tests.Utilities;
using Xunit;

namespace Digipost.Signature.Api.Client.Direct.Tests.Internal
{
    public class CreateActionTests
    {
        public class ConstructorMethod : CreateActionTests
        {
            [Fact]
            public void InitializesClassAndParentProperties()
            {
                //Arrange
                var businessCertificate = CoreDomainUtility.GetTestCertificate();
                var clientConfiguration = CoreDomainUtility.GetClientConfiguration();
                var directJob = new Job("Job title", DomainUtility.GetSingleDirectDocument(), DomainUtility.GetSigner(), "reference", DomainUtility.GetExitUrls(), CoreDomainUtility.GetSender());
                var serializedJob = SerializeUtility.Serialize(DataTransferObjectConverter.ToDataTransferObject(directJob));

                var asiceBundle = DirectAsiceGenerator.CreateAsice(directJob, businessCertificate, clientConfiguration);

                //Act
                var action = new CreateAction(directJob, asiceBundle);

                //Assert
                Assert.Equal(directJob, action.RequestContent);
                Assert.Equal(serializedJob, action.RequestContentXml.InnerXml);

                Assert.Null(action.MultipartFormDataContent);
            }
        }
    }
}
