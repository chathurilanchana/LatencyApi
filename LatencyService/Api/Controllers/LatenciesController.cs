using LatencyService.Api.Handlers;
using LatencyService.Api.Modals;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace LatencyService.APi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LatenciesController : ControllerBase
    {
      private readonly ILatencyHandler _latencyService;

        public LatenciesController(ILatencyHandler latencyService)
        {
            _latencyService = latencyService;
        }

        [HttpGet(Name = "GetLatencies")]
        public async Task<LatencyModal> Get(string startDate, string endDate)
        {
            var startDateInDateFormat = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endDateInDateFormat = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var result = await _latencyService.CalculateLatency(startDateInDateFormat, endDateInDateFormat);

            return result;
        }
    }
}