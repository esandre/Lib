using System;

namespace Lib.ACL.Rule
{
    public interface IRightBearer : IEquatable<IRightBearer>
    {
        string UUID { get; }
    }
}
