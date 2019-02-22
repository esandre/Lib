using System;
using Lib.Mocked.Moq;
using Lib.Patterns;

namespace Lib.Mocked.StubFactory
{
    internal class InterfaceStubFactory : IStubFactory
    {
        private readonly IFactory<Type, MoqBuilder> _moqBuilderFactory;

        public InterfaceStubFactory(IFactory<Type, MoqBuilder> moqBuilderFactory)
        {
            _moqBuilderFactory = moqBuilderFactory;
        }

        public virtual bool CanFactory(Type t) => t.IsInterface;

        public object Factory(Type t)
        {
            return _moqBuilderFactory.Factory(t)
                .Loose()
                .Idempotent()
                .RecursivelyStubbed()
                .Build();
        }
    }
}
