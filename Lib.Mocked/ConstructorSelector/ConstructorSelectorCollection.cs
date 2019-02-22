using System;
using System.Linq;
using System.Reflection;

namespace Lib.Mocked.ConstructorSelector
{
    internal class ConstructorSelectorCollection : IConstructorSelector
    {
        private readonly IConstructorSelector[] _elements;

        public ConstructorSelectorCollection(params IConstructorSelector[] elements)
        {
            _elements = elements;
        }

        public ConstructorInfo Factory(Type input) =>
            _elements
                .First(element => element.CanFactory(input))
                .Factory(input);

        public bool CanFactory(Type input)
        {
            try
            {
                return _elements.SingleOrDefault(element => element.CanFactory(input)) != null;
            }
            catch (InvalidOperationException)
            {
                throw new AmbiguousMatchException($"Multiple constructors for type {input} and no default one to pick");
            }
        }
    }
}
