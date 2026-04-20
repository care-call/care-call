using CC.Gateway.OpenApi;

namespace CC.Gateway.Swagger;

public static class SwaggerEndpoints
{
    extension(IApplicationBuilder builder)
    {
        public void AddSwaggerEndpoints()
        {
            var provider = builder.ApplicationServices.GetRequiredService<OpenApiProvider>();
        
            builder.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/Gateway-v1.json", "Gateway-v1");
                foreach (var doc in provider.GetDocuments())
                {
                    var url = $"/openapi/{doc.DocumentName}.json";
                    options.SwaggerEndpoint(url, $"{doc.DocumentName}");
                }
            });
        }
    }
}