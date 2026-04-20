using System.Text.Json.Nodes;

namespace CC.Gateway.OpenApi;

public static class OpenApiEndpoints
{
    extension(WebApplication app)
    {
        public void MapOpenApiEndpoints()
        {
            app.MapOpenApi("/openapi/gateway-v1.json");
            
            var provider = app.Services.GetRequiredService<OpenApiProvider>();
            
            app.MapGet("/openapi/{documentName}-{version}.json", 
                async (string documentName, string version, HttpContext httpContext) =>
            {
                var document = provider.GetDocument(documentName);
                if (document is null)
                    return Results.NotFound($"OpenAPI document '{documentName}-{version}' not found");

                var json = await provider.GetDocumentAsJsonAsync(document.ClusterId);
          
                return Results.Content(ReplaceDocumentServer(json, httpContext), "application/json");
            });
        }
    }
    
    private static string ReplaceDocumentServer(string documentJson, HttpContext httpContext)
    {
        var gatewayBaseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        
        var doc = JsonNode.Parse(documentJson) ?? throw new Exception("Invalid document json");
        doc["servers"] = new JsonArray(new JsonObject { ["url"] = gatewayBaseUrl });
        
        return doc.ToJsonString();
    }
}