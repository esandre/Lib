using System;

namespace Lib.ACL.Rule
{
    public interface IRule : IComparable<IRule>
    {
        bool Authorize { get; }
        bool IsApplicableFor(ISubject subject, IObject @object, DateTime at);
        ushort Priority { get; }
    }
}
