using System.Globalization;
using System.Text.Json;

namespace LatencyService.Infrastructure.LatencyApi
{
    public class LatencyServiceClient
    {
        private readonly HttpClient _httpClient;

        public LatencyServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://latencyapi-env.eba-kqb2ph3i.eu-west-1.elasticbeanstalk.com");
        }

        public async Task<IEnumerable<LatencyRecord>?> Get(DateTime date)
        {
            var formattedDateOnly = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var response = await _httpClient.GetAsync($"latencies?date={formattedDateOnly}");

            //TODO: Add exception handling
            if (response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var result = await JsonSerializer.DeserializeAsync<IEnumerable<LatencyRecord>>(contentStream);
                return result;
            }

            throw new Exception($"Status code from latency client is {response.StatusCode}");
        }
    }
}
