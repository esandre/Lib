namespace Lib.DateTime.Components
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
    }
}
