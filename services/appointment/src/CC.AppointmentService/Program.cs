using CC.AppointmentService.Api.Endpoints.Appointments;
using CC.AppointmentService.Application;
using CC.AppointmentService.Application.Dependencies.YandexTelemost;
using CC.AppointmentService.Infrastructure;
using CC.AppointmentService.Infrastructure.OpenApi;
using CC.AppointmentService.Infrastructure.YandexTelemost;
using CC.Common.Json;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.SerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

builder.Services.AddValidation();
builder.Services.AddOpenApi(OpenApiConfigurator.Configure);

builder.Services.AddHttpClient<IYandexTelemostService, YandexTelemostService>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapHangfireDashboard();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.MapAppointmentsEndpoints();

app.Run();