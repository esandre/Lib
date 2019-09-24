using System.Globalization;

namespace Lib.DateTime.Descriptors.TimeSlot.Relationship
{
    public abstract class TimeSlotRelationshipAbstract : ITimeSlot
    {
        private readonly ITimeSlot _a;
        private readonly ITimeSlot _b;

        protected TimeSlotRelationshipAbstract(ITimeSlot a, ITimeSlot b)
        {
            _a = a;
            _b = b;
        }

        public bool Contains(System.DateTime date)
        {
            return ApplyOperator(_a.Contains(date), _b.Contains(date));
        }

        protected abstract bool ApplyOperator(bool aContains, bool bContains);

        public bool IsAbsolute => _a.IsAbsolute && _b.IsAbsolute;

        protected abstract string OperatorString { get; }

        public override string ToString()
        {
            return "(" + _a + ' ' + OperatorString + ' ' + _b + ")";
        }

        public string ToString(CultureInfo culture)
        {
            return Descriptor.Translate(ToString(), CultureInfo.InvariantCulture, culture);
        }
    }
}
