using System.Text.Json.Nodes;
using CC.Common.Json;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CC.AppointmentService.Infrastructure.OpenApi;

public static class OpenApiConfigurator
{
    public static void Configure(OpenApiOptions options)
    {
        options.AddSchemaTransformer((schema, context, _) =>
        {
            var type = Nullable.GetUnderlyingType(context.JsonTypeInfo.Type) ?? context.JsonTypeInfo.Type;

            if (context.JsonTypeInfo.Type == typeof(DateOnly))
            {
                schema.Type = JsonSchemaType.String;
                schema.Format = "date";
                schema.Example = DateOnly.FromDateTime(DateTime.UtcNow).ToString(DateOnlyJsonConverter.Format);
            }
        
            if (context.JsonTypeInfo.Type  == typeof(TimeOnly))
            {
                schema.Type = JsonSchemaType.String;
                schema.Format = "time";
                schema.Example = "14:30:00";
            }

            if (type.IsEnum)
            {
                schema.Type = JsonSchemaType.String;
                schema.Enum = Enum.GetNames(type)
                    .Select(x => JsonValue.Create(x))
                    .Cast<JsonNode>()
                    .ToList();
            }
            if (context.JsonTypeInfo.Type  == typeof(TimeSpan))
            {
                schema.Type = JsonSchemaType.String;
                schema.Example = "00:30:00";
            }
            return Task.CompletedTask;
        });
    }
}