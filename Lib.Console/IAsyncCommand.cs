using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Lib.Console
{
    /// <summary>
    /// An asynchronous command
    /// </summary>
    public interface IAsyncCommand
    {
        /// <summary>
        /// Name of the command
        /// </summary>
        string CommandlineName { get; }

        /// <summary>
        /// Execution configuration
        /// </summary>
        Task ExecuteAsync(CommandLineApplication command);
    }
}
