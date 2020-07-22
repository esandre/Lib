using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.Executor
{
    [TestClass]
    public class TestSingleColumnExecutor
    {
        [TestMethod]
        public void TestSingleColumn()
        {
            var result = new SingleColumnExecutor
            {
                Adapter = new TestExecutorAbstractAbstract<SingleColumnExecutor, object[]>.TestAdapter()
            }.Execute("");

            Assert.AreEqual("b", result[0]);
            Assert.AreEqual("f", result[1]);
        }
    }
}
