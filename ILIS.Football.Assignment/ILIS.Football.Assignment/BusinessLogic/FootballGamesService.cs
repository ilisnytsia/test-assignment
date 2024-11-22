using ILIS.Football.Assignment.Infrastructure;
using ILIS.Football.Assignment.Models;

namespace ILIS.Football.Assignment.BusinessLogic
{
    public interface IFootballGamesService
    {
        Task<FootballApiResponse> GetMatchesAsync(string competitionsId, bool isRecent);
        Task<FootballApiResponse[]> GetAllMatchesAsync(bool isRecent);
    }

    public class FootballGamesService : IFootballGamesService
    {
        public readonly IFootballGamesApiClient _footballGamesApiClient;

        public FootballGamesService(IFootballGamesApiClient footballGamesApiClient) 
        {
            _footballGamesApiClient = footballGamesApiClient;
        }

        public async Task<FootballApiResponse> GetMatchesAsync(string competitionsId, bool isRecent)
        {
            var dateFrom = isRecent ? DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") :DateTime.Now.ToString("yyyy-MM-dd");

            var dateTo = isRecent ? DateTime.Now.ToString("yyyy-MM-dd") : DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");

            var response = await _footballGamesApiClient.GetMatchesAsync(competitionsId, dateFrom, dateTo);

            return response;
        }

        public async Task<FootballApiResponse[]> GetAllMatchesAsync(bool isRecent)
        {
            var dateFrom = isRecent ? DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");

            var dateTo = isRecent ? DateTime.Now.ToString("yyyy-MM-dd") : DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");

            var allCompetitionsIds = new string[] { "PPL", "PL", "BL1", "SA", "DED", "PD", "BSA" };

            var tasks = new List<Task<FootballApiResponse>>();

            foreach(var id in allCompetitionsIds)
            {
                tasks.Add(_footballGamesApiClient.GetMatchesAsync(id, dateFrom, dateTo));
            }

            var results = await Task.WhenAll(tasks);

            return results;
        }
    }
}
