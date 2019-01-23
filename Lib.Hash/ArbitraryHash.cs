namespace Lib.Hash
{
    /// <summary>
    /// Arbitrary hash created from bytes
    /// </summary>
    public class ArbitraryHash : HashAbstract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ArbitraryHash(byte[] bytes) : base(bytes)
        {
        }
    }
}
