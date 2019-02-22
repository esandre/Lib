using System;
using System.Diagnostics;
using System.Reflection;
using Lib.Mocked.Moq;
using Lib.Patterns;

namespace Lib.Mocked.InstanceFactory
{
    internal class AbstractClassFactory : IInstanceFactory
    {
        private readonly IFactory<Type, MoqBuilder> _moqBuilderFactory;

        public AbstractClassFactory(IFactory<Type, MoqBuilder> moqBuilderFactory)
        {
            _moqBuilderFactory = moqBuilderFactory;
        }

        public bool CanFactory((ConstructorInfo constructor, object[] dependencies) input)
        {
            var (constructor, _) = input;
            Debug.Assert(constructor.DeclaringType != null, "input.constructor.DeclaringType != null");
            return constructor.DeclaringType.IsAbstract;
        }

        public object Factory((ConstructorInfo constructor, object[] dependencies) input)
        {
            var (constructor, dependencies) = input;

            var classType = constructor.DeclaringType;

            return _moqBuilderFactory.Factory(classType)
                .CallBase()
                .Loose()
                .WithDependencies(dependencies)
                .CallBase();
        }
    }
}
