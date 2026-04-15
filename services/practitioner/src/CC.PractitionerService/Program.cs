using CC.Common.Json;
using CC.Common.Logging;
using CC.PractitionerService.Infrastructure.Persistence;
using CC.PractitionerService.Api.Endpoints.Practitioners;
using CC.PractitionerService.Api.Endpoints.WorkSchedules;
using CC.PractitionerService.Application;
using CC.PractitionerService.Infrastructure;
using CC.PractitionerService.Infrastructure.OpenApi;
using CC.ServiceDefaults;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

builder.Services.AddOpenApi(OpenApiConfigurator.Configure);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Logging.ClearProviders();
builder.Services.AddLogger(builder.Configuration);

builder.Host.UseWolverine(opts =>
{
    opts.Discovery.CustomizeHandlerDiscovery(t => t.Includes.WithNameSuffix("UseCase"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

await app.Services.ApplyMigrationsAsync();

app.MapWorkSchedulesEndpoints();
app.MapPractitionersEndpoints();

app.Run();
