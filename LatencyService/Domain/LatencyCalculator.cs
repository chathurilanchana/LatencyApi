using LatencyService.Domain.Entities;
using LatencyService.Infrastructure.LatencyApi;

namespace LatencyService.Domain
{
    public class LatencyCalculator : ILatencyCalculator
    {
        private readonly LatencyServiceClient _latencyService;

        public LatencyCalculator(LatencyServiceClient latencyService)
        {
            _latencyService = latencyService;
        }

        public async Task<Dictionary<byte, ServiceLatency>> CalculateLatencies(DateTime fromDate, DateTime toDate)
        {
            var serviceDictionary = new Dictionary<byte, ServiceLatency>();

            var numberOfDaysToFetch = (toDate - fromDate).TotalDays;

            for(int i = 0; i <= numberOfDaysToFetch; i++)
            {
                var dateToFetch = fromDate.AddDays(i);

                var latencyRecords = await _latencyService.Get(dateToFetch);

                if (latencyRecords == null  || latencyRecords.Count() ==0 )
                    continue;

                var distinctRequests = latencyRecords.DistinctBy(x => x.RequestId);
                
                foreach(var request in distinctRequests)
                {
                    if (serviceDictionary.ContainsKey(request.ServiceId))
                    {
                        var serviceLatency = serviceDictionary[request.ServiceId];
                        serviceLatency.AddLatencyForNewRequest(request.MilliSecondsDelay);
                        serviceDictionary[request.ServiceId] = serviceLatency;
                    }
                    else
                    {
                        var serviceLatency = new ServiceLatency(request.ServiceId, request.MilliSecondsDelay);
                        serviceDictionary.Add(request.ServiceId, serviceLatency);
                    }
                }

            }
            return serviceDictionary;
        }
    }
}
