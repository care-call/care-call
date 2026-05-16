using CC.Shared.Domain;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Kafka;
using Wolverine.Postgresql;
using СС.Contracts.Appointments.Events;

namespace CC.AppointmentService.Infrastructure.Messaging;

public static class DependencyInjection
{
    extension(IHostBuilder host)
    {
        public IHostBuilder AddMessaging(IConfiguration configuration)
        {
            return host.UseWolverine(opts =>
            {
                opts.Discovery.CustomizeHandlerDiscovery(t => t.Includes.WithNameSuffix("UseCase"));
                opts.UseKafka(configuration.GetConnectionString("Kafka")!);
                opts.PublishMessage<AppointmentCreatedEvent>().ToKafkaTopic("appointments");
                opts.PublishMessage<AppointmentTransferredEvent>().ToKafkaTopic("appointments");
                opts.PublishMessage<AppointmentCancelledEvent>().ToKafkaTopic("appointments");
                opts.PersistMessagesWithPostgresql(configuration.GetConnectionString("DefaultConnection")!);
                opts.UseEntityFrameworkCoreTransactions();
                opts.PublishDomainEventsFromEntityFrameworkCore<IDomainEventSource>(x => x.DomainEvents);
            });
        }
    }
}
