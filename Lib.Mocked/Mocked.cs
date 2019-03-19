using System.Collections.Generic;
using System.Linq;
using Lib.Mocked.ConstructorSelector;
using Lib.Mocked.DependenciesFactory;
using Lib.Mocked.InstanceFactory;
using Lib.Mocked.Moq;
using Lib.Mocked.StubFactory;

namespace Lib.Mocked
{
    /// <summary>
    /// Generates instances of concrete or abstract types, with mock dependencies.
    /// Mock dependencies can be overriden easily.
    /// </summary>
    public static class Mocked
    {
        private static readonly StubFactoryCollection DefaultStubFactory;
        private static readonly IConstructorSelector DefaultConstructorSelector;
        private static readonly IDependenciesFactory DefaultDependenciesFactory;
        private static readonly IInstanceFactory DefaultInstanceFactory;

        private static readonly MockedInstanceFactory DefaultMockedInstanceFactory;

        static Mocked()
        {
            var stubFactoryCollection = new StubFactoryCollection();
            var moqBuilderFactory = new MoqBuilderFactory(stubFactoryCollection);

            stubFactoryCollection.Append(new EmptyStringFactory());
            stubFactoryCollection.Append(new DefaultDateFactory());
            stubFactoryCollection.Append(new DefaultPrimitiveTypeFactory());
            stubFactoryCollection.Append(new NullableFactory());
            stubFactoryCollection.Append(new InterfaceStubFactory(moqBuilderFactory));
            stubFactoryCollection.Append(new TypeWithSingleParameterlessConstructorFactory(moqBuilderFactory));
            stubFactoryCollection.Append(new TypeWithSingleConstructorFactory(moqBuilderFactory, stubFactoryCollection));

            DefaultStubFactory = stubFactoryCollection;
            DefaultDependenciesFactory = new StubDependenciesFactory(stubFactoryCollection);

            DefaultInstanceFactory = new InstanceFactoryCollection(
                new ConcreteInstanceFactory(),
                new AbstractClassFactory(moqBuilderFactory)
            );

            DefaultConstructorSelector = new ConstructorSelectorCollection(
                new ParameterlessConstructorSelector(), 
                new SingleConstructorSelector()
            );

            DefaultMockedInstanceFactory = new MockedInstanceFactory(
                DefaultConstructorSelector,
                DefaultDependenciesFactory,
                DefaultInstanceFactory
            );
        }

        /// <summary>
        /// Add a StubFactory to handle a specific dependency type.
        /// </summary>
        public static void AddStubFactory(IStubFactory element) => DefaultStubFactory.Prepend(element);
        
        /// <summary>
        /// New mocked instances with all its dependencies as mock
        /// </summary>
        public static TMocked New<TMocked>() => DefaultMockedInstanceFactory.Factory<TMocked>();

        /// <summary>
        /// New mocked instances with some dependencies manually overriden
        /// </summary>
        public static TMocked New<TMocked>(params object[] overridenDependencies)
            => new MockedInstanceFactory(
                    DefaultConstructorSelector,
                    DefaultDependenciesFactory,
                    DefaultInstanceFactory)
                .Factory<TMocked>(overridenDependencies.Select(dependency => new DependencySpecification(dependency)));

        /// <summary>
        /// Generate a new single Stub of selected type
        /// </summary>
        public static TStubbed NewStub<TStubbed>() => (TStubbed) DefaultStubFactory.Factory(typeof(TStubbed));

        /// <summary>
        /// Generate multiple stubs of selected type
        /// </summary>
        public static TStubbed[] NewStubs<TStubbed>(int count) => GenerateStubs<TStubbed>().Take(count).ToArray();

        private static IEnumerable<TStubbed> GenerateStubs<TStubbed>()
        {
            while (true) yield return NewStub<TStubbed>();
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
