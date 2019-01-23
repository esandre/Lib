using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lib.OAuth2;
#pragma warning disable 1591

namespace Lib.Api
{
    [Obsolete]
    public abstract class EndpointClient
    {
        private readonly Uri _endPointUri;
        private readonly IToken _credentials;

        protected EndpointClient(Uri endPointUri, IToken credentials)
        {
            _endPointUri = endPointUri;
            _credentials = credentials;
        }

        public async Task<HttpResponseMessage> GetAsync(string relativeUri, object payload)
        {
            return await GetAsync(relativeUri, DataContractToDictionary(payload));
        }

        private async Task<HttpResponseMessage> GetAsync(string relativeUri, IEnumerable<KeyValuePair<string, string>> query = null)
        {
            var client = await BuildHttpClient();

            var uri = _endPointUri.Append(relativeUri);
            var uriWithQuery = new Uri(uri.ToString().TrimEnd('/') + InlineQuery(query));

            var response = await client.GetAsync(uriWithQuery);

            ThrowIfStatusCodeNotOk(uri, response);

            return response;
        }

        private static  string InlineQuery(IEnumerable<KeyValuePair<string, string>> query)
        {
            var keyValuePairs = query as KeyValuePair<string, string>[] ?? query?.ToArray() ?? new KeyValuePair<string, string>[0];

            if (!keyValuePairs.Any()) return string.Empty;

            var stringBuilder = new StringBuilder("?");
            foreach (var part in keyValuePairs)
            {
                stringBuilder.Append(HttpUtility.HtmlEncode(part.Key));
                stringBuilder.Append("=");
                stringBuilder.Append(HttpUtility.HtmlEncode(part.Value));
                stringBuilder.Append("&");
            }

            return stringBuilder.ToString().TrimEnd('&');
        }

        protected async Task DeleteAsync(string relativeUri)
        {
            var client = await BuildHttpClient();

            var uri = _endPointUri.Append(relativeUri);
            var response = await client.DeleteAsync(uri);

            ThrowIfStatusCodeNotOk(uri, response);
        }

        protected async Task<TResponseContract> DeleteManyAsync<TRequestContract, TResponseContract>(string relativeUri, IEnumerable<TRequestContract> payload)
        {
            await Task.Delay(1);
            throw new NotImplementedException();
        }
        
        protected async Task<IEnumerable<TContract>> GetManyAsync<TContract>(string relativeUri)
        {
            var response = await GetAsync(relativeUri);
            var deserializer = new DataContractJsonSerializer(typeof(TContract));
            
            return (TContract[])deserializer.ReadObject(await response.Content.ReadAsStreamAsync());
        }

        protected async Task<TContract> GetAsync<TContract>(string relativeUri)
        {
            var response = await GetAsync(relativeUri);
            var deserializer = new DataContractJsonSerializer(typeof(TContract));
            var responseStream = await response.Content.ReadAsStreamAsync();

            try
            {
                return (TContract) deserializer.ReadObject(responseStream);
            }
            catch (Exception e)
            {
                throw new DeserializationException(typeof(TContract), responseStream, e);
            }
        }

        protected async Task<TContract> GetAsync<TContract>(string relativeUri, object payload)
        {
            var response = await GetAsync(relativeUri, payload);
            var deserializer = new DataContractJsonSerializer(typeof(TContract));

            return (TContract)deserializer.ReadObject(await response.Content.ReadAsStreamAsync());
        }

        protected async Task<TResponseContract> PostAsync<TRequestContract, TResponseContract>(string relativeUri, IEnumerable<TRequestContract> payload)
        {
            var client = await BuildHttpClient();
            var uri = _endPointUri.Append(relativeUri);

            var serializer = new DataContractJsonSerializer(payload.GetType());

            HttpResponseMessage response;
            using (var contentStream = new MemoryStream())
            {
                serializer.WriteObject(contentStream, payload);
                contentStream.Position = 0;
                response = await client.PostAsync(uri, JsonContentFromStream(contentStream));
            }

            ThrowIfStatusCodeNotOk(uri, response);

            var deserializer = new DataContractJsonSerializer(typeof(TResponseContract));

            return (TResponseContract)deserializer.ReadObject(await response.Content.ReadAsStreamAsync());
        }

        protected async Task<TContract> PostAsync<TContract>(string relativeUri, object payload)
        {
            var client = await BuildHttpClient();
            var uri = _endPointUri.Append(relativeUri);

            var serializer = new DataContractJsonSerializer(payload.GetType());

            HttpResponseMessage response;
            using (var contentStream = new MemoryStream())
            {
                serializer.WriteObject(contentStream, payload);
                contentStream.Position = 0;
                response = await client.PostAsync(uri, JsonContentFromStream(contentStream));
            }

            ThrowIfStatusCodeNotOk(uri, response);

            var deserializer = new DataContractJsonSerializer(typeof(TContract));

            return (TContract)deserializer.ReadObject(await response.Content.ReadAsStreamAsync());
        }

        protected async Task<HttpResponseMessage> PutAsync(string relativeUri, object payload)
        {
            var client = await BuildHttpClient();
            var uri = _endPointUri.Append(relativeUri);

            var serializer = new DataContractJsonSerializer(payload.GetType());

            HttpResponseMessage response;
            using (var contentStream = new MemoryStream())
            {
                serializer.WriteObject(contentStream, payload);
                contentStream.Position = 0;
                response = await client.PutAsync(uri, JsonContentFromStream(contentStream));
            }

            ThrowIfStatusCodeNotOk(uri, response);

            return response;
        }

        private static HttpContent JsonContentFromStream(Stream stream) =>
            new StreamContent(stream) {Headers = {ContentType = new MediaTypeHeaderValue("application/json")}};

        private static void ThrowIfStatusCodeNotOk(Uri uri, HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException(uri.AbsoluteUri + " unauthorized");
                case HttpStatusCode.Forbidden:
                    throw new UnauthorizedAccessException(uri.AbsoluteUri + " forbidden");
                case HttpStatusCode.NotFound:
                    throw new FileNotFoundException(uri.AbsoluteUri + " not found");
                case HttpStatusCode.BadRequest:
                    throw new FormatException(uri.AbsoluteUri + " bad request");
            }

            if ((int)response.StatusCode < 200 || (int)response.StatusCode > 299)
                throw new ApplicationException(uri.AbsoluteUri + " code " + response.StatusCode);
        }

        private static IEnumerable<KeyValuePair<string, string>> DataContractToDictionary(object contract)
        {
            var contractType = contract.GetType();
            if(contractType.GetCustomAttribute(typeof(DataContractAttribute)) == null)
                throw new Exception(contract.GetType() + " n'est pas un [DataContract]");
            
            foreach (var property in contractType.GetProperties())
            {
                var dataMemberAttribute = property.GetCustomAttribute<DataMemberAttribute>();
                if(dataMemberAttribute == null) continue;
                
                var name = dataMemberAttribute.IsNameSetExplicitly
                    ? dataMemberAttribute.Name
                    : property.Name;

                yield return new KeyValuePair<string, string>(name, property.GetValue(contract).ToString());
            }
        }

        private async Task<HttpClient> BuildHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", await _credentials.GetValueAsync());

            return client;
        }
    }
}
