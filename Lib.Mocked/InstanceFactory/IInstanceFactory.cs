using System.Reflection;
using Lib.Patterns;

namespace Lib.Mocked.InstanceFactory
{
    internal interface IInstanceFactory : IResponsibleFactory<(ConstructorInfo constructor, object[] dependencies), object>
    {
    }
}
