using CC.PractitionerService.Application.Dependencies.UnitOfWork;
using CC.PractitionerService.Infrastructure.Persistence.Availability.Abstractions;
using CC.Shared.Domain.TimeRanges;
using СС.Contracts.Appointments.Events;

namespace CC.PractitionerService.Infrastructure.MessageConsumers.Appointments;

public class AppointmentTransferredMessageConsumer
{
    public async Task ConsumeAsync(
        AppointmentTransferredEvent message,
        IEmploymentRecordStorage storage,
        IUnitOfWork unitOfWork,
        ILogger<AppointmentTransferredMessageConsumer> logger,
        DateTime now,
        CancellationToken token)
    {
        var key = message.AppointmentId.ToString();
        var exists = await storage.GetByKeyAsync(key);

        if (exists is null)
        {
            logger.LogError("Занятость с ключем {key} не существует", key);
            return;
        }

        exists.Period = new DateTimeRange(message.TimeSlot.From, message.TimeSlot.To);
        exists.UpdatedAt = now;

        await unitOfWork.SaveAsync(token);
    }
}