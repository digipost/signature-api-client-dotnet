using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    [TestClass]
    public class RequestHelperTests
    {
        [TestClass]
        public class DoPostMethod : RequestHelperTests
        {
            [TestMethod]
            public async Task DeserializesClassOnOkResponse()
            {
                //Arrange
                var fakeHandler = new FakeHttpClientForDataResponse();
                var responseData = await fakeHandler.GetContent().ReadAsStringAsync();
                var expectedResponseData = $"Appended{responseData}";
                Func<string, string> deserializeFunc = data => $"Appended{data}";
                var requestHelper = new RequestHelper(new HttpClient(fakeHandler));

                //Act
                var actualResponseData = await requestHelper.Create(new Uri("http://fakeuri.no"), new StringContent("SomePostData"), deserializeFunc);

                //Assert
                Assert.AreEqual(expectedResponseData, actualResponseData);
            }

            [TestMethod]
            [ExpectedException(typeof(SignatureException), AllowDerivedTypes = true)]
            public async Task ThrowsGeneralErrorOnNotSuccessResponse()
            {
                //Arrange
                var fakeHandler = new FakeHttpClientHandlerForInternalServerErrorResponse();
                var requestHelper = new RequestHelper(new HttpClient(fakeHandler));

                //Act
                Func<string, string> deserializeFunc = data => $"Appended{data}";
                await requestHelper.Create(new Uri("http://fakeUri.no"), new StringContent("SomePostData"), deserializeFunc);

                //Assert
                Assert.Fail();
            }
        }

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
                var result = await requestHelper.GetStream(new Uri("http://fakeUri.no"));

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
                var result = await requestHelper.GetStream(new Uri("http://fakeUri.no"));

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
                var unexpectedResponseException = (UnexpectedResponseException) exception;

                //Assert
                Assert.IsNull(unexpectedResponseException.Error);
                Assert.AreEqual(internalServerErrorResponse.ResultCode, unexpectedResponseException.StatusCode);
            }
        }
    }
}