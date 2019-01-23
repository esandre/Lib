namespace Lib.DateTime
{
    /// <summary>
    /// Provides local time, from <see cref="System.DateTime"/>
    /// </summary>
    public class LocalDateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc />
        public System.DateTime Now => System.DateTime.Now;
    }
}
