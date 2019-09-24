namespace Lib.DateTime.Descriptors.TimeSlot.Relationship
{
    public class OrTimeSlotRelationship : TimeSlotRelationshipAbstract
    {
        public OrTimeSlotRelationship(ITimeSlot a, ITimeSlot b) : base(a, b)
        {
        }

        protected override bool ApplyOperator(bool aContains, bool bContains)
        {
            return aContains || bContains;
        }

        protected override string OperatorString => Descriptor.Keywords.Or;
    }
}
