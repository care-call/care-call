using CC.AppointmentService.Api.Contracts.Appointments;
using CC.AppointmentService.Application.UseCases.Appointments.Creation;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Wolverine;

namespace CC.AppointmentService.Api.Endpoints.Appointments;

public static class CreateAppointmentEndpoint
{
    public static async Task<IResult> Handle(
        CreateAppointmentRequest request,
        IMessageBus bus)
    {
        var result = await bus.InvokeAsync<Result>(new CreateAppointment
        {
            PractitionerId = request.PractitionerId,
            ClientId = request.ClientId,
            TimeSlot = new DateTimeRange(request.StartedAt, request.EndedAt),
            ClientSnapshot = request.ClientSnapshot,
            PractitionerSnapshot = request.PractitionerSnapshot
        });
        return result.IsSuccess ? Results.Created() : Results.BadRequest(result.Errors);
    }
}