using System.Linq;
using System.Reflection;
using Lib.Mocked.StubFactory;

namespace Lib.Mocked.DependenciesFactory
{
    internal class StubDependenciesFactory : IDependenciesFactory
    {
        private readonly IStubFactory _stubFactory;

        public StubDependenciesFactory(IStubFactory stubFactory)
        {
            _stubFactory = stubFactory;
        }

        public object[] Factory(ConstructorInfo input)
            => input.GetParameters()
                .AsParallel()
                .Select(parameterInfo => _stubFactory.Factory(parameterInfo.ParameterType))
                .ToArray();
    }
}
