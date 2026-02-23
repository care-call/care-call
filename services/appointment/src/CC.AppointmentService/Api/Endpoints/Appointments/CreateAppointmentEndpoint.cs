using CC.AppointmentService.Api.Contracts;
using CC.AppointmentService.Application.UseCases.Appointments.Creation;
using CC.Shared.Domain.TimeRanges;
using Mediator;

namespace CC.AppointmentService.Api.Endpoints.Appointments;

public static class CreateAppointmentEndpoint
{
    public static async Task<IResult> Handle(
        CreateAppointmentRequest request,
        IMediator mediator)
    {
        var result = await mediator.Send(new CreateAppointment
        {
            PractitionerId = request.PractitionerId,
            ClientId = request.ClientId,
            TimeSlot = new DateTimeRange(request.StartedAt, request.EndedAt),
            ClientSnapshot = request.ClientSnapshot,
            PractitionerSnapshot = request.PractitionerSnapshot
        });
        return Results.Ok(result);
    }
}