using CC.AppointmentService.Api.Contracts.Appointments;
using CC.AppointmentService.Application.UseCases.Appointments.Cancellation;
using FluentResults;
using Wolverine;

namespace CC.AppointmentService.Api.Endpoints.Appointments;

public static class CancelAppointmentEndpoint
{
    public static async Task<IResult> Handle(
        CancelAppointmentRequest request,
        IMessageBus bus)
    {
        var result = await bus.InvokeAsync<Result>(new CancelAppointment
        {
            AppointmentId = request.AppointmentId,
            ClientId = request.ClientId,
            Reason = request.Reason,
        });
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
    }
}