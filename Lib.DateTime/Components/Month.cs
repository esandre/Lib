namespace Lib.DateTime.Components
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
    }
}
