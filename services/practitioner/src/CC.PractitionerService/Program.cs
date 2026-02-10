using CC.Common.Json;
using CC.PractitionerService.Api.Endpoints.WorkSchedules;
using CC.PractitionerService.Application;
using CC.PractitionerService.Infrastructure;
using CC.PractitionerService.Infrastructure.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

builder.Services.AddOpenApi(OpenApiConfigurator.Configure);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.MapWorkSchedulesEndpoints();

app.Run();