using System;
using Lib.Mocked.StubFactory;
using Lib.Patterns;

namespace Lib.Mocked.Moq
{
    internal class MoqBuilderFactory : IFactory<Type, MoqBuilder>
    {
        private readonly IStubFactory _stubFactory;

        public MoqBuilderFactory(IStubFactory stubFactory)
        {
            _stubFactory = stubFactory;
        }

        public MoqBuilder Factory(Type input)
        {
            return new MoqBuilder(new AutoMockDefaultValueProvider(_stubFactory), input);
        }
    }
}
