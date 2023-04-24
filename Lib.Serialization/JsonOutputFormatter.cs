using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ConsoleApi.Serialization.Abstractions;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace ConsoleApi.Serialization;

/// <inheritdoc />
/// <summary>
/// Replaces .NET's JsonOutputFormatter for the sake of consistency.
/// </summary>
internal class JsonOutputFormatter : TextOutputFormatter
{
    private readonly IJsonSerializer _serializer;

    /// <inheritdoc />
    public JsonOutputFormatter(IJsonSerializer serializer)
    {
        _serializer = serializer;
            
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/json"));
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/*+json"));

        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }
        
    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        if (context == null)  throw new ArgumentNullException(nameof(context));
        if (selectedEncoding == null) throw new ArgumentNullException(nameof(selectedEncoding));

        var httpContext = context.HttpContext;
        var responseStream = httpContext.Response.Body;

        var writer = new StreamWriter(
            selectedEncoding.CodePage == Encoding.UTF8.CodePage
                ? responseStream
                : Encoding.CreateTranscodingStream(
                    httpContext.Response.Body, selectedEncoding, Encoding.UTF8, true));

        await writer.WriteAsync(_serializer.SerializeUnchecked(context.Object));
        await writer.FlushAsync();
    }
}