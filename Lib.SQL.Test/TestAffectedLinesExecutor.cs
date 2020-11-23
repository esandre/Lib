using System.Collections.Generic;
using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lib.SQL.Test
{
    [TestClass]
    public class TestAffectedLinesExecutor
    {
        [TestMethod]
        public void TestLastinsertedId()
        {
            var executor = new AffectedLinesExecutor();

            var commandChannel = Mock.Of<ICommandChannel>(m => m.Execute(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()) == 1);
            Assert.AreEqual(1, executor.ExecuteOnAdapter(commandChannel, ""));
        }
    }
}
