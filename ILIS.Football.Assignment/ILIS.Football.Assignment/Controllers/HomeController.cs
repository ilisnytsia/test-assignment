using ILIS.Football.Assignment.BusinessLogic;
using ILIS.Football.Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ILIS.Football.Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IFootballGamesService _footballGamesService;

        public HomeController(ILogger<HomeController> logger, IFootballGamesService footballGamesService)
        {
            _footballGamesService = footballGamesService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var competitionsWithMatches = await _footballGamesService.GetAllMatchesAsync(false);

            return View(competitionsWithMatches);
        }

        [HttpGet]
        public async Task<IActionResult> GetCompetitionPartial(int competitionId, bool isRecent)
        {
            var competition = await _footballGamesService.GetMatchesAsync(competitionId, isRecent);

            if (competition == null)
            {
                return NotFound();
            }

            if (isRecent)
            {
                competition.Matches.Sort((x, y) => y.UtcDate.CompareTo(x.UtcDate));
            }

            return PartialView("_CompetitionPartial", competition);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
