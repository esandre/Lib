using System;
using System.Collections.Generic;
using System.Linq;

namespace Lib.DateTime.Descriptors.Algorithms
{
    public class ShuntingYard
    {
        private readonly IDictionary<string, Operator> _operators;
        private readonly Stack<string> _operatorStack = new Stack<string>();
        private readonly Stack<string> _outputStack = new Stack<string>();

        public ShuntingYard(IDictionary<string, Operator> operators)
        {
            _operators = operators;

            if(_operators.ContainsKey("(") || _operators.ContainsKey(")")) throw new Exception("Operators ( and ) are reserved");
            _operators.Add("(", new Operator(int.MinValue, false));
            _operators.Add(")", new Operator(int.MaxValue, true));
        }

        private string ToPolishNotation(string formula)
        {
            foreach (var o in formula.Split(' ')) ProcessSymbol(o);
            while (_operatorStack.Any()) _outputStack.Push(_operatorStack.Pop());
            return _outputStack.Aggregate((c, e) => c + ' ' + e);
        }

        public string ToReversePolishNotation(string formula)
        {
            return ToPolishNotation(formula).Split(' ').Reverse().Aggregate((c,e) => c + ' ' + e);
        }

        private void ProcessSymbol(string o)
        {
            switch (o)
            {
                case ")":
                    StackRightParenthesis();
                    break;
                case "(":
                    _operatorStack.Push(o);
                    break;
                default:
                    if (_operators.ContainsKey(o)) StackOperator(o);
                    else _outputStack.Push(o);
                    break;
            }
        }

        private void StackRightParenthesis()
        {
            while (_operatorStack.Peek() != "(")
                _outputStack.Push(_operatorStack.Pop());
            _operatorStack.Pop();
        }

        private static bool OperatorHasPriority(Operator a, Operator b)
        {
            return a.LeftAssociativity ? a.Precedence <= b.Precedence : a.Precedence < b.Precedence;
        }

        private void StackOperator(string operatorKey)
        {
            while (_operatorStack.Any() 
                && OperatorHasPriority(_operators[operatorKey], _operators[_operatorStack.Peek()]))
                _outputStack.Push(_operatorStack.Pop());
            _operatorStack.Push(operatorKey);
        }
    }
}
