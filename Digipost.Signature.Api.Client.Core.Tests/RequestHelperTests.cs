using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Digipost.Signature.Api.Client.Portal.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass]
    public class RequestHelperTests
    {
        [TestClass]
        public class DoStreamRequestMethod : RequestHelperTests
        {
            [TestMethod]
            public async Task ReturnsStreamOnSuccessStatusCode()
            {
                //Arrange
                var internalServerErrorHandler = new FakeHttpClientForPadesResponse();
                var requestHelper = new RequestHelper(new HttpClient(internalServerErrorHandler));

                //Act
                var result = await requestHelper.DoStreamRequest(new Uri("http://fakeUri.no"));

                //Assert
                Assert.IsTrue(result.Length > 5000);
            }

            [TestMethod]
            [ExpectedException(typeof(SignatureException), AllowDerivedTypes = true)]
            public async Task ThrowsSignatureExceptionIfNotSuccessStatusCode()
            {
                //Arrange
                var internalServerErrorHandler = new FakeHttpClientHandlerForInternalServerErrorResponse();
                var requestHelper = new RequestHelper(new HttpClient(internalServerErrorHandler));

                //Act
                var result = await requestHelper.DoStreamRequest(new Uri("http://fakeUri.no"));
                
                //Assert
                Assert.Fail();
            }
        }

        [TestClass]
        public class HandleGeneralErrorMethod : RequestHelperTests
        {
            [TestMethod]
            public async Task ThrowsBrokerNotAuthorizedExceptionOnNotAuthorized()
            {
                //Arrange
                var brokerNotAuthorizedResponse = new FakeHttpClientHanderForBrokerNotAuthorizedErrorResponse();
                var requestHelper = new RequestHelper(new HttpClient(brokerNotAuthorizedResponse));

                //Act
                var exception = requestHelper.HandleGeneralException(await brokerNotAuthorizedResponse.GetContent().ReadAsStringAsync(), brokerNotAuthorizedResponse.ResultCode.Value);
                
                //Assert
                Assert.IsInstanceOfType(exception, typeof(BrokerNotAuthorizedException));
            }

            [TestMethod]
            public async Task ReturnsUnexpectedResponseExceptionWithErrorAndCodeOnXmlErrorResult()
            {
                //Arrange
                var errorResponse = new FakeHttpClientHandlerForErrorResponse();
                var requestHelper = new RequestHelper(new HttpClient(errorResponse));

                //Act
                var exception = requestHelper.HandleGeneralException(await errorResponse.GetContent().ReadAsStringAsync(), errorResponse.ResultCode.Value);
                var unexpectedResponseException = (UnexpectedResponseException) exception;

                //Assert
                Assert.IsNotNull(unexpectedResponseException.Error);
                Assert.AreEqual(errorResponse.ResultCode, unexpectedResponseException.StatusCode);
            }

            [TestMethod]
            public async Task ReturnsUnexpectedResponseExceptionWithoutErrorAndCodeOnNotXmlResult()
            {
                //Arrange
                var internalServerErrorResponse = new FakeHttpClientHandlerForInternalServerErrorResponse();
                var requestHelper = new RequestHelper(new HttpClient(internalServerErrorResponse));

                //Act
                var exception = requestHelper.HandleGeneralException(await internalServerErrorResponse.GetContent().ReadAsStringAsync(), internalServerErrorResponse.ResultCode.Value);
                var unexpectedResponseException = (UnexpectedResponseException)exception;

                //Assert
                Assert.IsNull(unexpectedResponseException.Error);
                Assert.AreEqual(internalServerErrorResponse.ResultCode, unexpectedResponseException.StatusCode);
                
            }

        }
    }
}