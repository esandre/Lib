namespace Lib.DateTime.Descriptors.TimeSlot.Relationship
{
    public class IntersectingTimeSlotRelationship : TimeSlotRelationshipAbstract
    {
        public IntersectingTimeSlotRelationship(ITimeSlot a, ITimeSlot b) : base(a, b)
        {
        }

        protected override bool ApplyOperator(bool aContains, bool bContains)
        {
            return aContains && bContains;
        }

        protected override string OperatorString => Descriptor.Keywords.Intersecting;
    }
}
