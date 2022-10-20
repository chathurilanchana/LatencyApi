namespace LatencyService.Api.Modals
{
    public class LatencyModal
    {
        public PeriodModal Period { get; }

        public LatencyModal(PeriodModal period, List<AverageLatency> averageLatencies = null)
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
            public AverageLatency(byte serviceId, int numberOfRequest, int averageResponseTimeMs)
            {
                ServiceId = serviceId;
                NumberOfRequest = numberOfRequest;
                AVerageResponseTimeMs = averageResponseTimeMs;
            }

            public byte ServiceId { get; }
            public int NumberOfRequest { get; }
            public int AVerageResponseTimeMs { get; }
        }
    }
}
