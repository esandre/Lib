using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Lib.SQL.Log
{
    public class CommandChannelLogger : ICommandChannel
    {
        private readonly ICommandChannel _logged;
        private readonly ILogger<ICommandChannel> _logger;

        public CommandChannelLogger(ICommandChannel logged, ILogger<ICommandChannel> logger)
        {
            _logged = logged;
            _logger = logger;
        }

        public void ExecuteInTransaction(Func<ICommandChannel, TransactionResult> whatToDo)
        {
            TransactionResult OverridenWhatToDo(ICommandChannel channel)
            {
                using var scope = _logger.BeginScope("Transaction {Guid}", Guid.NewGuid());

                var innerResult = whatToDo(new CommandChannelLogger(channel, _logger));
                _logger.LogDebug("Transaction finished with result {Result}", innerResult);

                return innerResult;
            }

            _logged.ExecuteInTransaction(OverridenWhatToDo);
        }

        public IConvertible LastInsertedId
        {
            get
            {
                var value = _logged.LastInsertedId;
                _logger.LogTrace(nameof(LastInsertedId) + " is {Value}", value);
                return value;
            }
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

        public int Execute(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using var scope = _logger.BeginScope("Query {Guid} of type {Type}", Guid.NewGuid(), nameof(Execute));
            
            var enumeratedParameters = parameters?.ToArray() ?? new KeyValuePair<string, object>[0];
            LogInputParameters(sql, enumeratedParameters);

            var value = _logged.Execute(sql, enumeratedParameters);
            _logger.LogDebug("{Number} lines returned.", value);
            return value;
        }

        public object FetchValue(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using var scope = _logger.BeginScope("Query {Guid} of type {Type}", Guid.NewGuid(), nameof(FetchValue));

            var enumeratedParameters = parameters?.ToArray() ?? new KeyValuePair<string, object>[0];
            LogInputParameters(sql, enumeratedParameters);

            var value = _logged.FetchValue(sql, enumeratedParameters);
            _logger.LogDebug("{Value} returned.", value);
            return value;
        }

        public IReadOnlyDictionary<string, object> FetchLine(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using var scope = _logger.BeginScope("Query {Guid} of type {Type}", Guid.NewGuid(), nameof(FetchLine));

            var enumeratedParameters = parameters?.ToArray() ?? new KeyValuePair<string, object>[0];
            LogInputParameters(sql, enumeratedParameters);

            var line = _logged.FetchLine(sql, enumeratedParameters);
            _logger.LogDebug("{Line} returned", line);
            return line;
        }

        public IReadOnlyList<IReadOnlyDictionary<string, object>> FetchLines(string sql, IEnumerable<KeyValuePair<string, object>> parameters = null)
        {
            using var scope = _logger.BeginScope("Query {Guid} of type {Type}", Guid.NewGuid(), nameof(FetchLines));

            var enumeratedParameters = parameters?.ToArray() ?? new KeyValuePair<string, object>[0];
            LogInputParameters(sql, enumeratedParameters);

            var lines = _logged.FetchLines(sql, enumeratedParameters);
            _logger.LogDebug("{Lines} returned", lines);
            return lines;
        }
    }
}
