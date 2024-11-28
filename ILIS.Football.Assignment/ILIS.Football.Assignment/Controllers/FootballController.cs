using ILIS.Football.Assignment.BusinessLogic;
using ILIS.Football.Assignment.Models;
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
        public async Task<IActionResult> Get([FromQuery] int competitionsId, bool isRecent)
        {
            var competition = await _footballGamesService.GetMatchesAsync(competitionsId, isRecent);

            return Ok(competition);
        }

        [HttpGet("matches/all")]
        public async Task<IActionResult> GetAll ([FromQuery] bool isRecent, [FromQuery] int skip = 0, [FromQuery] int take = 2)
        {
            var matches = (await _footballGamesService.GetAllMatchesAsync(isRecent)).Skip(skip).Take(take).ToArray();

            return Ok(matches);
        }
    }
}
