using LatencyService.Domain.Entities;
using Xunit;
using Shouldly;

namespace LatencyService.UnitTests.Domain
{
    public class ServiceLatencyTests
    {
        [Fact]
        public void AddingNewRequest_UpdateStatisticsCorrectly()
        {
            var serviceLatency = new ServiceLatency(1, 20);
            serviceLatency.AddLatencyForNewRequest(40);

            serviceLatency.NumberOfRequest.ShouldBe(2);
            serviceLatency.TotalLatency.ShouldBe(20 + 40);
        }

        [Fact]
        public void AverageLatencyCalculationIsCorrect()
        {
            var serviceLatency = new ServiceLatency(1, 20);
            serviceLatency.AddLatencyForNewRequest(40);
            serviceLatency.AddLatencyForNewRequest(10);

            serviceLatency.NumberOfRequest.ShouldBe(3);
            serviceLatency.TotalLatency.ShouldBe(20 + 40 + 10);

            var expectedAverageLatency = 23;
            serviceLatency.CalculateAverageLatency().ShouldBe(expectedAverageLatency);
        }
    }
}
