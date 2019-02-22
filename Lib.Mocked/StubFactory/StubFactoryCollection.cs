using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lib.Mocked.StubFactory
{
    internal class StubFactoryCollection : IStubFactory
    {
        private readonly IList<IStubFactory> _factories;

        public StubFactoryCollection()
        {
            _factories = new List<IStubFactory>();
        }

        public void Prepend(IStubFactory element) => _factories.Insert(0, element);
        public void Append(IStubFactory element) => _factories.Add(element);

        public bool CanFactory(Type t) => _factories.Any(sb => sb.CanFactory(t));

        public object Factory(Type t)
        {
            foreach (var mockBuilder in _factories)
                if (mockBuilder.CanFactory(t))
                    return mockBuilder.Factory(t);

            throw new AmbiguousMatchException($"Cannot stub selected type : {t} \n" +
                                              "Probably multiple constructors and no default one to pick");
        }
    }
}
