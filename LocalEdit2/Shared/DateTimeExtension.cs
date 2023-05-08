namespace LocalEdit2.Shared
{
    public static class DateTimeExtension
    {
        public static int sTimezoneOffset;

        public static DateTime ToRealLocalTime(this DateTime datetime)
        {
            return datetime.AddMinutes(sTimezoneOffset);
        }
    }
}
