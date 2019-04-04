using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lib.Api
{
    /// <summary>
    /// Generic version of <see cref="JsonResult"/>
    /// </summary>
    public class JsonResult<TRepresented> : JsonResult
    {
        internal JsonResult(JsonResult nonGeneric) : base((TRepresented) nonGeneric.Value, nonGeneric.SerializerSettings)
        {
        }

        /// <inheritdoc />
        public JsonResult(TRepresented value) : base(value)
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
            => new JsonResult<TRepresented>(controller.Json(value));

        /// <summary>
        /// Extension that wraps <see cref="Controller"/> Json method with serializer settings
        /// </summary>
        public static JsonResult<TRepresented> Json<TRepresented>(this Controller controller, TRepresented value, JsonSerializerSettings serializerSettings)
            => new JsonResult<TRepresented>(controller.Json(value, serializerSettings));
    }
}
