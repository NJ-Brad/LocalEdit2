namespace LocalEdit2.CountDownTypes
{
    public class CountDownConfiguration
    {
        public TimeSpan? StartTime { get; set; } = LocalEdit2.Shared.DateTimeExtension.ToRealLocalTime(DateTime.Now).TimeOfDay;// DateTime.Now.TimeOfDay;
        public TimeSpan? EndTime { get; set; } = LocalEdit2.Shared.DateTimeExtension.ToRealLocalTime(DateTime.Now).TimeOfDay.Add(new TimeSpan(1, 0, 0)); //DateTime.Now.TimeOfDay.Add(new TimeSpan(1, 0, 0));
        public int WarningMinutes { get; set; } = 5;
    }
}
