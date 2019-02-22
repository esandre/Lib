using System;
using System.Reflection;
using Lib.Patterns;

namespace Lib.Mocked.ConstructorSelector
{
    internal interface IConstructorSelector : IResponsibleFactory<Type, ConstructorInfo>
    {
    }
}
