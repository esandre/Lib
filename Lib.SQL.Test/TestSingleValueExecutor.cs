using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
// ReSharper disable PossibleUnintendedReferenceComparison

namespace Lib.SQL.Test
{
    [TestClass]
    public class TestSingleValueExecutor
    {
        [TestMethod]
        public async Task TestString()
        {
            var commandChannel = Mock.Of<IAsyncCommandChannel>(m =>
                    m.FetchValueAsync(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, IConvertible>>>()) 
                    == Task.FromResult<IConvertible>("str")
            );

            var executor = new SingleValueExecutor();
            Assert.AreEqual("str", await executor.ExecuteOnAdapterAsync(commandChannel, "string"));
        }

        [TestMethod]
        public async Task TestInt()
        {
            var commandChannel = Mock.Of<IAsyncCommandChannel>(m =>
                m.FetchValueAsync(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, IConvertible>>>()) 
                == Task.FromResult<IConvertible>(1)
            );

            var executor = new SingleValueExecutor();
            Assert.AreEqual(1, await executor.ExecuteOnAdapterAsync(commandChannel, "int"));
        }

        [TestMethod]
        public async Task TestDouble()
        {
            var commandChannel = Mock.Of<IAsyncCommandChannel>(m =>
                m.FetchValueAsync(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, IConvertible>>>()) 
                == Task.FromResult<IConvertible>(8.0)
            );

            var executor = new SingleValueExecutor();
            Assert.AreEqual(8.0, await executor.ExecuteOnAdapterAsync(commandChannel, "double"));
        }

        [TestMethod]
        public async Task TestDateTime()
        {
            var commandChannel = Mock.Of<IAsyncCommandChannel>(m =>
                m.FetchValueAsync(It.IsAny<string>(), It.IsAny<IEnumerable<KeyValuePair<string, IConvertible>>>()) 
                == Task.FromResult((IConvertible) DateTime.MinValue)
            );

            var executor = new SingleValueExecutor();
            Assert.AreEqual(DateTime.MinValue, await executor.ExecuteOnAdapterAsync(commandChannel, "date"));
        }

        [TestMethod]
        public async Task TestExceptionsNotSwallowedAsync()
        {
            var executor = new SingleValueExecutor();

            var commandChannel = new Mock<IAsyncCommandChannel>();
            commandChannel
                .Setup(m => m.FetchValueAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, IConvertible>>()))
                .Throws<VerySpecificException>();

            await Assert.ThrowsExceptionAsync<VerySpecificException>(async () => await executor.ExecuteOnAdapterAsync(commandChannel.Object, ""));
        }

        private class VerySpecificException : Exception;
    }
}
