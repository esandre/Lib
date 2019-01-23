namespace Lib.DateTime
{
    /// <summary>
    /// Provides an abstract clock
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Now
        /// </summary>
        System.DateTime Now { get; }
    }
}
