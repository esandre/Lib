using Lib.SQL.Executor;
using Lib.SQL.Executor.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.Executor
{
    [TestClass]
    public class TestSingleLineExecutor : TestExecutorAbstractAbstract<SingleLineExecutor, ResultLine>
    {
        [TestMethod]
        public void TestSingleLine()
        {
            var result = new SingleLineExecutor { Adapter = new TestAdapter() }.Execute("");
            Assert.AreEqual("b", result["a"].ToString());
            Assert.AreEqual("d", result["c"].ToString());
        }
    }
}
