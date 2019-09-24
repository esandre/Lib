using System;

namespace Lib.ACL.Rule
{
    public struct Rule : IRule
    {
        public ITimeSlot TimeSlot { get; }
        public IRightBearer Subject { get; }
        public IRuleTarget Objet { get; }
        public DateTime CreationDate { get; }
        public bool Authorize { get; }
        public int Priority { get; }

        public Rule(IRightBearer subject, IRuleTarget target, ITimeSlot timeSlot, bool authorize, DateTime creationDate)
        {
            CreationDate = creationDate;

            Subject = subject;
            Objet = target;
            TimeSlot = timeSlot;
            Authorize = authorize;

            Priority = (TimeSlot.IsAbsolute ? 0x1 : 0x0) +
                               (Objet is IObject ? 0x2 : 0x0) +
                               (Subject is ISubject ? 0x4 : 0x00);
        }

        public bool Equals(IRule other)
        {
            return other != null
                   && Authorize == other.Authorize
                   && TimeSlot.Equals(other.TimeSlot)
                   && Subject.Equals(other.Subject)
                   && Objet.Equals(other.Objet)
                   && CreationDate.Equals(other.CreationDate);
        }
    }
}
