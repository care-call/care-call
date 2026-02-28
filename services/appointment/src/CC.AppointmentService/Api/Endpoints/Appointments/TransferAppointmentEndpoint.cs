using CC.AppointmentService.Api.Contracts;
using CC.AppointmentService.Application.UseCases.Appointments.Rescheduling;
using CC.Shared.Domain.TimeRanges;
using Mediator;

namespace CC.AppointmentService.Api.Endpoints.Appointments;

public static class TransferAppointmentEndpoint
{
    public static async Task<IResult> Handle(Guid id,
        TransferAppointmentRequest request,
        IMediator mediator)
    {
        var result = await mediator.Send(new TransferAppointment()
        {
            AppointmentId = id,
            TimeSlot = new DateTimeRange(request.From, request.To)
        });
        return Results.Ok(result);
    }
}