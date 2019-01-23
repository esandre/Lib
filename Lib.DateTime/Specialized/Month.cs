namespace Lib.DateTime.Specialized
{
    /// <inheritdoc />
    /// <summary>
    /// A month
    /// </summary>
    public class Month : SpecializedDateTimeAbstract
    {
        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        public Month(int year, int month) : base(new System.DateTime(year, month, 1))
        {
        }

        /// <summary>
        /// Extracts a Month from a DateTime, ignoring more precise parts
        /// </summary>
        public static implicit operator Month(System.DateTime dateTime)
            => new Month(dateTime.Year, dateTime.Month);
    }
}
