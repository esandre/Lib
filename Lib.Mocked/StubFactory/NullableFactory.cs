using System;

namespace Lib.Mocked.StubFactory
{
    internal class NullableFactory : IStubFactory
    {
        public object Factory(Type input) => null;

        public bool CanFactory(Type input) => input.IsGenericType && input.GetGenericTypeDefinition() == typeof(Nullable<>);
    }
}
