using ILIS.Football.Assignment.Infrastructure;
using ILIS.Football.Assignment.Infrastructure.Models;
using ILIS.Football.Assignment.Models.ViewModels;
using Microsoft.Extensions.Options;

namespace ILIS.Football.Assignment.BusinessLogic
{
    public interface IFootballGamesService
    {
        Task<CompetitionResponse> GetMatchesAsync(int competitionsId, bool isRecent);
        Task<CompetitionResponse[]> GetAllMatchesAsync(bool isRecent);
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

        public async Task<CompetitionResponse> GetMatchesAsync(int competitionsId, bool isRecent)
        {
            var key = $"{competitionsId}_{(isRecent ? "recent" : "upcoming")}";

            //read from cahce coz of limited requests for minute due to free sub
            var competition = _memoryCacheManager.Get<CompetitionResponse>(key);
            if(competition != null)
            {
                return competition;
            }

            //TODO: use dynamic dates or paging
            var dateFrom = isRecent ? DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd") :DateTime.Now.ToString("yyyy-MM-dd");
            var dateTo = isRecent ? DateTime.Now.ToString("yyyy-MM-dd") : DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");
            var status = isRecent ? "FINISHED" : "SCHEDULED,LIVE";

            var response = await _footballGamesApiClient.GetMatchesAsync(competitionsId, dateFrom, dateTo, status);
            if(response != null)
            {
                if (isRecent)
                {
                    CompetitionResponse viewModel = new CompetitionResponse()
                    {
                        Competition = response.Competition,
                        Matches = response.Matches.AsParallel().Select(x => new MatchResponse(x)).OrderByDescending(x => x.UtcDate).ToList(),
                    };
                    _memoryCacheManager.Set(key, viewModel, TimeSpan.FromMinutes(1));
                    return viewModel;
                }
                else
                {
                    CompetitionResponse viewModel = new CompetitionResponse()
                    {
                        Competition = response.Competition,
                        Matches = response.Matches.AsParallel().Select(x => new MatchResponse(x)).ToList(),
                    };
                    _memoryCacheManager.Set(key, viewModel, TimeSpan.FromMinutes(1));
                    return viewModel;
                }                             
            }

            return null;
        }

        public async Task<CompetitionResponse[]> GetAllMatchesAsync(bool isRecent)
        {
            var allCompetitionsIds = _competitionsConfig.Items.Select(x => x.Id);

            var tasks = new List<Task<CompetitionResponse>>();
            foreach(var id in allCompetitionsIds)
            {
                tasks.Add(this.GetMatchesAsync(id,isRecent));
            }

            var results = await Task.WhenAll(tasks);

            return results;
        }
    }
}
