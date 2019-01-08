namespace Lib.Console
{
    /// <summary>
    /// Forkable console application
    /// </summary>
    public interface IConsoleApplication
    {
        /// <summary>
        /// Executes application normally
        /// </summary>
        int Execute(string[] args);

        /// <summary>
        /// Forks the application
        /// </summary>
        IConsoleApplication Fork();

        /// <summary>
        /// Name of the command in a shell
        /// </summary>
        string CommandlineName { get; }
    }
}
