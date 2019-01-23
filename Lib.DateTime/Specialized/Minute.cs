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
    }
}
