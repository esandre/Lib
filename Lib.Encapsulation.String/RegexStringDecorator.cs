using System;
using System.Text.RegularExpressions;

namespace Lib.Encapsulation.String
{
    public class RegexStringDecorator : EncapsulatedValueAbstract<string>
    {
        public RegexStringDecorator(IEncapsulatedValue<string> other, Regex regex) : base(other)
        {
            if (!regex.IsMatch(other.Value)) throw new FormatException(BadInputMessage(other.Value, regex));
        }

        public RegexStringDecorator(string input, Regex regex) : base(input)
        {
            if (!regex.IsMatch(input)) throw new FormatException(BadInputMessage(input, regex));
        }

        /// <summary>
        /// Message for when the input is not alphanumerical
        /// </summary>
        private static string BadInputMessage(string input, Regex regex) => $"Input {input} doest not matches regex {regex}";
    }
}
