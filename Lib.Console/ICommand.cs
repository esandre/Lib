using Microsoft.Extensions.CommandLineUtils;

namespace Lib.Console
{
    /// <summary>
    /// A program command
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Name of the command
        /// </summary>
        string CommandlineName { get; }

        /// <summary>
        /// Execution configuration
        /// </summary>
        void Execute(CommandLineApplication command);
    }
}
