using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lib.Api
{
    /// <summary>
    /// Generic version of <see cref="JsonResult"/>
    /// </summary>
    public class JsonResult<TRepresented> : JsonResult
    {
        /// <inheritdoc />
        internal JsonResult(TRepresented value) : base(value)
        {
        }

        /// <inheritdoc />
        public JsonResult(TRepresented value, JsonSerializerSettings serializerSettings) : base(value, serializerSettings)
        {
        }

        /// <summary>
        /// The value to be serialized
        /// </summary>
        public new TRepresented Value
        {
            get => (TRepresented)base.Value;
            set => base.Value = value;
        }
    }

    /// <summary>
    /// Extends <see cref="Controller"/> to enable creation of generic JsonResult
    /// </summary>
    public static class ControllerJsonResultExtensions
    {
        /// <summary>
        /// Extension that wraps <see cref="Controller"/> Json method
        /// </summary>
        public static JsonResult<TRepresented> Json<TRepresented>(this Controller controller, TRepresented value)
        {
            var innerResult = controller.Json(value);

            var result = new JsonResult<TRepresented>((TRepresented) innerResult.Value)
            {
                SerializerSettings = innerResult.SerializerSettings,
                ContentType = innerResult.ContentType,
                StatusCode = innerResult.StatusCode
            };

            return result;
        }
    }
}
