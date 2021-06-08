using System.Threading;
using System.Threading.Tasks;

namespace Lib.Patterns
{
    /// <summary>
    /// A Factory with one input
    /// </summary>
    public interface IFactory<in TInput, out TOutput>
    {
        /// <summary>
        /// Factories Output from Input
        /// </summary>
        TOutput Factory(TInput input);
    }

    /// <summary>
    /// An async Factory with one input
    /// </summary>
    public interface IAsyncFactory<in TInput, TOutput>
    {
        /// <summary>
        /// Factories Output from Input
        /// </summary>
        Task<TOutput> FactoryAsync(TInput input, CancellationToken token);
    }

    /// <summary>
    /// A Factory with two inputs
    /// </summary>
    public interface IFactory<in TInput1, in TInput2, out TOutput>
    {
        /// <summary>
        /// Factories Output from Inputs
        /// </summary>
        TOutput Factory(TInput1 input1, TInput2 input2);
    }

    /// <summary>
    /// A Factory with three inputs
    /// </summary>
    public interface IFactory<in TInput1, in TInput2, in TInput3, out TOutput>
    {
        /// <summary>
        /// Factories Output from Inputs
        /// </summary>
        TOutput Factory(TInput1 input1, TInput2 input2, TInput3 input3);
    }
}
