using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CC.HandbookService.Infrastructure.OpenApi;

public static class OpenApiConfigurator
{
    public static void Configure(OpenApiOptions options)
    {
        options.AddSchemaTransformer((schema, context, ct) =>
        {
            var isParsable = context.JsonTypeInfo.Type
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IParsable<>));

            if (isParsable)
                schema.Type = JsonSchemaType.String;
            
            if (context.JsonTypeInfo.Type == typeof(int) ||
                context.JsonTypeInfo.Type == typeof(int?))
            {
                schema.Type = JsonSchemaType.Integer;
            }

            return Task.CompletedTask;
        });
    }
}