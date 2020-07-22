using System;

namespace Lib.ACL.Rule
{
    public class Rule : IRule
    {
        private ITimeSlot TimeSlot { get; }
        private IMatcher<ISubject> Subject { get; }
        private IMatcher<IObject> Objet { get; }
        public bool Authorize { get; }

        public bool IsApplicableFor(ISubject subject, IObject @object, DateTime at)
            => Subject.Includes(subject) && Objet.Includes(@object) && TimeSlot.Contains(at);

        public ushort Priority { get; }

        public Rule(
            IMatcher<ISubject> subject,
            bool isSubjectAGroup,
            IMatcher<IObject> @object, 
            bool isObjectAGroup,
            ITimeSlot timeSlot,
            bool authorize)
        {
            Subject = subject;
            Objet = @object;
            TimeSlot = timeSlot;
            Authorize = authorize;

            Priority = (ushort) ((TimeSlot.IsAbsolute ? 0x1 : 0x0) +
                                 (isObjectAGroup ? 0x0 : 0x2) +
                                 (isSubjectAGroup ? 0x0 : 0x4) +
                                 (authorize ? 0x8 : 0x0));
        }

        public int CompareTo(IRule other) => Priority - other.Priority;
    }
}
