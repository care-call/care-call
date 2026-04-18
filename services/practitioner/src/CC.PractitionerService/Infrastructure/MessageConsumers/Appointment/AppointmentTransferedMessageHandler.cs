using CC.PractitionerService.Application.Dependencies.UnitOfWork;
using CC.PractitionerService.Infrastructure.Persistence.Availability.Abstractions;
using CC.Shared.Domain.TimeRanges;
using СС.Contracts.Messages.Appointment;

namespace CC.PractitionerService.Infrastructure.MessageConsumers.Appointment;

public static class AppointmentTransferedMessageHandler
{
    public static async Task Handle(
        AppointmentTransferedMessage message,
        IEmploymentRecordStorage storage,
        IUnitOfWork unitOfWork,
        ILogger logger,
        DateTime now,
        CancellationToken token)
    {
        var key = message.AppointmentId.ToString();
        var exist = await storage.GetByKeyAsync(key);

        if (exist is null)
        {
            logger.LogError("Занятость с ключем {key} не существует", key);
            return;
        }

        exist.Period = new DateTimeRange(message.TimeSlot.From, message.TimeSlot.To);
        exist.UpdatedAt = now;

        await unitOfWork.SaveAsync(token);
    }
}