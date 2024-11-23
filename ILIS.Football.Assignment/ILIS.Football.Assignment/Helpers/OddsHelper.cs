namespace ILIS.Football.Assignment.Helpers
{
    public static class OddsHelper
    {
        private static readonly Random _random = new Random();

        public static double GenerateRandomdOdds()
        {
            int category = _random.Next(1, 4); 
            switch (category)
            {
                case 1: 
                    return Math.Round(_random.NextDouble() * (2.50 - 1.10) + 1.10, 2);
                case 2: 
                    return Math.Round(_random.NextDouble() * (5.00 - 2.50) + 2.50, 2);
                default:
                    return 2.01;
            }
        }
    }
}