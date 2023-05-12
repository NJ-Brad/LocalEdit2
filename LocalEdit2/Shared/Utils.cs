namespace LocalEdit2.Shared
{
    public class Utils
    {
        public static string VOD(string? originalValue, string defaultValue = "")
        {
            return (originalValue != null) ? originalValue : defaultValue;
        }
        public static int VOD(int? originalValue, int defaultValue = 0)
        {
            return (originalValue != null) ? originalValue.Value : defaultValue;
        }
    }
}
