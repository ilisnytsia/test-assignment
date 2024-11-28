namespace ILIS.Football.Assignment.Helpers
{
    public static class JsonHelper
    {
        private static readonly System.Text.Json.JsonSerializerOptions _camelCaseOptions =
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            };

        public static string SerializeCamelCase(object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj, _camelCaseOptions);
        }
    }
}
