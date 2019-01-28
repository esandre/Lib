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
        void Hash(IHashableVisitor visitor);
    }
}
