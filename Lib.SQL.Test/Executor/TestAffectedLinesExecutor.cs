using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.Executor
{
    [TestClass]
    public class TestAffectedLinesExecutor : TestExecutorAbstractAbstract<AffectedLinesExecutor, int>
    {
        [TestMethod]
        public void TestLastinsertedId()
        {
            var executor = new AffectedLinesExecutor { Adapter = new TestAdapter() };
            Assert.AreEqual(1, executor.Execute(""));
        }
    }
}
