public static class DateTimeExtension
{
    public static DateTime ToLocalDateTime(this DateTime utcDateTime)
    {
        TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, localTimeZone);
    }
}