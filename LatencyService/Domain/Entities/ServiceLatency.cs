namespace LatencyService.Domain.Entities
{
    public class ServiceLatency
    {
        public byte ServiceId { get; }

        public ServiceLatency(byte serviceId, int latency)
        {
            ServiceId = serviceId;
            NumberOfRequest = 1;
            TotalLatency = latency;
        }

        public void AddLatencyForNewRequest(int latency)
        {
            NumberOfRequest++;
            TotalLatency = TotalLatency + latency;
        }

        public int CalculateAverageLatency()
        {
            var averageLatency = (int)Math.Round(TotalLatency / (decimal)NumberOfRequest, 0, MidpointRounding.AwayFromZero);

            return averageLatency;
        }

        public int NumberOfRequest { get; private set; }
        public int TotalLatency { get; private set; }
    }
}
