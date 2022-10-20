using LatencyService.Domain.Entities;

namespace LatencyService.Domain
{
    public interface ILatencyDataProcessor
    {
        Task<Dictionary<byte, ServiceLatency>> CalculateLatencies(DateTime fromDate, DateTime toDate);
    }
}
