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
            return _elements.Any(element => element.CanFactory(input));
        }
    }
}
