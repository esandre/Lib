using System.Reflection;
using Lib.Patterns;

namespace Lib.Mocked.DependenciesFactory
{
    internal interface IDependenciesFactory : IFactory<ConstructorInfo, object[]>
    {
    }
}
