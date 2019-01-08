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
            this._application = new CommandLineApplication
            {
                Name = name,
                Description = description
            };

            this._application.HelpOption("-?|-h|--help");
        }

        /// <summary>
        /// Adds a command to this application
        /// </summary>
        protected void AddCommand(ICommand command)
        {
            this._application.Command(command.CommandlineName, command.Execute);
        }

        /// <inheritdoc />
        public int Execute(string[] args) => this._application.Invoke();

        /// <inheritdoc />
        public abstract IConsoleApplication Fork();

        /// <inheritdoc />
        public abstract string CommandlineName { get; }
    }
}
