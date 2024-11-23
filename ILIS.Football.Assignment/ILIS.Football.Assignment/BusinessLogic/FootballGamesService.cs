using ILIS.Football.Assignment.Infrastructure;
using ILIS.Football.Assignment.Infrastructure.Models;
using ILIS.Football.Assignment.Models;
using Microsoft.Extensions.Options;

namespace ILIS.Football.Assignment.BusinessLogic
{
    public interface IFootballGamesService
    {
        Task<FootballApiResponse> GetMatchesAsync(int competitionsId, bool isRecent);
        Task<FootballApiResponse[]> GetAllMatchesAsync(bool isRecent);
    }

    public class FootballGamesService : IFootballGamesService
    {
        public readonly IFootballGamesApiClient _footballGamesApiClient;

        private readonly IMemoryCacheManager _memoryCacheManager;

        private readonly CompetitionsConfig _competitionsConfig;

        public FootballGamesService(IFootballGamesApiClient footballGamesApiClient, IMemoryCacheManager memoryCacheManager, IOptions<CompetitionsConfig> options) 
        {
            _footballGamesApiClient = footballGamesApiClient;
            _memoryCacheManager = memoryCacheManager;
            _competitionsConfig = options.Value;
        }

        public async Task<FootballApiResponse> GetMatchesAsync(int competitionsId, bool isRecent)
        {
            var key = $"{competitionsId}_{(isRecent ? "recent" : "upcoming")}";

            //read from cahce coz of limited requests for minute due to free sub
            var competition = _memoryCacheManager.Get<FootballApiResponse>(key);
            if(competition != null)
            {
                return competition;
            }

            var dateFrom = isRecent ? DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd") :DateTime.Now.ToString("yyyy-MM-dd");
            var dateTo = isRecent ? DateTime.Now.ToString("yyyy-MM-dd") : DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
            var status = isRecent ? "FINISHED" : "SCHEDULED,LIVE";

            var response = await _footballGamesApiClient.GetMatchesAsync(competitionsId, dateFrom, dateTo, status);
            if(response != null)
            {
                _memoryCacheManager.Set(key, response, TimeSpan.FromMinutes(1));
            }

            return response;
        }

        public async Task<FootballApiResponse[]> GetAllMatchesAsync(bool isRecent)
        {
            var allCompetitionsIds = _competitionsConfig.Items.Select(x => x.Id);

            var tasks = new List<Task<FootballApiResponse>>();
            foreach(var id in allCompetitionsIds)
            {
                tasks.Add(this.GetMatchesAsync(id,isRecent));
            }

            var results = await Task.WhenAll(tasks);

            return results;
        }
    }
}
