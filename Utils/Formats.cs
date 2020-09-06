namespace Isogeo.Utils
{
    public static class Formats
    {
        public static string FormatDate(string dateStr)
        {
            return dateStr == null ? "" : dateStr.Split('T')[0];
        }
    }
}
