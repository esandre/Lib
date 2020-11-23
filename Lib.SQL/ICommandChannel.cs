using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lib.SQL
{
    public interface ICommandChannel : ITransactionControl
    {
        IConvertible LastInsertedId { get; }

        int Execute(string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null);

        IConvertible FetchValue(string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null);

        IReadOnlyDictionary<string, IConvertible> FetchLine(string sql,
            IEnumerable<KeyValuePair<string, IConvertible>> parameters = null);

        IReadOnlyList<IReadOnlyDictionary<string, IConvertible>> FetchLines(string sql,
            IEnumerable<KeyValuePair<string, IConvertible>> parameters = null);
    }

    public interface IAsyncCommandChannel : IAsyncTransactionControl
    {
        Task<IConvertible> LastInsertedIdAsync();

        Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, IConvertible>> parameters = null);

        Task<IConvertible> FetchValueAsync(string sql,
            IEnumerable<KeyValuePair<string, IConvertible>> parameters = null);

        Task<IReadOnlyDictionary<string, IConvertible>> FetchLineAsync(string sql,
            IEnumerable<KeyValuePair<string, IConvertible>> parameters = null);

        Task<IReadOnlyList<IReadOnlyDictionary<string, IConvertible>>> FetchLinesAsync(string sql,
            IEnumerable<KeyValuePair<string, IConvertible>> parameters = null);
    }
}
