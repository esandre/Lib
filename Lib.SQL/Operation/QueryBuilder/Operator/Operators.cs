namespace Lib.SQL.Operation.QueryBuilder.Operator
{
    public static class Is
    {
        public static readonly IBinaryOperator EqualWith = new EqualityOperator();
        public static readonly IBinaryOperator DifferentWith = new InequalityOperator();
        public static readonly IBinaryOperator LesserThan = new LesserOperator();
        public static readonly IBinaryOperator GreaterThan = new GreaterOperator();
        public static readonly IBinaryOperator LesserOrEqualThan = new LesserOrEqualsOperator();
        public static readonly IBinaryOperator GreaterOrEqualThan = new GreaterOrEqualsOperator();
        public static readonly IBinaryOperator In = new InOperator();
    }

    internal class InequalityOperator : SimpleBinaryOperator { internal InequalityOperator() : base("!=") { } }
    internal class EqualityOperator : SimpleBinaryOperator { internal EqualityOperator() : base("=") { } }
    internal class LesserOperator : SimpleBinaryOperator { internal LesserOperator() : base("<") { } }
    internal class LesserOrEqualsOperator : SimpleBinaryOperator { internal LesserOrEqualsOperator() : base("<=") { } }
    internal class GreaterOperator : SimpleBinaryOperator { internal GreaterOperator() : base(">") { } }
    internal class GreaterOrEqualsOperator : SimpleBinaryOperator { internal GreaterOrEqualsOperator() : base(">=") { } }
}
