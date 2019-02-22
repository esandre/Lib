using System.Diagnostics;
using System.Reflection;

namespace Lib.Mocked.InstanceFactory
{
    internal class ConcreteInstanceFactory : IInstanceFactory
    {
        public bool CanFactory((ConstructorInfo constructor, object[] dependencies) input)
        {
            var (constructor, _) = input;
            Debug.Assert(constructor.DeclaringType != null, "input.constructor.DeclaringType != null");
            return !constructor.DeclaringType.IsAbstract;
        }

        public object Factory((ConstructorInfo constructor, object[] dependencies) input)
        {
            try
            {
                var (constructor, dependencies) = input;
                return constructor.Invoke(dependencies);
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException == null) throw;
                throw e.InnerException;
            }
        }
    }
}
