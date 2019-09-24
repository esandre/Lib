using Lib.ACL.Rule;

namespace Lib.ACL
{
    public interface IObject : IRuleTarget
    {
        IObjectGroup[] Groups { get; }
    }
}
