using System.Text.Json.Nodes;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CC.PractitionerService.Infrastructure.OpenApi;

public static class OpenApiSchemaExampleExtensions
{
    extension(IOpenApiSchema schema)
    {
        public void ApplyExamplesFromAttributes(OpenApiSchemaTransformerContext context)
        {
            foreach (var jsonProp in context.JsonTypeInfo.Properties)
            {
                if (schema.Properties is null)
                    continue;

                if (!schema.Properties.TryGetValue(jsonProp.Name, out var propSchema))
                    continue;

                if (propSchema is not OpenApiSchema concretePropSchema)
                    continue;

                var exampleAttr = jsonProp.AttributeProvider
                    ?.GetCustomAttributes(typeof(OpenApiExampleAttribute), inherit: true)
                    .Cast<OpenApiExampleAttribute>()
                    .FirstOrDefault();

                if (exampleAttr is null)
                    continue;

                concretePropSchema.Example = JsonValue.Create(exampleAttr.Value);
            }
        }
    }
}