using CC.AppointmentService.Api.Endpoints.Appointments;
using CC.AppointmentService.Api.Endpoints.Appointments.Practitioner;
using CC.AppointmentService.Api.Endpoints.Feedback;
using CC.AppointmentService.Application;
using CC.AppointmentService.Infrastructure;
using CC.AppointmentService.Infrastructure.OpenApi;
using CC.AppointmentService.Infrastructure.Persistence;
using CC.Common.Json;
using CC.Common.Logging;
using CC.ServiceDefaults;
using Hangfire;
using CC.Shared.Domain;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Kafka;
using Wolverine.Postgresql;
using СС.Contracts.Appointments.Events;

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
    opts.UseKafka(builder.Configuration.GetConnectionString("Kafka")!);
    opts.PublishMessage<AppointmentCreatedEvent>().ToKafkaTopic("appointments");
    opts.PublishMessage<AppointmentTransferredEvent>().ToKafkaTopic("appointments");
    opts.PublishMessage<AppointmentCancelledEvent>().ToKafkaTopic("appointments");
    opts.PersistMessagesWithPostgresql(builder.Configuration.GetConnectionString("DefaultConnection")!);
    opts.UseEntityFrameworkCoreTransactions();
    opts.PublishDomainEventsFromEntityFrameworkCore<IDomainEventSource>(x => x.DomainEvents);
});

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

await app.Services.ApplyMigrationsAsync();

app.MapPractitionerAppointmentEndpoints();
app.MapAppointmentsEndpoints();
app.MapFeedbackEndpoints();

app.Run();
