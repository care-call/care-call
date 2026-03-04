using CC.AppointmentService.Application.UseCases.Appointments.Complete;
using Mediator;

namespace CC.AppointmentService.Api.Endpoints.Appointments;

public class CompleteAppointmentEndpoint
{
    public static async Task<IResult> Handle(Guid id, Guid practitionerId, IMediator mediator)
    {
        var result = new CompleteAppointment
        {
            AppointmentId = id,
            PractitionerId = practitionerId
        };
        var response = await mediator.Send(result);
        if (response.IsSuccess)
            return Results.Ok();
        
        return Results.BadRequest(response.Errors);
    }
}