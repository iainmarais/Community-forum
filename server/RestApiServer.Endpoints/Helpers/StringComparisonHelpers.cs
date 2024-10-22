namespace RestApiServer.Endpoints.Helpers
{
    public class StringComparisonHelpers
    {
        public static bool CaseInsensitiveContains(string? src, string query)
        {
            if(string.IsNullOrEmpty(src) || string.IsNullOrEmpty(query))
            {
                return false;
            }
            return src.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}