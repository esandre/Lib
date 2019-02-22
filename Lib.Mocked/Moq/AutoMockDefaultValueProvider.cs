using System;
using Lib.Mocked.StubFactory;
using Moq;

namespace Lib.Mocked.Moq
{
    /// <summary>
    /// Loopbacks Mock's DefaultValueProvider to AutoMock
    /// </summary>
    internal class AutoMockDefaultValueProvider : DefaultValueProvider
    {
        private readonly IStubFactory _stubFactory;

        public AutoMockDefaultValueProvider(IStubFactory stubFactory)
        {
            _stubFactory = stubFactory;
        }

        protected override object GetDefaultValue(Type type, Mock mock) => _stubFactory.Factory(type);
    }
}
