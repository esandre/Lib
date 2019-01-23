using Microsoft.Extensions.CommandLineUtils;

namespace Lib.Console
{
    /// <inheritdoc />
    /// <summary>
    /// Defines some basic actions for console applications
    /// </summary>
    public abstract class ConsoleApplicationAbstract : IConsoleApplication
    {
        private readonly CommandLineApplication _application;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ConsoleApplicationAbstract(string name, string description)
        {
            _application = new CommandLineApplication
            {
                Name = name,
                Description = description
            };

            _application.HelpOption("-?|-h|--help");
        }

        /// <summary>
        /// Adds a command to this application
        /// </summary>
        protected void AddCommand(ICommand command)
        {
            _application.Command(command.CommandlineName, command.Execute);
        }

        /// <summary>
        /// Adds a command to this application
        /// </summary>
        protected void AddCommand(IAsyncCommand command)
            => AddCommand(new AsyncCommandSyncAdapter(command));

        /// <inheritdoc />
        public int Execute(string[] args) => _application.Invoke();

        /// <inheritdoc />
        public abstract IConsoleApplication Fork();

        /// <inheritdoc />
        public abstract string CommandlineName { get; }
    }
}
