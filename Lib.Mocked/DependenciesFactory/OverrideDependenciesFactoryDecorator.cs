using System;
using System.Linq;
using System.Reflection;

namespace Lib.Mocked.DependenciesFactory
{
    internal class OverrideDependenciesFactoryDecorator : IDependenciesFactory
    {
        private readonly IDependenciesFactory _decorated;
        private readonly IDependencySpecification[] _overrides;

        public OverrideDependenciesFactoryDecorator(
            IDependenciesFactory decorated, 
            params IDependencySpecification[] overrides)
        {
            _decorated = decorated;
            _overrides = overrides;
        }

        public object[] Factory(ConstructorInfo input)
        {
            var defaultDependencies = _decorated.Factory(input);
            return MergeOverridenDependencies(defaultDependencies, input.GetParameters());
        }

        private object[] MergeOverridenDependencies(object[] defaultDependencies, ParameterInfo[] requirements)
        {
            var requirementsList = requirements.ToList();
            var dependencies = defaultDependencies.ToList();

            foreach (var dependencySpecification in _overrides)
            {
                var position = requirementsList.FindIndex(dependencySpecification.ParameterInfoMatches);

                if (position < 0) throw new ArgumentException("Trying to replace inexistant dependency. " +
                                                              $"Type : {dependencySpecification.DependencyType}");

                dependencies[position] = dependencySpecification.Dependency;
            }

            return dependencies.ToArray();
        }
    }
}
