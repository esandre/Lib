namespace Lib.Encapsulation
{
    /// <summary>
    /// An Encapsulated value
    /// </summary>
    public interface IEncapsulatedValue<out TValue>
    {
        /// <summary>
        /// InnerValue
        /// </summary>
        TValue Value { get; }
    }
}
