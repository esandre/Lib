using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Lib.Console
{
    internal class AsyncCommandSyncAdapter : ICommand
    {
        private readonly IAsyncCommand _command;

        public AsyncCommandSyncAdapter(IAsyncCommand command)
        {
            _command = command;
        }

        public string CommandlineName => _command.CommandlineName;

        public void Execute(CommandLineApplication command)
        {
            var task = Task.Run(async () => await _command.ExecuteAsync(command));
            task.RunSynchronously();
        }
    }
}
