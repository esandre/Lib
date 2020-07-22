using System;
using System.Text.RegularExpressions;

namespace Lib.Encapsulation.String
{
    /// <summary>
    /// Enforces that the string is alphanumerical ([A-Za-z0-9])
    /// </summary>
    public class AlphanumericStringDecorator : EncapsulatedValueAbstract<string>
    {
        private static readonly Regex AlphanumericRegex = new Regex("^[A-Za-z0-9]*$");

        /// <summary>
        /// Constructor from other encapsulated value
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public AlphanumericStringDecorator(IEncapsulatedValue<string> decorated) : base(decorated)
        {
            if(!AlphanumericRegex.IsMatch(decorated.Value)) throw new FormatException(BadInputMessage);
        }

        /// <summary>
        /// Constructor from raw value
        /// </summary>
        /// <exception cref="FormatException"></exception>
        public AlphanumericStringDecorator(string input) : base(input)
        {
            if (!AlphanumericRegex.IsMatch(input)) throw new FormatException(BadInputMessage);
        }

        /// <summary>
        /// Message for when the input is not alphanumerical
        /// </summary>
        public static string BadInputMessage => $"Input doest not matches regex {AlphanumericRegex}";
    }
}
