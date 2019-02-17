using JetBrains.Annotations;

namespace Lib.Patterns
{
    /// <summary>
    /// A Responsible Factory, with a CanFactory method
    /// </summary>
    [PublicAPI]
    public interface IResponsibleFactory<in TInput, out TOutput> : IFactory<TInput, TOutput>
    {
        /// <summary>
        /// Returns true if the factory can handle the input
        /// </summary>
        bool CanFactory(TInput input);
    }
}
