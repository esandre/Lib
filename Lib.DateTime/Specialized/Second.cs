namespace Lib.DateTime.Specialized
{
    /// <summary>
    /// A Second
    /// </summary>
    public class Second : SpecializedDateTimeAbstract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Second(int year, int month, int day, int hour, int minute, int second) 
            : base(new System.DateTime(year, month, day, hour, minute, second))
        {
        }

        /// <summary>
        /// Extracts a Second from a DateTime, ignoring more precise parts
        /// </summary>
        public static implicit operator Second(System.DateTime dateTime)
            => new Second(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
    }
}
