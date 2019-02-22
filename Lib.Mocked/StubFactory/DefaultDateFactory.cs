using System;

namespace Lib.Mocked.StubFactory
{
    internal class DefaultDateFactory : IStubFactory
    {
        public bool CanFactory(Type t) => t == typeof(DateTime);

        public object Factory(Type t) => DateTime.MinValue;
    }
}
