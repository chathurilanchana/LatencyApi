using LatencyService.Infrastructure.LatencyApi;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace LatencyService.UnitTests.Infrastructure
{
    public class LatencyServiceClientTests
    {
        [Fact]
        public async Task WhenApiReturnsSuccessCode_DataIsDeserialisedAsExpected()
        {
            var expectedLatencyRecord = new List<LatencyRecord> {
                new LatencyRecord { ServiceId=1, RequestId=2, MilliSecondsDelay=300 }
                };

            var mockedHttpClient = GetMockedHttpClient(expectedLatencyRecord);

            var latencyServiceClient = new LatencyServiceClient(mockedHttpClient);

            var result = (await latencyServiceClient.Get(DateTime.Today))?.ToList();

            result.ShouldNotBeNull();
            result.Count.ShouldBe(1);
            result[0].ServiceId.ShouldBe(expectedLatencyRecord[0].ServiceId);
            result[0].RequestId.ShouldBe(expectedLatencyRecord[0].RequestId);
            result[0].MilliSecondsDelay.ShouldBe(expectedLatencyRecord[0].MilliSecondsDelay);
        }

        [Fact]
        public async Task WhenApiReturnsUnsuccessStatusCode_ExceptionThrown()
        {
            var mockedHttpClient = GetMockedHttpClient(null, HttpStatusCode.BadRequest);

            var latencyServiceClient = new LatencyServiceClient(mockedHttpClient);

            await Assert.ThrowsAsync<Exception>(async () => await latencyServiceClient.Get(DateTime.Today));
        }

        public HttpClient GetMockedHttpClient(List<LatencyRecord> expectedRecords, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(JsonSerializer.Serialize(expectedRecords))
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            return client;
        }
    }
}
