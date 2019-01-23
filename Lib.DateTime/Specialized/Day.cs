namespace Lib.DateTime.Specialized
{
    /// <inheritdoc />
    /// <summary>
    /// A Day
    /// </summary>
    public class Day : SpecializedDateTimeAbstract
    {
        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        public Day(int year, int month, int day) : base(new System.DateTime(year, month, day))
        {
        }

        /// <summary>
        /// Extracts a Day from a DateTime, ignoring more precise parts
        /// </summary>
        public static implicit operator Day(System.DateTime dateTime)
            => new Day(dateTime.Year, dateTime.Month, dateTime.Day);
    }
}
