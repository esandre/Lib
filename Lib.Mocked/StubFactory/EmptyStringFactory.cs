using System;

namespace Lib.Mocked.StubFactory
{
    internal class EmptyStringFactory : IStubFactory
    {
        public bool CanFactory(Type t) => t == typeof(string);

        public object Factory(Type t) => string.Empty;
    }
}
