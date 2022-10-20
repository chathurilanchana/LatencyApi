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
        private readonly ILatencyHandler _latencyHandler;

        public LatenciesController(ILatencyHandler latencyService)
        {
            _latencyHandler = latencyService;
        }

        [HttpGet(Name = "GetLatencies")]
        [ProducesResponseType(typeof(LatencyResponseModal), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ObjectResult> Get([FromQuery] LatencyRequestModal requestModal)
        {
            var startDateInDateFormat = DateTime.ParseExact(requestModal.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endDateInDateFormat = DateTime.ParseExact(requestModal.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var result = await _latencyHandler.HandleRequest(startDateInDateFormat, endDateInDateFormat);

            return Ok(result);
        }
    }
}