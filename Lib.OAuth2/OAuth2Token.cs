﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Lib.OAuth2.Contracts;

namespace Lib.OAuth2
{
    /// <inheritdoc />
    /// <summary>
    /// Token renouvelé automatiquement avant expiration
    /// </summary>
    public class OAuth2Token : IToken
    {
        /// <summary>
        /// Multiplicateur permettant de renouveler le jeton OAuth avant son terme,
        /// afin d'éviter les imprécisions dues à la latence ou aux désynchronisations d'horloge
        /// </summary>
        private const float PreventiveRenewalFactor = 0.9f;
        private static readonly DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(PostTokenResponseContract));
        
        private readonly Uri _provider;
        private readonly FormUrlEncodedContent _requestContent;
        private readonly HttpClient _client = new HttpClient();
        
        private DateTime _expirationDate;
        private string _latestToken;

        /// <summary>
        /// Créé un Token OAuth à partir d'un provider et d'un compte
        /// </summary>
        public OAuth2Token(Uri provider, IAccount account)
        {
            _provider = provider;
            _client.DefaultRequestHeaders.Clear();

            _requestContent = new FormUrlEncodedContent(new Dictionary<string, string> 
            {
                {"client_id", account.ClientId}, 
                {"client_secret", account.ClientSecret},
                {"grant_type", "client_credentials"}
            });

            _expirationDate = DateTime.MinValue;
        }

        private async Task RefreshToken()
        {
            var response = await _client.PostAsync(_provider, _requestContent);
            var responseContract = (PostTokenResponseContract) Serializer.ReadObject(await response.Content.ReadAsStreamAsync());

            _latestToken = responseContract.AccessToken;
            _expirationDate = DateTime.Now.AddSeconds(responseContract.ExpiresIn * PreventiveRenewalFactor);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Renvoie la valeur du token, en la rafraîchissant au besoin auprès du provider
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetValueAsync()
        {
            if (DateTime.Now >= _expirationDate) await RefreshToken();
            return _latestToken;
        }
    }
}
