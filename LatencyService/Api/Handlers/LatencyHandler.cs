using LatencyService.Api.Modals;
using LatencyService.Domain;

namespace LatencyService.Api.Handlers
{
    public interface ILatencyHandler
    {
        Task<LatencyResponseModal> HandleRequest(DateTime fromDate, DateTime toDate);
    }

    public class LatencyHandler : ILatencyHandler
    {
        private readonly ILatencyDataProcessor _latencyCalculator;

        public LatencyHandler(ILatencyDataProcessor latencyCalculator)
        {
            _latencyCalculator = latencyCalculator;
        }

        public async Task<LatencyResponseModal> HandleRequest(DateTime fromDate, DateTime toDate)
        {
            var serviceLatencies = await _latencyCalculator.CalculateLatencies(fromDate, toDate);

            var period = new LatencyResponseModal.PeriodModal(fromDate, toDate);

            var averageLatencies = new List<LatencyResponseModal.AverageLatency>();

            foreach (var serviceId in serviceLatencies.Keys)
            {
                var serviceLatency = serviceLatencies[serviceId];
                var averageLatency = serviceLatency.CalculateAverageLatency();
                var averageLatencyModal = new LatencyResponseModal.AverageLatency(serviceId, serviceLatency.NumberOfRequest, averageLatency);
                averageLatencies.Add(averageLatencyModal);
            }

            return new LatencyResponseModal(period, averageLatencies);
        }
    }
}
