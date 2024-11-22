namespace ILIS.Football.Assignment.Models
{
    public class Match
    {
        public Competition Competition { get; set; }
        public Season Season { get; set; }
        public int Id { get; set; }
        public string UtcDate { get; set; }
        public string Status { get; set; }
        public int Matchday { get; set; }
        public string Stage { get; set; }
        public string Group { get; set; }
        public string LastUpdated { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public Score Score { get; set; }
        public Odds Odds { get; set; }
    }

    public class Odds
    {
        public string Msg { get; set; }
    }

    public class Score
    {
        public string Winner { get; set; }
        public string Duration { get; set; }
    }

    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Tla { get; set; }
        public string Crest { get; set; }
    }

    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Emblem { get; set; }
    }

    public class Season
    {
        public int Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int CurrentMatchday { get; set; }
        public object Winner { get; set; }
    }
}
