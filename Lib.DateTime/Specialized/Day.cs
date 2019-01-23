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
    }
}
