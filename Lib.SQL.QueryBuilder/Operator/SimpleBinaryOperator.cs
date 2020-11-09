namespace Lib.SQL.QueryBuilder.Operator
{
    public class SimpleBinaryOperator : IBinaryOperator
    {
        private readonly string _operatorSymbol;

        protected SimpleBinaryOperator(string operatorSymbol)
        {
            _operatorSymbol = operatorSymbol;
        }

        public string ToString(string operand1, string operand2)
        {
            return operand1 + ' ' + _operatorSymbol + ' ' + operand2;
        }
    }
}
