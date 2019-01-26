using Lib.Patterns;

namespace Lib.Hash.Hashable
{
    /// <summary>
    /// Factory that changes an object into a Hash
    /// </summary>
    public interface IHashFactory : IFactory<object, IHash>
    {
    }
}
