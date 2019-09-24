using System;

namespace Lib.ACL.Rule
{
    public interface IRule : IEquatable<IRule>
    {
        ITimeSlot TimeSlot { get; }
        IRightBearer Subject { get; }
        IRuleTarget Objet { get; }
        DateTime CreationDate { get; }
        bool Authorize { get; }
        int Priority { get; }
    }
}
