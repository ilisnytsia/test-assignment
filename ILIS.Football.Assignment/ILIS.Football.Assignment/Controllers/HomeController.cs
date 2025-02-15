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
            var userAgent = Request.Headers["User-Agent"].ToString();
            if (userAgent.Contains("Mobile"))
            {
                var competitionsWithMatches = (await _footballGamesService.GetAllMatchesAsync(false)).Take(2).ToArray();

                return View(competitionsWithMatches);
            }

            return Content("Mobile access Only");            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
