namespace Lib.Encapsulation
{
    /// <summary>
    /// An Encapsulated value
    /// </summary>
    public interface IEncapsulatedValue<TValue>
    {
        /// <summary>
        /// InnerValue
        /// </summary>
        TValue Value { get; }
    }
}
