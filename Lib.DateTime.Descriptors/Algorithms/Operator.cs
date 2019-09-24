namespace Lib.DateTime.Descriptors.Algorithms
{
    public struct Operator
    {
        public readonly int Precedence;
        public readonly bool LeftAssociativity;

        public Operator(int precedence, bool leftAssociativity)
        {
            Precedence = precedence;
            LeftAssociativity = leftAssociativity;
        }
    }
}
