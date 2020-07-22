using System;

namespace Lib.Encapsulation.String
{
    /// <summary>
    /// Enforces a minimal length for a string
    /// </summary>
    public class MinimalLengthStringDecorator : NotNullOrEmptyStringDecorator
    {
        /// <summary>
        /// Message triggered when string is too short
        /// </summary>
        public static readonly Func<int, string> TooShortMessage = limit => $"Input does not satisfy {limit} characters length requirement.";

        /// <summary>
        /// Constructor from other encapsulated value
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public MinimalLengthStringDecorator(int minLength, IEncapsulatedValue<string> decorated) : base(decorated)
        {
            if(decorated.Value.Length < minLength) throw new FormatException(TooShortMessage(minLength));
        }

        /// <summary>
        /// Constructor from raw value
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public MinimalLengthStringDecorator(int minLength, string input) : base(input)
        {
            if (input.Length < minLength) throw new FormatException(TooShortMessage(minLength));
        }
    }
}
