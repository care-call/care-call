using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Application.Dependencies.YandexTelemost;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using FluentResults;

namespace CC.AppointmentService.Application.UseCases.Appointments.CallCreation;

public sealed record CreateCall(Guid AppointmentId);

public static class CreateCallUseCase
{
    public static async ValueTask<Result> Handle(
        CreateCall request,
        IUnitOfWork unitOfWork,
        IYandexTelemostService yandexTelemostService,
        IAppointmentsRepository appointmentRepository,
        CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetByIdAsync(request.AppointmentId);
        if (appointment is null)
            return Result.Fail(AppointmentErrors.NotFound());
 
        var response = await yandexTelemostService.CreateCallLinkAsync(cancellationToken);
        if (response is null)
            return Result.Fail("Ошибка создания звонка в яндекс телемосте");
        
        appointment.CallUrl = new Uri(response.Url);
        await unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}