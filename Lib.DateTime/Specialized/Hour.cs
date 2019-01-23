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
    }
}
