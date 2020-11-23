using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Lib.SQL.Log
{
    public class AsyncCommandChannelLogger : IAsyncCommandChannel
    {
        private readonly IAsyncCommandChannel _logged;
        private readonly ILogger<IAsyncCommandChannel> _logger;

        public AsyncCommandChannelLogger(IAsyncCommandChannel logged, ILogger<IAsyncCommandChannel> logger)
        {
            _logged = logged;
            _logger = logger;
        }

        public async Task ExecuteInTransactionAsync(Func<IAsyncCommandChannel, Task<TransactionResult>> whatToDo)
        {
            async Task<TransactionResult> OverridenWhatToDo(IAsyncCommandChannel channel)
            {
                using var scope = _logger.BeginScope("Transaction {Guid}", Guid.NewGuid());

                var innerResult = await whatToDo(new AsyncCommandChannelLogger(channel, _logger));
                _logger.LogDebug("Transaction finished with result {Result}", innerResult);

                return innerResult;
            }

            await _logged.ExecuteInTransactionAsync(OverridenWhatToDo);
        }

        public async Task<IConvertible> LastInsertedIdAsync()
        {
            var id = await _logged.LastInsertedIdAsync();
            _logger.LogTrace(nameof(LastInsertedIdAsync) + " is {Value}", id);
            return id;
        }

        private void LogInputParameters(string sql,
            IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace("Sent Sql:{Sql}, Parameters:{Parameters}", sql, parameters);
            }
            else
            {
                _logger.LogDebug("Sent Sql:{Sql}", sql);
            }
        }

        public async Task<int> ExecuteAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using var scope = _logger.BeginScope("Query {Guid} of type {Type}", Guid.NewGuid(), nameof(ExecuteAsync));

            var enumeratedParameters = parameters?.ToArray() ?? new KeyValuePair<string, object>[0];
            LogInputParameters(sql, enumeratedParameters);
            var lines = await _logged.ExecuteAsync(sql, enumeratedParameters);
            return lines;
        }

        public async Task<object> FetchValueAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using var scope = _logger.BeginScope("Query {Guid} of type {Type}", Guid.NewGuid(), nameof(FetchValueAsync));

            var enumeratedParameters = parameters?.ToArray() ?? new KeyValuePair<string, object>[0];
            LogInputParameters(sql, enumeratedParameters);
            var value = await _logged.FetchValueAsync(sql, enumeratedParameters);

            return value;
        }

        public async Task<IReadOnlyDictionary<string, object>> FetchLineAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using var scope = _logger.BeginScope("Query {Guid} of type {Type}", Guid.NewGuid(), nameof(FetchLineAsync));

            var enumeratedParameters = parameters?.ToArray() ?? new KeyValuePair<string, object>[0];
            LogInputParameters(sql, enumeratedParameters);
            var line = await _logged.FetchLineAsync(sql, enumeratedParameters);

            return line;
        }

        public async Task<IReadOnlyList<IReadOnlyDictionary<string, object>>> FetchLinesAsync(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using var scope = _logger.BeginScope("Query {Guid} of type {Type}", Guid.NewGuid(), nameof(FetchLinesAsync));

            var enumeratedParameters = parameters?.ToArray() ?? new KeyValuePair<string, object>[0];
            LogInputParameters(sql, enumeratedParameters);
            var lines = await _logged.FetchLinesAsync(sql, enumeratedParameters);

            return lines;
        }
    }
}
