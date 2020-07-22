using System;

namespace Lib.Encapsulation.String
{
    /// <summary>
    /// Enforces that a string is not null nor empty
    /// </summary>
    public class NotNullOrEmptyStringDecorator : EncapsulatedValueAbstract<string>
    {
        /// <summary>
        /// Message triggered is string is null or empty
        /// </summary>
        public const string NullOrEmptyInputMessage = "Decorated value string cannot be null nor empty";

        /// <summary>
        /// Constructor from other encapsulated value
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public NotNullOrEmptyStringDecorator(IEncapsulatedValue<string> decorated) : base(decorated)
        {
            if (string.IsNullOrEmpty(decorated.Value)) throw new FormatException(NullOrEmptyInputMessage);
        }

        /// <summary>
        /// Constructor from raw value
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public NotNullOrEmptyStringDecorator(string input) : base(input)
        {
            if (string.IsNullOrEmpty(input)) throw new FormatException(NullOrEmptyInputMessage);
        }
    }
}
