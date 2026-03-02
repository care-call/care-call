using CC.AppointmentService.Api.Contracts;
using CC.AppointmentService.Application.UseCases.Appointments.Cancellation;
using Mediator;

namespace CC.AppointmentService.Api.Endpoints.Appointments;

public static class CancelAppointmentEndpoint
{
    public static async Task<IResult> Handle(
        CancelAppointmentRequest request,
        IMediator mediator)
    {
        var result = await mediator.Send(new CancelAppointment()
        {
           AppointmentId =  request.AppointmentId,
           ClientId = request.ClientId,
           Reason =  request.Reason,
        });
        return Results.Ok(result);
    }
}