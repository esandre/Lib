using System;
using System.Linq;

namespace Lib.Console
{
    /// <inheritdoc />
    /// <summary>
    /// Wraps another <see cref="T:Lib.Console.IConsoleApplication" /> and adds some Console outputs
    /// </summary>
    public class ShellWrapper : IConsoleApplication
    {
        private readonly IConsoleApplication _application;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShellWrapper(IConsoleApplication application)
        {
            _application = application;
        }

        /// <summary>
        /// Executes and prints returnCode and exceptions
        /// If DEBUG automatically spawns a fork after execution
        /// </summary>
        public int Execute(string[] args)
        {
            if (!args.Any())
            {
                System.Console.Write($"> {CommandlineName} ");
                var inputArgs = System.Console.ReadLine()?.Split(' ');

                try
                {
                    var returnCode = _application.Execute(inputArgs);
                    System.Console.WriteLine("Application returned code " + returnCode);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Application throwed " + e);
                }

#if DEBUG
                return Fork().Execute(new string[0]);
#else
                return 0;
#endif
            }
            else
            {
                return _application.Execute(args);
            }
        }

        /// <inheritdoc />
        public IConsoleApplication Fork() => new ShellWrapper(_application.Fork());

        /// <inheritdoc />
        public string CommandlineName => _application.CommandlineName;
    }
}
