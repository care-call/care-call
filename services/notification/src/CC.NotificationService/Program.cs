using CC.Common.Json;
using CC.Common.Logging;
using CC.NotificationService.Api.Endpoints.WebNotifications;
using CC.NotificationService.Application;
using CC.NotificationService.Infrastructure;
using CC.NotificationService.Infrastructure.OpenApi;
using CC.NotificationService.Infrastructure.Persistence;
using CC.ServiceDefaults;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

builder.Services.AddValidation();
builder.Services.AddOpenApi(OpenApiConfigurator.Configure);
builder.Logging.ClearProviders();
builder.Services.AddLogger(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Host.UseWolverine(opts =>
{
    opts.Discovery.CustomizeHandlerDiscovery(t => t.Includes.WithNameSuffix("UseCase"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });
}

await app.Services.ApplyMigrationsAsync();

app.MapWebNotificationsEndpoints();

app.Run();
