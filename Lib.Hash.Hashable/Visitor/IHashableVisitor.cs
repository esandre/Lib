namespace Lib.Hash.Hashable.Visitor
{
    /// <summary>
    /// Hashable visitor, passed to Hashable objects
    /// </summary>
    public interface IHashableVisitor
    {
        /// <summary>
        /// Hash raw data
        /// </summary>
        void HashData(object data);

        /// <summary>
        /// Hash another Hashable
        /// </summary>
        void HashOther(IHashable other);
    }
}
