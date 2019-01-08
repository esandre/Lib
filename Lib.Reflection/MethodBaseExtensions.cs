using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Lib.Reflection
{
    /// <summary>
    /// <see cref="MethodBase"/> extensions
    /// </summary>
    public static class MethodBaseExtensions
    {
        /// <summary>
        /// Returns all non-optional parameters of a method
        /// </summary>
        /// <param name="method">The method</param>
        /// <returns>Array of <see cref="ParameterInfo"/></returns>
        [PublicAPI]
        public static ParameterInfo[] GetRequiredParameters(this MethodBase method)
        {
            return method.GetParameters().Where(parameter => !parameter.IsOptional).ToArray();
        }
    }
}
