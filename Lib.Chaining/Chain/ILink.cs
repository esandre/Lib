namespace Lib.Chaining.Chain
{
    /// <summary>
    /// An independant element which can be linked to others, or not.
    /// </summary>
    public interface ILink<out TPayload>
    {
        /// <summary>
        /// The payload of the element
        /// </summary>
        TPayload Payload { get; }
    }
}
