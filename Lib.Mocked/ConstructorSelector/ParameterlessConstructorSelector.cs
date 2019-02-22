using System;
using System.Reflection;
using Lib.Reflection;

namespace Lib.Mocked.ConstructorSelector
{
    internal class ParameterlessConstructorSelector : IConstructorSelector
    {
        public bool CanFactory(Type input) => Factory(input) != null;

        public ConstructorInfo Factory(Type input) => input.GetParameterlessContructor();
    }
}
