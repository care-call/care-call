using CC.PractitionerService.Application.Dependencies.UnitOfWork;
using CC.PractitionerService.Infrastructure.Persistence.Availability.Abstractions;
using CC.PractitionerService.Infrastructure.Persistence.Availability.Models;
using CC.Shared.Domain.TimeRanges;
using СС.Contracts.Messages.Appointment;

namespace CC.PractitionerService.Infrastructure.MessageConsumers.Appointment;

public static class AppointmentCreatedMessageHandler
{
    public static async Task Handle(
        AppointmentCreatedMessage message,
        IEmploymentRecordStorage storage,
        IUnitOfWork unitOfWork,
        ILogger logger,
        DateTime now,
        CancellationToken token)
    {
        var key = message.AppointmentId.ToString();
        var exist = await storage.GetByKeyAsync(key);

        if (exist is not null)
        {
            logger.LogError("Занятость с ключем {ExternalEmploymentKey} уже существует", exist.ExternalEmploymentKey);
            return;
        }

        var employment = new EmploymentRecord()
        {
            Id = Guid.CreateVersion7(),
            PractitionerId = message.PractitionerId,
            ExternalEmploymentKey = key,
            Period = new DateTimeRange(message.TimeSlot.From, message.TimeSlot.To),
            CreatedAt = now,
            UpdatedAt = now
        };

        await storage.CreateAsync(employment);
        await unitOfWork.SaveAsync(token);
    }
}
