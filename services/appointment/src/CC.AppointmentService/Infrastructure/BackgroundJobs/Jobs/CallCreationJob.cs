using CC.AppointmentService.Application.UseCases.Appointments.CallCreation;
using Mediator;

namespace CC.AppointmentService.Infrastructure.BackgroundJobs.Jobs;

public class CallCreationJob(
    ILogger<CallCreationJob> logger,
    IMediator mediator)
{
    public async Task Execute(Guid appointmentId)
    {
        try
        {
            logger.LogInformation("Джоба стартанула {appointmentId}", appointmentId);
            await mediator.Send(new CreateCall(appointmentId));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Ошибка выполнения джобы");
        }
    }
}