using Lib.ACL.Rule;

namespace Lib.ACL
{
    public interface ISubject : IRightBearer
    {
        bool AuthorizedByDefault { get; }
        ISubjectGroup[] Groups { get; }
    }
}
