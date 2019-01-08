using System.Runtime.Serialization;

namespace Lib.OAuth2.Contracts
{
    [DataContract]
    internal class PostTokenResponseContract
    {
        [DataMember(Name = "access_token")] public string AccessToken { get; set; }
        [DataMember(Name = "expires_in")] public int ExpiresIn { get; set; }
        [DataMember(Name = "scope")] public string Scope { get; set; }
        [DataMember(Name = "token_type")] public string TokenType { get; set; }
    }
}
