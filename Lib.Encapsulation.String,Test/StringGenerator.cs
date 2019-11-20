using System.Linq;

namespace Lib.Encapsulation.String.Test
{
    internal static class StringGenerator
    {
        public static string GenerateDesiredLengthString(int length) => string.Concat(Enumerable.Repeat('x', length));
    }
}
