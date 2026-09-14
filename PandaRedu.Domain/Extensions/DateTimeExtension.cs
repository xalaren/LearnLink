namespace PandaRedu.Domain.Extensions;

/// <summary>
/// Extensions of <see cref="DateTime"/>
/// </summary>
public static class DateTimeExtension
{
    extension(DateTime utcDateTime)
    {
        /// <summary>
        /// Converts current date time into local time zone
        /// </summary>
        /// <returns><see cref="DateTime"/> converted to local time zone</returns>
        public DateTime ToLocalDateTime()
        {
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, localTimeZone);
        }
    }
}