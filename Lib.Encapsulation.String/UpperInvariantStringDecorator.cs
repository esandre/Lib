namespace Lib.Encapsulation.String
{
    public class UpperInvariantStringDecorator : EncapsulatedValueAbstract<string>
    {
        public UpperInvariantStringDecorator(IEncapsulatedValue<string> other) : base(other.Value.ToUpperInvariant())
        {
        }

        public UpperInvariantStringDecorator(string input) : base(input.ToUpperInvariant())
        {
        }
    }
}
