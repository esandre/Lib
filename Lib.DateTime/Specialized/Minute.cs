namespace Lib.DateTime.Specialized
{
    /// <summary>
    /// A Minute
    /// </summary>
    public class Minute : SpecializedDateTimeAbstract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Minute(int year, int month, int day, int hour, int minute) 
            : base(new System.DateTime(year, month, day, hour, minute, 0))
        {
        }

        /// <summary>
        /// Extracts a Minute from a DateTime, ignoring more precise parts
        /// </summary>
        public static implicit operator Minute(System.DateTime dateTime)
            => new Minute(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute);
    }
}
