using CC.AppointmentService.Application.UseCases.Appointments.CallCreation;
using Mediator;

namespace CC.AppointmentService.Infrastructure.BackgroundJobs.Jobs;

public class CallCreationJob(
    ILogger<CallCreationJob> logger,
    IMediator mediator)
{
    public async Task Execute(Guid appointmentId, CancellationToken ct)
    {
        var jobId = Guid.CreateVersion7();
        using var scropeLogg = logger.BeginScope(new {
            JobId = jobId,
            AppointmentId = appointmentId
        });
        try
        {
            logger.LogInformation("Запуск джобы создания звонка в яндекс телемосте {appointmentId}",
                appointmentId);
            var result = await mediator.Send(new CreateCall(appointmentId), ct);
            if(result.IsFailed)
                logger.LogError("Ошибка создания звонка {errors}", result.Errors);
        }
        
        catch (Exception ex)
        {
            logger.LogError(ex, "Критическая ошибка");
            
            throw;
        }
    }
}