using System.Collections.Generic;
using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lib.SQL.Test
{
    [TestClass]
    public class TestSingleLineExecutor
    {
        [TestMethod]
        public void TestSingleLine()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchLine(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()) ==
                new Dictionary<string, object> {{"a", "b"}, {"c", "d"}}
            );

            var result = new SingleLineExecutor().ExecuteOnAdapter(commandChannel, "");
            Assert.AreEqual("b", result["a"].ToString());
            Assert.AreEqual("d", result["c"].ToString());
        }
    }
}
