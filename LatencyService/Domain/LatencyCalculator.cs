using LatencyService.Domain.Entities;
using LatencyService.Infrastructure.LatencyApi;

namespace LatencyService.Domain
{
    public class LatencyCalculator : ILatencyCalculator
    {
        private readonly ILatencyServiceClient _latencyService;

        public LatencyCalculator(ILatencyServiceClient latencyService)
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

                //NOTE: Duplicate request ids are not possible in the same day and same service, but can happen in same day in different services or different days
                var distinctRequests = latencyRecords.DistinctBy(x => new { x.RequestId, x.ServiceId });
                
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
