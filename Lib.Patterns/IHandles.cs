using System.Threading;
using System.Threading.Tasks;

namespace Lib.Patterns
{
    public interface IHandlesAsync<in TMessage, TReturn>
    {
        Task<TReturn> HandleAsync(TMessage message, CancellationToken token);
    }
}
