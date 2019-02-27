using Lib.DateTime.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace Lib.DateTime.Test.Specialized
{
    public class SpecializedDateTimeAbstractTest<TChild> where TChild : SpecializedDateTimeAbstract
    {
        // ReSharper disable once StaticMemberInGenericType
        protected static System.Random Random { get; } = new System.Random();

        [TestMethod]
        public void IsImplicitly_ConvertibleFromDateTime()
        {
            Check.That(SpecializedDateTimeAbstract.FromDateTime<TChild>(System.DateTime.MinValue))
                .IsInstanceOf<TChild>();
        }

        [TestMethod]
        public void IsImplicitly_ConvertibleToNullDateTime()
        {
            var child = (TChild) null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Check.That((System.DateTime?) child).IsNull();
        }
    }
}
