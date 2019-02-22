using System;
using System.Reflection;

namespace Lib.Mocked.DependenciesFactory
{
    internal interface IDependencySpecification
    {
        object Dependency { get; }
        Type DependencyType { get; }

        bool ParameterInfoMatches(ParameterInfo parameterInfo);
    }
}
