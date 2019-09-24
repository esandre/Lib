using System;

namespace Lib.ACL
{
    public interface ITimeSlot : IEquatable<ITimeSlot>
    {
        bool Contains(DateTime datetime);
        bool IsAbsolute { get; }
    }
}
