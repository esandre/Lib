namespace Lib.OAuth2
{
    /// <summary>
    /// Informations de compte OAuth2
    /// </summary>
    public interface IAccount
    {
        /// <summary>
        /// ID
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Secret
        /// </summary>
        string ClientSecret { get; }
    }
}
