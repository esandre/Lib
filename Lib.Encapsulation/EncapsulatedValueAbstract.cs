namespace Lib.Encapsulation
{
    /// <summary>
    /// Decorated a string
    /// </summary>
    public abstract class EncapsulatedValueAbstract<TValue> : IEncapsulatedValue<TValue>
    {
        /// <inheritdoc />
        public TValue Value { get; }

        /// <summary>
        /// Constructor from other instance
        /// </summary>
        protected EncapsulatedValueAbstract(IEncapsulatedValue<TValue> other)
        {
            Value = other.Value;
        }

        /// <summary>
        /// Constructor from string
        /// </summary>
        protected EncapsulatedValueAbstract(TValue input)
        {
            Value = input;
        }
    }
}
