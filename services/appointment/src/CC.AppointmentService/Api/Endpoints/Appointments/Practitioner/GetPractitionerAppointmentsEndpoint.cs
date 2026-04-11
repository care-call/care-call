using CC.AppointmentService.Api.Contracts.Appointments.Practitioner;
using CC.AppointmentService.Api.Endpoints.Appointments.Mapper;
using CC.AppointmentService.Application.UseCases.Appointments.Getting;
using FluentResults;
using Wolverine;

namespace CC.AppointmentService.Api.Endpoints.Appointments.Practitioner;

public static class GetPractitionerAppointmentsEndpoint
{
    public static async Task<IResult> Handle(
        [AsParameters] GetPractitionerAppointmentRequest request,
        IMessageBus bus,
        CancellationToken ct)
    {
        var result = await bus.InvokeAsync<Result<AppointmentListItem[]>>(
            GetPractitionerAppointmentsMapper.ToUseCase(request), ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }
}