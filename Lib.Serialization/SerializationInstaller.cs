using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ConsoleApi.Serialization.Abstractions;
using ConsoleApi.Serialization.Rules;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ConsoleApi.Serialization;

[PublicAPI]
public static class SerializationInstaller
{
    public static IJsonSerializer SelfContainedSerializer()
    {
        var microContainer = new ServiceCollection()
            .AddFrameworkJsonSerializer()
            .AddConsoleApiJsonSerializationOptions()
            .BuildServiceProvider();

        return microContainer.GetRequiredService<IJsonSerializer>();
    }

    public static IServiceCollection AddFrameworkJsonSerializer(this IServiceCollection collection)
    {
        collection.AddSingleton<IJsonSerializer, FrameworkJsonSerializer>();
        collection.AddSingleton<IConfigureOptions<MvcOptions>, ConfigureFormatting>();
        return collection;
    }

    public static IServiceCollection AddConsoleApiJsonSerializationOptions(this IServiceCollection collection)
    {
        collection.AddSingleton(context =>
        {
            var options = new JsonSerializerOptions
            {
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = new LowerCamelCaseNamingPolicy()
            };

            foreach (var jsonConverter in context.GetServices<JsonConverter>())
            {
                options.Converters.Add(jsonConverter);
            }

            return options;
        });

        return collection;
    }

    private class ConfigureFormatting : IConfigureOptions<MvcOptions>
    {
        private readonly IJsonSerializer _serializer;

        public ConfigureFormatting(IJsonSerializer serializer)
        {
            _serializer = serializer;
        }

        /// <inheritdoc />
        public void Configure(MvcOptions options)
        {
            var originalFormatters = options.OutputFormatters.ToArray();

            // Ce single est parfaitement intentionnel, s'il devait casser, cela signifie que ASP.NET a changé 
            // de formatter par défaut.
            var formatterToRemove = originalFormatters.Single(
                formatter => formatter.GetType().FullName ==
                             "Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter"
            );

            options.OutputFormatters.Remove(formatterToRemove);
            options.OutputFormatters.Add(new JsonOutputFormatter(_serializer));
        }
    }

}