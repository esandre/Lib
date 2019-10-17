using Lib.SQL.Executor;
using Lib.SQL.Executor.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.Executor
{
    [TestClass]
    public class TestMultipleLinesExecutor : TestExecutorAbstractAbstract<MultipleLinesExecutor, ResultLines>
    {
        [TestMethod]
        public void TestMultipleLines()
        {
            var result = new MultipleLinesExecutor { Adapter = new TestAdapter() }.Execute("");
            Assert.AreEqual("b", result[0]["a"].ToString());
            Assert.AreEqual("d", result[0]["c"].ToString());
            Assert.AreEqual("f", result[1]["a"].ToString());
            Assert.AreEqual("h", result[1]["c"].ToString());
        }
    }
}
