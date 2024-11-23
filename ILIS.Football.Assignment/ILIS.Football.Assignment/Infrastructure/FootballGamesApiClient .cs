using ILIS.Football.Assignment.Models;

namespace ILIS.Football.Assignment.Infrastructure
{
    public interface IFootballGamesApiClient
    {
        Task<FootballApiResponse> GetMatchesAsync(int competitionsId, string dateFrom, string dateTo,string status);
    }

    public class FootballGamesApiClient : IFootballGamesApiClient
    {
        private readonly HttpClient _httpClient;

        public FootballGamesApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FootballApiResponse> GetMatchesAsync(int competitionsId, string dateFrom, string dateTo, string status)
        {
            var url = $"competitions/{competitionsId}/matches?dateFrom={dateFrom}&dateTo={dateTo}&status={status}";
            var response = await _httpClient.GetAsync(url);
            var test = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) 
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<FootballApiResponse>(json, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result;
            }

            return null;

        }
    }
}
