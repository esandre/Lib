using System;

namespace Lib.Encapsulation.String
{
    /// <summary>
    /// Enforces a maximal length for a string
    /// </summary>
    public class MaximalLengthStringDecorator : EncapsulatedValueAbstract<string>
    {
        /// <summary>
        /// Message triggered when length exceeded
        /// </summary>
        public static readonly Func<int, string> LengthExceededMessage = limit => $"Length of input exceeds {limit} characters length limit.";

        /// <summary>
        /// Constructor from other encapsulated value
        /// </summary>
        public MaximalLengthStringDecorator(int limit, IEncapsulatedValue<string> decorated) : base(decorated)
        {
            if (decorated.Value.Length > limit) throw new FormatException(LengthExceededMessage(limit));
        }

        /// <summary>
        /// Constructor from raw value
        /// </summary>
        public MaximalLengthStringDecorator(int limit, string input) : base(input)
        {
            if (input.Length > limit)
            {
                throw new FormatException(LengthExceededMessage(limit));
            }
        }
    }
}
