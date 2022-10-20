using LatencyService.Api.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

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
        public async Task<ObjectResult> Get(string startDate, string endDate)
        {
            var startDateInDateFormat = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endDateInDateFormat = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var errorMessage = ValidateDates(startDateInDateFormat, endDateInDateFormat);
            if (errorMessage != string.Empty)
                return StatusCode(400, errorMessage);

            var result = await _latencyHandler.HandleRequest(startDateInDateFormat, endDateInDateFormat);

            return Ok(result);
        }

        private string ValidateDates(DateTime startDateInDateFormat, DateTime endDateInDateFormat)
        {
            var errorMessageBuilder = new StringBuilder();
            if (startDateInDateFormat.Year != 2021)
                errorMessageBuilder.Append("Start date should be in 2021.");
            if (endDateInDateFormat.Year != 2021)
                errorMessageBuilder.Append("End date should be in 2021.");
            if (startDateInDateFormat > endDateInDateFormat)
                errorMessageBuilder.Append("Start date should be less than or equal to end date.");
            if ((endDateInDateFormat - startDateInDateFormat).TotalDays > 31)
                    errorMessageBuilder.Append("You can only request up to 30 days at once. Please batch requests for longer durations.");

            var errorMessage = errorMessageBuilder.ToString();
            return errorMessage.Replace('.', '\n');
        }
    }
}