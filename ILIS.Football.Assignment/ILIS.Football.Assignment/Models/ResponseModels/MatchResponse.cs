using ILIS.Football.Assignment.Helpers;

namespace ILIS.Football.Assignment.Models.ViewModels
{
    public class MatchResponse
    {
        public string UtcDate { get; set; }
        public string UtcDateFormatted { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public Score Score { get; set; }


        public MatchResponse(Match match)
        {

            UtcDate = match.UtcDate;
            UtcDateFormatted = match.UtcDate.FormatDateTime();
            HomeTeam = match.HomeTeam;
            AwayTeam = match.AwayTeam;
            Score = match.Score;
        }
    }
}
