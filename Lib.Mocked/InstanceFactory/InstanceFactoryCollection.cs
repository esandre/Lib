using System.Linq;
using System.Reflection;

namespace Lib.Mocked.InstanceFactory
{
    internal class InstanceFactoryCollection : IInstanceFactory
    {
        private readonly IInstanceFactory[] _factories;

        public InstanceFactoryCollection(params IInstanceFactory[] factories)
        {
            _factories = factories;
        }

        public object Factory((ConstructorInfo constructor, object[] dependencies) input)
            => _factories.FirstOrDefault(factory => factory.CanFactory(input))?.Factory(input) 
               ?? throw new AmbiguousMatchException($"Cannot instanciate type {input.constructor.DeclaringType} with arguments {string.Join(",", input.dependencies)}");
        
        public bool CanFactory((ConstructorInfo constructor, object[] dependencies) input)
            => _factories.Any(factory => factory.CanFactory(input));
    }
}
