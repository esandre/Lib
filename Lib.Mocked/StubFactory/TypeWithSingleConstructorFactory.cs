using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Lib.Mocked.Moq;
using Lib.Patterns;
using Lib.Reflection;

namespace Lib.Mocked.StubFactory
{
    internal class TypeWithSingleConstructorFactory : IStubFactory
    {
        private readonly IStubFactory _stubFactory;
        private readonly IFactory<Type, MoqBuilder> _moqBuilderFactory;

        public TypeWithSingleConstructorFactory(IFactory<Type, MoqBuilder> moqBuilderFactory, IStubFactory stubFactory)
        {
            _moqBuilderFactory = moqBuilderFactory;
            _stubFactory = stubFactory;
        }

        public bool CanFactory(Type t) => t.IsClass && t.GetConstructors().Length == 1;

        public object Factory(Type t)
        {
            var constructor = t.GetSingleConstructor();

            var parameterInfos = constructor.GetRequiredParameters();
            var parameters = parameterInfos.Select(StubFromParameterInfo).ToArray();

            return _moqBuilderFactory.Factory(t)
                .Loose()
                .Idempotent()
                .RecursivelyStubbed()
                .WithDependencies(parameters)
                .Build();
        }

        private object StubFromParameterInfo(ParameterInfo info)
        {
            var defaultValueAttribute = info.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultValueAttribute == null) return _stubFactory.Factory(info.ParameterType);

            Debug.Assert(info.ParameterType.IsInstanceOfType(defaultValueAttribute.Value), 
                "info.ParameterType not assignable from defaultValueAttribute.Value");
            return defaultValueAttribute.Value;
        }
    }
}
