using System;
using System.Diagnostics;
using System.Reflection;

namespace Lib.Mocked.DependenciesFactory
{
    internal class DependencySpecification : IDependencySpecification
    {
        public DependencySpecification(object dependency)
        {
            Debug.Assert(dependency != null);
            
            Dependency = dependency;
        }

        public bool ParameterInfoMatches(ParameterInfo parameterInfo)
        {
            if (!parameterInfo.ParameterType.IsAssignableFrom(DependencyType)) return false;
            return true;
        }
        
        private object Dependency { get; }
        public Type DependencyType => Dependency.GetType();
        object IDependencySpecification.Dependency => Dependency;
    }
}
