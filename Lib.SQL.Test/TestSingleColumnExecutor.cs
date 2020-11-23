using System.Collections.Generic;
using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lib.SQL.Test
{
    [TestClass]
    public class TestSingleColumnExecutor
    {
        [TestMethod]
        public void TestSingleColumn()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchLines(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()) ==
                new[]
                {
                    new Dictionary<string, object> {{"_", "b"}},
                    new Dictionary<string, object> {{"_", "f"}},
                }
            );

            var result = new SingleColumnExecutor().ExecuteOnAdapter(commandChannel, "");

            Assert.AreEqual("b", result[0]);
            Assert.AreEqual("f", result[1]);
        }
    }
}
