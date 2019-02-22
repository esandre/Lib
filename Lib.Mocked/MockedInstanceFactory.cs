using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lib.Mocked.ConstructorSelector;
using Lib.Mocked.DependenciesFactory;
using Lib.Mocked.InstanceFactory;

namespace Lib.Mocked
{
    internal class MockedInstanceFactory
    {
        private readonly IConstructorSelector _constructorSelector;
        private readonly IDependenciesFactory _dependenciesFactory;
        private readonly IInstanceFactory _instanceFactory;

        public MockedInstanceFactory(
            IConstructorSelector constructorSelector,
            IDependenciesFactory dependenciesFactory,
            IInstanceFactory instanceFactory)
        {
            _dependenciesFactory = dependenciesFactory;
            _constructorSelector = constructorSelector;
            _instanceFactory = instanceFactory;
        }

        public TMocked Factory<TMocked>()
        {
            var constructor = SelectConstructor(typeof(TMocked));
            var dependencies = CreateDependencies(constructor);
            return (TMocked) BuildInstance(constructor, dependencies);
        }

        public TMocked Factory<TMocked>(IEnumerable<IDependencySpecification> overrides)
        {
            var constructor = SelectConstructor(typeof(TMocked));
            var dependencies = CreateDependencies(constructor, overrides);
            return (TMocked)BuildInstance(constructor, dependencies);
        }

        private ConstructorInfo SelectConstructor(Type type)
        {
            if (!_constructorSelector.CanFactory(type))
                throw new InvalidOperationException(_constructorSelector + " cannot handle type " + type);

            return _constructorSelector.Factory(type);
        }

        private object[] CreateDependencies(ConstructorInfo constructor) => _dependenciesFactory.Factory(constructor);

        private object[] CreateDependencies(ConstructorInfo constructor, IEnumerable<IDependencySpecification> overrides)
        {
            var factory = new OverrideDependenciesFactoryDecorator(_dependenciesFactory, overrides.ToArray());
            return factory.Factory(constructor);
        }

        private object BuildInstance(ConstructorInfo constructor, object[] dependencies) =>
            _instanceFactory.Factory((constructor, dependencies));
    }
}
