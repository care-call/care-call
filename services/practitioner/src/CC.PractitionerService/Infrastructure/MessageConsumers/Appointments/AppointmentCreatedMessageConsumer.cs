using CC.PractitionerService.Application.Dependencies.UnitOfWork;
using CC.PractitionerService.Infrastructure.Persistence.Availability.Abstractions;
using CC.PractitionerService.Infrastructure.Persistence.Availability.Models;
using CC.Shared.Domain.TimeRanges;
using СС.Contracts.Appointments.Events;

namespace CC.PractitionerService.Infrastructure.MessageConsumers.Appointments;

public class AppointmentCreatedMessageConsumer
{
    public async Task ConsumeAsync(
        AppointmentCreatedEvent message,
        IEmploymentRecordStorage storage,
        IUnitOfWork unitOfWork,
        ILogger<AppointmentCreatedMessageConsumer> logger,
        DateTime now,
        CancellationToken token)
    {
        var key = message.AppointmentId.ToString();
        var exists = await storage.GetByKeyAsync(key);

        if (exists is not null)
        {
            logger.LogError("Занятость с ключем {ExternalEmploymentKey} уже существует", exists.ExternalEmploymentKey);
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
