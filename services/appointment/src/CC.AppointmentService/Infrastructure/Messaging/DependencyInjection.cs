using CC.Shared.Domain;
using CC.AppointmentService.Application.UseCases.Appointments.CallCreation;
using CC.AppointmentService.Application.UseCases.Appointments.Cancellation;
using CC.AppointmentService.Application.UseCases.Appointments.Complete;
using CC.AppointmentService.Application.UseCases.Appointments.Creation;
using CC.AppointmentService.Application.UseCases.Appointments.GetClientHistory;
using CC.AppointmentService.Application.UseCases.Appointments.Getting;
using CC.AppointmentService.Application.UseCases.Appointments.Transferring;
using CC.AppointmentService.Application.UseCases.Feedbacks.Creation;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.ErrorHandling;
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
                opts.Discovery.IncludeType(typeof(CreateAppointmentUseCase));
                opts.Discovery.IncludeType(typeof(TransferAppointmentUseCase));
                opts.Discovery.IncludeType(typeof(CancelAppointmentUseCase));
                opts.Discovery.IncludeType(typeof(CompleteAppointmentUseCase));
                opts.Discovery.IncludeType(typeof(CreateCallUseCase));
                opts.Discovery.IncludeType(typeof(GetPractitionerAppointmentsUseCase));
                opts.Discovery.IncludeType(typeof(GetClientHistoryUseCase));
                opts.Discovery.IncludeType(typeof(CreateFeedbackUseCase));
                opts.Discovery.IncludeType(typeof(AppointmentIntegrationEventHandler));
                opts.UseKafka(configuration.GetConnectionString("Kafka")!);
                opts.PublishMessage<AppointmentCreatedEvent>().ToKafkaTopic("appointments");
                opts.PublishMessage<AppointmentTransferredEvent>().ToKafkaTopic("appointments");
                opts.PublishMessage<AppointmentCancelledEvent>().ToKafkaTopic("appointments");
                opts.PersistMessagesWithPostgresql(configuration.GetConnectionString("DefaultConnection")!);
                opts.Policies.UseDurableOutboxOnAllSendingEndpoints();
                opts.OnException<Exception>()
                    .RetryWithCooldown(
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15));
                opts.UseEntityFrameworkCoreTransactions();
                opts.PublishDomainEventsFromEntityFrameworkCore<AggregationRoot<Guid>>(x => x.DomainEvents);
            });
        }
    }
}
