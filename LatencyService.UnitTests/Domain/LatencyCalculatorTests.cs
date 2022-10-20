using LatencyService.Domain;
using LatencyService.Infrastructure.LatencyApi;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace LatencyService.UnitTests.Domain
{
    public class LatencyCalculatorTests
    {
        private readonly Mock<ILatencyServiceClient> _latencyServiceClient;
        public LatencyCalculatorTests()
        {
            _latencyServiceClient = new Mock<ILatencyServiceClient>();
        }

        [Fact]
        public async Task AverageServiceDelayCalculation_Correct()
        {

            _latencyServiceClient.Setup(p => p.Get(It.IsAny<DateTime>())).ReturnsAsync(new List<LatencyRecord>() {
                new LatencyRecord { ServiceId=1, RequestId=1, MilliSecondsDelay=100 },
                new LatencyRecord { ServiceId=1, RequestId=2, MilliSecondsDelay=20 },
                new LatencyRecord { ServiceId=2, RequestId=3, MilliSecondsDelay=30 },
                new LatencyRecord { ServiceId=2, RequestId=4, MilliSecondsDelay=60 }
           });

            var latencyCalculator = GetSut();

            var result = await latencyCalculator.CalculateLatencies(new DateTime(2021, 1, 1), new DateTime(2021, 01, 03));

            result.Count.ShouldBe(2);
            var service1Latencies = result[1];
            service1Latencies.NumberOfRequest.ShouldBe(2*3);
            service1Latencies.CalculateAverageLatency().ShouldBe((120 / 2));

            var service2Latencies = result[2];
            service1Latencies.NumberOfRequest.ShouldBe(2 * 3);
            service1Latencies.CalculateAverageLatency().ShouldBe((90 / 2));
        }


        [Fact]
        public async Task GivenDuplicateRequest_WhenCalculatingAverage_DuplicatesAreSkipped()
        {

            _latencyServiceClient.Setup(p => p.Get(It.IsAny<DateTime>())).ReturnsAsync(new List<LatencyRecord>() {
                new LatencyRecord { ServiceId=1, RequestId=1, MilliSecondsDelay=100 },
                new LatencyRecord { ServiceId=1, RequestId=1, MilliSecondsDelay=100 },
                new LatencyRecord { ServiceId=1, RequestId=2, MilliSecondsDelay=20 },
                new LatencyRecord { ServiceId=2, RequestId=3, MilliSecondsDelay=30 },
                new LatencyRecord { ServiceId=2, RequestId=4, MilliSecondsDelay=60 }
           });

            var latencyCalculator = GetSut();

            var result = await latencyCalculator.CalculateLatencies(new DateTime(2021, 1, 1), new DateTime(2021, 01, 01));

            result.Count.ShouldBe(2);
            var service1Latencies = result[1];
            service1Latencies.NumberOfRequest.ShouldBe(2);
            service1Latencies.CalculateAverageLatency().ShouldBe(120 / 2);
        }

        [Fact]
        public async Task ApplicationDoesNotCrash_IfClientReturnsAnEmptyList()
        {
            _latencyServiceClient.SetupSequence(p => p.Get(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<LatencyRecord>() { })
                .ReturnsAsync(new List<LatencyRecord>() {
                    new LatencyRecord { ServiceId=1, RequestId=1, MilliSecondsDelay=100 },
                    new LatencyRecord { ServiceId=1, RequestId=2, MilliSecondsDelay=20 }
           });

            var latencyCalculator = GetSut();

            var result = await latencyCalculator.CalculateLatencies(new DateTime(2021, 1, 1), new DateTime(2021, 01, 02));
            
            result.Count.ShouldBe(1);
            var service1Latencies = result[1];
            service1Latencies.NumberOfRequest.ShouldBe(2);
            service1Latencies.CalculateAverageLatency().ShouldBe(120 / 2);
        }

        private LatencyDataProcessor GetSut()
        {
            return new LatencyDataProcessor(_latencyServiceClient.Object);
        }
    }

}
