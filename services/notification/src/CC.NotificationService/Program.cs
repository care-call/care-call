using CC.Common.Json;
using CC.NotificationService.Api.Endpoints.WebNotifications;
using CC.NotificationService.Application;
using CC.NotificationService.Infrastructure;
using CC.NotificationService.Infrastructure.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

builder.Services.AddValidation();
builder.Services.AddOpenApi(OpenApiConfigurator.Configure);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "v1"); });
}

app.MapWebNotificationsEndpoints();

app.Run();