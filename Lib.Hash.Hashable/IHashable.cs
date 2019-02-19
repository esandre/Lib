using Lib.Hash.Hashable.Visitor;

namespace Lib.Hash.Hashable
{
    /// <summary>
    /// An object hashable by a visitor
    /// </summary>
    public interface IHashable
    {
        /// <summary>
        /// Describes how to hash self
        /// </summary>
        HashableVisitDelegate Hash { get; }
    }

    /// <summary>
    /// Hashable visitor pattern, should return the way to hash instance
    /// </summary>
    public delegate void HashableVisitDelegate(IHashableVisitor visitor);
}
