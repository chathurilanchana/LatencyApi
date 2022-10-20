namespace LatencyService.Api.Modals
{
    public class LatencyResponseModal
    {
        public PeriodModal Period { get; }

        public LatencyResponseModal(PeriodModal period, List<AverageLatency> averageLatencies = null)
        {
            Period = period;
            AverageLatencies = averageLatencies ?? new List<AverageLatency>();
        }

        public List<AverageLatency> AverageLatencies { get; }

        public void AddLatency(AverageLatency latency)
        {
            AverageLatencies.Add(latency);
        }

        public class PeriodModal
        {
            public PeriodModal(DateTime startDate, DateTime endDate)
            {
                StartDate = startDate;
                EndDate = endDate;
            }
            public DateTime StartDate { get; }
            public DateTime EndDate { get; }
        }

        public class AverageLatency
        {
            public AverageLatency(byte serviceId, int numberOfRequests, int averageResponseTimeMs)
            {
                ServiceId = serviceId;
                NumberOfRequests = numberOfRequests;
                AverageResponseTimeMs = averageResponseTimeMs;
            }

            public byte ServiceId { get; }
            public int NumberOfRequests { get; }
            public int AverageResponseTimeMs { get; }
        }
    }
}
