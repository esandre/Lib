using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Lib.OAuth2
{
    /// <summary>
    /// Donne accès à un token OAuth2 réactualisable
    /// </summary>
    [PublicAPI]
    public interface IToken
    {
        /// <summary>
        /// Récupère la valeur du Token
        /// </summary>
        Task<string> GetValueAsync();
    }
}
