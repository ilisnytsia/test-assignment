namespace ILIS.Football.Assignment.Infrastructure.Models
{
    public class CompetitionsConfig
    {
        public List<CompetitionConfiguration> Items { get; set; }
    }

    public class CompetitionConfiguration
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
