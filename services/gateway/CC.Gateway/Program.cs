using CC.Gateway.OpenApi;
using CC.Gateway.Swagger;
using CC.Gateway.Yarp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddServiceDiscovery();
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

builder.Services.AddSingleton<OpenApiProvider>();
builder.Services.AddSingleton<YarpClusterInfoProvider>();

var app = builder.Build();

app.MapReverseProxy();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApiEndpoints();
    app.AddSwaggerEndpoints();
}

app.Run();