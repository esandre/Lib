using System;

namespace Lib.ACL.Rule
{
    public interface IRuleTarget : IEquatable<IRightBearer>
    {
        string UUID { get; }
    }
}
