namespace Lib.Chaining.Chain
{
    /// <summary>
    /// Double chained link with both a Successor and a Predecessor
    /// </summary>
    public interface IDoubleChainedLink<out TPayload> : ISuccessor<TPayload>, IPredecessor<TPayload>
    {
    }
}
