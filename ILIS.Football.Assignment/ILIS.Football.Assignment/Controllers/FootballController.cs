using ILIS.Football.Assignment.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace ILIS.Football.Assignment.Controllers
{
    [Route("api/football")]
    [ApiController]
    public class FootballController : ControllerBase
    {
        private readonly IFootballGamesService _footballGamesService;

        public FootballController(IFootballGamesService footballGamesService)
        {
            _footballGamesService = footballGamesService;
        }

        [HttpGet("matches")]
        public async Task<IActionResult> Get([FromQuery] string competitionsId, bool isRecent)
        {
            var matches = await _footballGamesService.GetMatchesAsync(competitionsId, isRecent);

            return Ok(matches);
        }

        [HttpGet("matches/all")]
        public async Task<IActionResult> GetAll ([FromQuery] bool isRecent)
        {
            var matches = await _footballGamesService.GetAllMatchesAsync(isRecent);

            return Ok(matches);
        }
    }
}
