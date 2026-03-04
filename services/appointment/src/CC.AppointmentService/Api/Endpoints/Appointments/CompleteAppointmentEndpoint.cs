using CC.AppointmentService.Api.Contracts;
using CC.AppointmentService.Application.UseCases.Appointments.Complete;
using CC.AppointmentService.Application.UseCases.Appointments.Transferring;
using CC.Shared.Domain.TimeRanges;
using Mediator;

namespace CC.AppointmentService.Api.Endpoints.Appointments;

public class CompleteAppointmentEndpoint
{
    public static async Task<IResult> Handle(Guid id, Guid practicantId, IMediator mediator)
    {
        var result = new CompleteAppointment
        {
            AppointmentId = id,
            Practicant = practicantId
        };
        var response = await mediator.Send(result);
        if (response.IsSuccess)
            return Results.Ok();
        
        return Results.BadRequest(response.Errors);
    }
}