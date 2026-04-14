using CC.AppointmentService.Api.Contracts;
using CC.AppointmentService.Api.Endpoints.Appointments.Mapper;
using CC.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace CC.AppointmentService.Api.Endpoints.Appointments;

public static class GetHistoryClientEndpoint
{
    public static async Task<IResult> Handle(
        [FromQuery] Guid clientId,
        GetClientHistoryRequest request,
        IMessageBus bus)
    {
        var result = await bus.InvokeAsync<PagedResult<AppointmentPageItemDto>>(
            GetClientHistoryAppointmentsMapper.ToUseCase(clientId, request));
        return Results.Ok(result);
    }
}