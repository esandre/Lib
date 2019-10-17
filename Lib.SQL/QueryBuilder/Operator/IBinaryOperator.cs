namespace Lib.SQL.QueryBuilder.Operator
{
    public interface IBinaryOperator
    {
        string ToString(string operand1, string operand2);
    }
}
