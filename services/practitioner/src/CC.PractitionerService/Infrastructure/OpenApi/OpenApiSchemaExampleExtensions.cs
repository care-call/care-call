using System.Text.Json.Nodes;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CC.PractitionerService.Infrastructure.OpenApi;

public static class OpenApiSchemaExampleExtensions
{
    public static void ApplyExamplesFromAttributes(this IOpenApiSchema schema, OpenApiSchemaTransformerContext context)
    {
        foreach (var jsonProp  in context.JsonTypeInfo.Properties)
        {
            if (!schema.Properties.TryGetValue(jsonProp.Name, out var propSchema))
                continue;

            var provider = jsonProp.AttributeProvider;
            if (provider is null)
                continue;

            var exampleAttr = provider
                .GetCustomAttributes(typeof(OpenApiExample), inherit: true)
                .Cast<OpenApiExample>()
                .FirstOrDefault();

            if (exampleAttr is null)
                continue;

            if (propSchema is OpenApiSchema concreteProp) 
                concreteProp.Example = JsonValue.Create(exampleAttr.Value);
        }
    }
}