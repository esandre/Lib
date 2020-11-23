using System.Collections.Generic;
using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lib.SQL.Test
{
    [TestClass]
    public class TestMultipleLinesExecutor
    {
        [TestMethod]
        public void TestMultipleLines()
        {
            var commandChannel = Mock.Of<ICommandChannel>(m =>
                m.FetchLines(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, object>>>()) ==
                new[]
                {
                    new Dictionary<string, object> {{"a", "b"}, {"c", "d"}},
                    new Dictionary<string, object> {{"a", "f"}, {"c", "h"}},
                }
            );

            var result = new MultipleLinesExecutor().ExecuteOnAdapter(commandChannel, "");
            Assert.AreEqual("b", result[0]["a"].ToString());
            Assert.AreEqual("d", result[0]["c"].ToString());
            Assert.AreEqual("f", result[1]["a"].ToString());
            Assert.AreEqual("h", result[1]["c"].ToString());
        }
    }
}
