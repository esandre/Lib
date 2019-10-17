using System;
using System.Collections.Generic;
using Lib.SQL.Adapter;
using Lib.SQL.Adapter.Session;
using Lib.SQL.Executor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lib.SQL.Test.Executor
{
    public class TestExecutorAbstractAbstract<TTestedType, TResultType> where TTestedType : ExecutorAbstract<TResultType>, new()
    {
        internal class TestAdapter : DbAdapter
        {
            public TestAdapter() : base(new TestConnexion())
            {
            }
        }

        private class TestConnexion : IConnection
        {
            public ITransaction BeginTransaction()
            {
                throw new NotImplementedException();
            }

            public long LastInsertedId
            {
                get { return 1; }
            }

            public int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters)
            {
                return 1;
            }

            public object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters)
            {
                switch (sql)
                {
                    case "int":
                        return 1;
                    case "string":
                        return "str";
                    case "date":
                        return DateTime.MinValue;
                    case "double":
                        return 8.0;
                }
                return null;
            }

            public IDictionary<string, object> FetchLine(string sql, IEnumerable<KeyValuePair<string, object>> parameters)
            {
                return new Dictionary<string, object> {{"a", "b"}, {"c", "d"}};
            }

            public IEnumerable<IDictionary<string, object>> FetchLines(string sql, IEnumerable<KeyValuePair<string, object>> parameters)
            {
                var l1 = new Dictionary<string, object> { { "a", "b" }, { "c", "d" } };
                var l2 = new Dictionary<string, object> { { "a", "f" }, { "c", "h" } };
                return new List<Dictionary<string, object>> {l1, l2};
            }

            public void Open()
            {
            }

            public void Close()
            {
            }

            public void Dispose()
            {
            }
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestNullAdapterException()
        {
            new TTestedType().Execute("");
        }
    }
}
