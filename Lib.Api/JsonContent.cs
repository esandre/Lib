using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Lib.Api
{
    /// <summary>
    /// Writes an object as Json in HttpContent
    /// </summary>
    public class JsonContent : StringContent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonContent(object o) 
            : base(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json") { }
    }
}
