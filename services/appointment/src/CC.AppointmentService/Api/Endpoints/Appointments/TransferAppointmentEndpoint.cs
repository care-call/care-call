using CC.AppointmentService.Api.Contracts.Appointments;
using CC.AppointmentService.Application.UseCases.Appointments.Transferring;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Wolverine;

namespace CC.AppointmentService.Api.Endpoints.Appointments;

public static class TransferAppointmentEndpoint
{
    public static async Task<IResult> Handle(Guid id,
        TransferAppointmentRequest request,
        IMessageBus bus)
    {
        var result = await bus.InvokeAsync<Result>(new TransferAppointment
        {
            AppointmentId = id,
            TimeSlot = new DateTimeRange(request.From, request.To),
            ClientId = request.ClientId
        });
        return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Errors);
    }
}