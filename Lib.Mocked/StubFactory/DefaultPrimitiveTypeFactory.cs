using System;

namespace Lib.Mocked.StubFactory
{
    internal class DefaultPrimitiveTypeFactory : IStubFactory
    {
        public bool CanFactory(Type t) => t.IsPrimitive;

        public object Factory(Type t) => Activator.CreateInstance(t);
    }
}
