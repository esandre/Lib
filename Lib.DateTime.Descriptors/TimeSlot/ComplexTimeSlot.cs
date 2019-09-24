using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lib.DateTime.Descriptors.Algorithms;
using Lib.DateTime.Descriptors.TimeSlot.Relationship;

namespace Lib.DateTime.Descriptors.TimeSlot
{
    public class ComplexTimeSlot : ITimeSlot
    {
        private readonly ITimeSlot _tree;

        public static ComplexTimeSlot CreateOr(ITimeSlot a, ITimeSlot b)
        {
            return new ComplexTimeSlot(new OrTimeSlotRelationship(a, b));
        }

        public static ComplexTimeSlot CreateBut(ITimeSlot a, ITimeSlot b)
        {
            return new ComplexTimeSlot(new ButTimeSlotRelationship(a, b));
        }

        public static ComplexTimeSlot CreateIntersecting(ITimeSlot a, ITimeSlot b)
        {
            return new ComplexTimeSlot(new IntersectingTimeSlotRelationship(a, b));
        }

        private ComplexTimeSlot(ITimeSlot tree)
        {
            _tree = tree;
        }

        internal static ComplexTimeSlot FromDescriptor(string descriptor, IAliasProvider provider)
        {
            if (!descriptor.Contains(Descriptor.Keywords.Or) 
                && !descriptor.Contains(Descriptor.Keywords.But) 
                && !descriptor.Contains(Descriptor.Keywords.Intersecting)) 
                throw new Exception("Not a complex time slot");

            var words = CleanAndSplit(descriptor);
            var extracted = ExtractTimeslots(words);

            var rpn = new ShuntingYard(new Dictionary<string, Operator>
            {
                {Descriptor.Keywords.Or, new Operator(3, true)},
                {Descriptor.Keywords.Intersecting, new Operator(2, true)},
                {Descriptor.Keywords.But, new Operator(2, true)}
            }).ToReversePolishNotation(extracted.Key.Aggregate((c,e) => c + ' ' + e));

            return BuildFromReversePolish(rpn, extracted.Value, provider);
        }

        private static IEnumerable<string> CleanAndSplit(string descriptor)
        {
            if (descriptor.Count(c => c == '(') != descriptor.Count(c => c == ')')) throw new Exception("Parentesis error");

            //Séparation des parenthèses
            descriptor = descriptor.Replace("(", " ( ");
            descriptor = descriptor.Replace(")", " ) ");

            return descriptor.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }

        private static KeyValuePair<IList<string>, IList<string>> ExtractTimeslots(IEnumerable<string> words)
        {
            var output = new KeyValuePair<IList<string>, IList<string>>(new List<string>(), new List<string>());
            var current = string.Empty;

            Action stackCurrent = () =>
            {
                // ReSharper disable AccessToModifiedClosure
                if (current == string.Empty) return;
                output.Value.Add(current.TrimEnd(' '));
                // ReSharper restore AccessToModifiedClosure
                output.Key.Add("" + (output.Value.Count() - 1));
                current = string.Empty;
            };

            foreach (var word in words)
            {
                if (word == "(" || word == ")"
                    || word == Descriptor.Keywords.Or
                    || word == Descriptor.Keywords.But
                    || word == Descriptor.Keywords.Intersecting)
                {
                    if (word != "(") stackCurrent();
                    output.Key.Add(word);
                }
                else current += word + " ";
            }

            stackCurrent();
            return output;
        }

        private static ComplexTimeSlot BuildFromReversePolish(string rpn, IList<string> extracted, IAliasProvider provider)
        {
            var stack = new Stack<ITimeSlot>();
            foreach (var symbol in rpn.Split(' '))
            {
                if (symbol == Descriptor.Keywords.Or)
                {
                    var first = stack.Pop();
                    stack.Push(CreateOr(stack.Pop(), first));
                }
                else if (symbol == Descriptor.Keywords.But)
                {
                    var first = stack.Pop();
                    stack.Push(CreateBut(stack.Pop(), first));
                }
                else if (symbol == Descriptor.Keywords.Intersecting)
                {
                    var first = stack.Pop();
                    stack.Push(CreateIntersecting(stack.Pop(), first));
                }
                else stack.Push(new TimeSlotBuilder(provider).Build(extracted[(int.Parse(symbol))]));
            }

            return (ComplexTimeSlot)stack.Pop();
        }

        public bool Contains(System.DateTime date)
        {
            return _tree.Contains(date);
        }

        public bool IsAbsolute => _tree.IsAbsolute;

        public string ToString(CultureInfo culture)
        {
            return _tree.ToString(culture);
        }

        public override string ToString()
        {
            var str = _tree.ToString();
            if (str.StartsWith("(") && str.EndsWith(")")) return str.TrimStart('(').TrimEnd(')');
            return str;
        }

        #region equality

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && ToString().Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(ComplexTimeSlot left, ComplexTimeSlot right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ComplexTimeSlot left, ComplexTimeSlot right)
        {
            return !Equals(left, right);
        }

        #endregion
    }
}
