using System;
using Lib.Mocked.Moq;
using Lib.Patterns;
using Lib.Reflection;

namespace Lib.Mocked.StubFactory
{
    internal class TypeWithSingleParameterlessConstructorFactory : InterfaceStubFactory
    {
        public override bool CanFactory(Type t) => t.IsClass && t.GetParameterlessContructor() != null;

        public TypeWithSingleParameterlessConstructorFactory(IFactory<Type, MoqBuilder> moqBuilderFactory) : base(moqBuilderFactory)
        {
        }
    }
}
