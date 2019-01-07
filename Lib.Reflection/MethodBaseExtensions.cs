using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Lib.Reflection
{
    public static class MethodBaseExtensions
    {
        [PublicAPI]
        public static ParameterInfo[] GetRequiredParameters(this MethodBase method)
        {
            return method.GetParameters().Where(parameter => !parameter.IsOptional).ToArray();
        }
    }
}
