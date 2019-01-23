namespace Lib.DateTime.Specialized
{
    /// <summary>
    /// Extends <see cref="IDateTimeProvider"/> with Specialized instances creation
    /// </summary>
    public static class SpecializedDateTimeProviderExtensions
    {
        /// <summary>
        /// Returns provider's current year
        /// </summary>
        public static Year CurrentYear(this IDateTimeProvider provider) => new Year(provider.Now.Year);

        /// <summary>
        /// Returns provider's current month
        /// </summary>
        public static Month CurrentMonth(this IDateTimeProvider provider)
        {
            var now = provider.Now;
            return new Month(now.Year, now.Month);
        }

        /// <summary>
        /// Returns provider's current day
        /// </summary>
        public static Day CurrentDay(this IDateTimeProvider provider)
        {
            var now = provider.Now;
            return new Day(now.Year, now.Month, now.Day);
        }

        /// <summary>
        /// Returns provider's current hour
        /// </summary>
        public static Hour CurrentHour(this IDateTimeProvider provider)
        {
            var now = provider.Now;
            return new Hour(now.Year, now.Month, now.Day, now.Hour);
        }

        /// <summary>
        /// Returns provider's current minute
        /// </summary>
        public static Minute CurrentMinute(this IDateTimeProvider provider)
        {
            var now = provider.Now;
            return new Minute(now.Year, now.Month, now.Day, now.Hour, now.Minute);
        }

        /// <summary>
        /// Returns provider's current hour
        /// </summary>
        public static Second CurrentSecond(this IDateTimeProvider provider)
        {
            var now = provider.Now;
            return new Second(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        }
    }
}
