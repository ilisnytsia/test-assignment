namespace ILIS.Football.Assignment.Helpers
{
    public class DateTimeHelper
    {
        public static string FormatDateTime(DateTime utcDate)
        {
            var localDate = utcDate.ToLocalTime();
            var now = DateTime.Now;

            if (localDate.Date == now.Date)
            {
                return $"Today {localDate.ToString("h:mm tt")}";
            }
            else if (localDate.Date == now.Date.AddDays(-1))
            {
                return $"Yesterday {localDate.ToString("h:mm tt")}";
            }
            else if (localDate.Date == now.Date.AddDays(1))
            {
                return $"Tomorrow {localDate.ToString("h:mm tt")}";
            }
            else
            {
                return localDate.ToString("MM/dd h:mm tt");
            }
        }
    }
}