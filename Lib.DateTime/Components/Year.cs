namespace Lib.DateTime.Components
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
    }
}
