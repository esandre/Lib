using System;
using System.Reflection;
using Lib.Reflection;
using Moq;

namespace Lib.Mocked.Moq
{
    internal class MoqBuilder
    {
        private readonly DefaultValueProvider _valueProvider;
        private object[] _parameters = Array.Empty<object>();
        private bool _callBase;
        private bool _recursivelyStubbed;
        private readonly MethodInfo _buildMethod;
        private MockBehavior _behavior = MockBehavior.Default;
        private bool _idempotent;

        public MoqBuilder(DefaultValueProvider defaultValueProvider, Type builtType)
        {
            _valueProvider = defaultValueProvider;

            _buildMethod = typeof(MoqBuilder)
                .GetGenericMethod("Build", BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(builtType);
        }

        public MoqBuilder CallBase()
        {
            _callBase = true;
            return this;
        }

        public MoqBuilder Loose()
        {
            _behavior = MockBehavior.Loose;
            return this;
        }

        public MoqBuilder RecursivelyStubbed()
        {
            _recursivelyStubbed = true;
            return this;
        }

        public MoqBuilder WithDependencies(params object[] parameters)
        {
            _parameters = parameters;
            return this;
        }

        public MoqBuilder Idempotent()
        {
            _idempotent = true;
            return this;
        }

        public object Build()
        {
            return _buildMethod.Invoke(this, new object[] {_parameters});
        }

        // ReSharper disable once UnusedMember.Local
        private TMock Build<TMock>(params object[] parameters) where TMock : class
        {
            var mock = new Mock<TMock>(_behavior, parameters)
            {
                CallBase = _callBase
            };

            if (_recursivelyStubbed) mock.DefaultValueProvider = _valueProvider;
            if (_idempotent)
            {
                var equatable = mock.As<IEquatable<TMock>>();
                var self = mock.Object;

                // ReSharper disable EqualExpressionComparison
                mock.Setup(m => m.Equals(self)).Returns(true);
                equatable.Setup(m => m.Equals(self)).Returns(true);
                // ReSharper restore EqualExpressionComparison
            }

            return mock.Object;
        }
    }
}