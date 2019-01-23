namespace Lib.DateTime.Specialized
{
    /// <inheritdoc />
    /// <summary>
    /// A year
    /// </summary>
    public class Year : SpecializedDateTimeAbstract
    {
        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        public Year(int year) : base(new System.DateTime(year, 1, 1))
        {
        }

        /// <summary>
        /// Extracts a Year from a DateTime, ignoring more precise parts
        /// </summary>
        public static implicit operator Year(System.DateTime dateTime)
            => new Year(dateTime.Year);
    }
}
