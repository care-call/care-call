using CC.AppointmentService.Api.Endpoints.Appointments;
using CC.AppointmentService.Api.Endpoints.Reviews;
using CC.AppointmentService.Application;
using CC.AppointmentService.Infrastructure;
using CC.AppointmentService.Infrastructure.OpenApi;
using CC.Common.Json;
using CC.Common.Logging;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

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
app.MapReviewsEndpoints();

app.Run();