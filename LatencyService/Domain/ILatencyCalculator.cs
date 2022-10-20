using LatencyService.Domain.Entities;

namespace LatencyService.Domain
{
    public interface ILatencyCalculator
    {
        Task<Dictionary<byte, ServiceLatency>> CalculateLatencies(DateTime fromDate, DateTime toDate);
    }
}
