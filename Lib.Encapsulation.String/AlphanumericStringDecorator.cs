using System;
using System.Text.RegularExpressions;

namespace Lib.Encapsulation.String
{
    /// <summary>
    /// Enforces that the string is alphanumerical ([A-Za-z0-9])
    /// </summary>
    public class AlphanumericStringDecorator : RegexStringDecorator
    {
        private static readonly Regex AlphanumericRegex = new Regex("^[A-Za-z0-9]*$");

        /// <summary>
        /// Constructor from other encapsulated value
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public AlphanumericStringDecorator(IEncapsulatedValue<string> decorated) : base(decorated, AlphanumericRegex)
        {
        }

        /// <summary>
        /// Constructor from raw value
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public AlphanumericStringDecorator(string input) : base(input, AlphanumericRegex)
        {
        }
    }
}
