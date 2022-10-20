using System.Text.Json.Serialization;

namespace LatencyService.Infrastructure.LatencyApi
{
    public class LatencyRecord
    {
        [JsonPropertyName("requestId")]
        public int RequestId { get; set; }

        [JsonPropertyName("serviceId")]
        public byte ServiceId { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("milliSecondsDelay")]
        public int MilliSecondsDelay { get; set; }

    }
}
