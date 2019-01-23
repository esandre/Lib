namespace Lib.DateTime.Specialized
{
    /// <summary>
    /// An Hour
    /// </summary>
    public class Hour : SpecializedDateTimeAbstract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Hour(int year, int month, int day, int hour) 
            : base(new System.DateTime(year, month, day, hour, 0, 0))
        {
        }

        /// <summary>
        /// Extracts an Hour from a DateTime, ignoring more precise parts
        /// </summary>
        public static implicit operator Hour(System.DateTime dateTime)
            => new Hour(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour);
    }
}
