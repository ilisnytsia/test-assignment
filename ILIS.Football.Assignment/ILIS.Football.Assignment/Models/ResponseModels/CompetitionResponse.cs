namespace ILIS.Football.Assignment.Models.ViewModels
{
    public class CompetitionResponse
    {
        public Competition Competition { get; set; }

        public List<MatchResponse> Matches { get; set; }
    }
}
