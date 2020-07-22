namespace Lib.SQL.Operation.QueryBuilder.Operator
{
    public class InOperator : IBinaryOperator
    {
        public string ToString(string operand1, string operand2) => operand1 + $" IN ({operand2})";
    }
}
