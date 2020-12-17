using System;
using System.Threading;

namespace Lib.Console
{
    public class ConsoleCancellationToken
    {
        public static CancellationToken SigtermCancellationToken
        {
            get
            {
                var cancellationTokenSource = new CancellationTokenSource();
                AppDomain.CurrentDomain.ProcessExit +=
                    (sender, args) => cancellationTokenSource.Cancel();
                System.Console.CancelKeyPress  +=
                    (sender, args) => cancellationTokenSource.Cancel();
                return cancellationTokenSource.Token;
            }
        }
    }
}
