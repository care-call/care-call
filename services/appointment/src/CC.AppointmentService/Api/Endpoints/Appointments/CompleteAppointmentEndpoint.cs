using CC.AppointmentService.Application.UseCases.Appointments.Complete;
using FluentResults;
using Wolverine;

namespace CC.AppointmentService.Api.Endpoints.Appointments;

public static class CompleteAppointmentEndpoint
{
    public static async Task<IResult> Handle(Guid id, Guid practitionerId, IMessageBus bus)
    {
        var result = await bus.InvokeAsync<Result>(new CompleteAppointment
        {
            AppointmentId = id,
            PractitionerId = practitionerId
        });
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
    }
}