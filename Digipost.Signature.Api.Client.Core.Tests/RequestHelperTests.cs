using System;
using System.Net.Http;
using System.Threading.Tasks;
using Digipost.Signature.Api.Client.Core.Exceptions;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Digipost.Signature.Api.Client.Core.Tests
{
    public class RequestHelperTests
    {
        public class DoPostMethod : RequestHelperTests
        {
            [Fact]
            public async Task Deserializes_class_on_ok_response()
            {
                //Arrange
                var fakeHandler = new FakeHttpClientForDataResponse();
                var responseData = await fakeHandler.GetContent().ReadAsStringAsync().ConfigureAwait(false);
                var expectedResponseData = $"Appended{responseData}";
                Func<string, string> deserializeFunc = data => $"Appended{data}";
                var requestHelper = new RequestHelper(new HttpClient(fakeHandler), new NullLoggerFactory());

                //Act
                var actualResponseData = await requestHelper.Create(new Uri("http://fakeuri.no"), new StringContent("SomePostData"), deserializeFunc).ConfigureAwait(false);

                //Assert
                Assert.Equal(expectedResponseData, actualResponseData);
            }

            [Fact]
            public async Task Throws_general_exception_error_on_not_success_response()
            {
                //Arrange
                var fakeHandler = new FakeHttpClientHandlerForInternalServerErrorResponse();
                var requestHelper = new RequestHelper(new HttpClient(fakeHandler), new NullLoggerFactory());

                //Act
                Func<string, string> deserializeFunc = data => $"Appended{data}";
                await Assert.ThrowsAsync<UnexpectedResponseException>(async () => await requestHelper.Create(new Uri("http://fakeUri.no"), new StringContent("SomePostData"), deserializeFunc).ConfigureAwait(false)).ConfigureAwait(false);
            }
        }

        public class DoStreamRequestMethod : RequestHelperTests
        {
            [Fact]
            public async Task Returns_stream_on_success_status_code()
            {
                //Arrange
                var internalServerErrorHandler = new FakeHttpClientForPadesResponse();
                var requestHelper = new RequestHelper(new HttpClient(internalServerErrorHandler), new NullLoggerFactory());

                //Act
                var result = await requestHelper.GetStream(new Uri("http://fakeUri.no")).ConfigureAwait(false);

                //Assert
                Assert.True(result.Length > 5000);
            }

            [Fact]
            public async Task Throws_signature_exception_if_not_success_status_code()
            {
                //Arrange
                var internalServerErrorHandler = new FakeHttpClientHandlerForInternalServerErrorResponse();
                var requestHelper = new RequestHelper(new HttpClient(internalServerErrorHandler), new NullLoggerFactory());

                //Act
                await Assert.ThrowsAsync<UnexpectedResponseException>(async () => await requestHelper.GetStream(new Uri("http://fakeUri.no")).ConfigureAwait(false)).ConfigureAwait(false);
            }
        }

        public class HandleGeneralErrorMethod : RequestHelperTests
        {
            [Fact]
            public async Task Returns_unexpected_response_exception_with_error_and_code_on_xml_error_result()
            {
                //Arrange
                var errorResponse = new FakeHttpClientHandlerForErrorResponse();
                var requestHelper = new RequestHelper(new HttpClient(errorResponse), new NullLoggerFactory());

                //Act
                var exception = requestHelper.HandleGeneralException(errorResponse.ResultCode.Value, await errorResponse.GetContent().ReadAsStringAsync().ConfigureAwait(false));
                var unexpectedResponseException = (UnexpectedResponseException) exception;

                //Assert
                Assert.NotNull(unexpectedResponseException.Error);
                Assert.Equal(errorResponse.ResultCode, unexpectedResponseException.StatusCode);
            }

            [Fact]
            public async Task Returns_unexpected_response_exception_without_error_and_code_on_not_xml_result()
            {
                //Arrange
                var internalServerErrorResponse = new FakeHttpClientHandlerForInternalServerErrorResponse();
                var requestHelper = new RequestHelper(new HttpClient(internalServerErrorResponse), new NullLoggerFactory());

                //Act
                var exception = requestHelper.HandleGeneralException(internalServerErrorResponse.ResultCode.Value, await internalServerErrorResponse.GetContent().ReadAsStringAsync().ConfigureAwait(false));
                var unexpectedResponseException = (UnexpectedResponseException) exception;

                //Assert
                Assert.Null(unexpectedResponseException.Error);
                Assert.Equal(internalServerErrorResponse.ResultCode, unexpectedResponseException.StatusCode);
            }

            [Fact]
            public async Task Throws_broker_not_authorized_exception_on_not_authorized()
            {
                //Arrange
                var brokerNotAuthorizedResponse = new FakeHttpClientHanderForBrokerNotAuthorizedErrorResponse();
                var requestHelper = new RequestHelper(new HttpClient(brokerNotAuthorizedResponse), new NullLoggerFactory());

                //Act
                var exception = requestHelper.HandleGeneralException(brokerNotAuthorizedResponse.ResultCode.Value, await brokerNotAuthorizedResponse.GetContent().ReadAsStringAsync().ConfigureAwait(false));

                //Assert
                Assert.IsType<BrokerNotAuthorizedException>(exception);
            }
        }
    }
}
