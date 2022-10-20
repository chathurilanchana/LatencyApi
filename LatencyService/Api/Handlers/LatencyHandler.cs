using LatencyService.Api.Modals;
using LatencyService.Domain;

namespace LatencyService.Api.Handlers
{
    public interface ILatencyHandler
    {
        Task<LatencyModal> HandleRequest(DateTime fromDate, DateTime toDate);
    }

    public class LatencyHandler : ILatencyHandler
    {
        private readonly ILatencyCalculator _latencyCalculator;

        public LatencyHandler(ILatencyCalculator latencyCalculator)
        {
            _latencyCalculator = latencyCalculator;
        }

        public async Task<LatencyModal> HandleRequest(DateTime fromDate, DateTime toDate)
        {
            var serviceLatencies = await _latencyCalculator.CalculateLatencies(fromDate, toDate);

            var period = new LatencyModal.PeriodModal(fromDate, toDate);

            var averageLatencies = new List<LatencyModal.AverageLatency>();

            foreach (var serviceId in serviceLatencies.Keys)
            {
                var serviceLatency = serviceLatencies[serviceId];
                var averageLatency = serviceLatency.CalculateAverageLatency();
                var averageLatencyModal = new LatencyModal.AverageLatency(serviceId, serviceLatency.NumberOfRequest, averageLatency);
                averageLatencies.Add(averageLatencyModal);
            }

            return new LatencyModal(period, averageLatencies);
        }
    }
}
