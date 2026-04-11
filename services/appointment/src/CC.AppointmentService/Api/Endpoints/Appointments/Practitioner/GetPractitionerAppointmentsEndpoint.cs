using CC.AppointmentService.Api.Contracts.Appointments.Practitioner;
using CC.AppointmentService.Api.Endpoints.Appointments.Mapper;
using Mediator;

namespace CC.AppointmentService.Api.Endpoints.Appointments.Practitioner;

public static class GetPractitionerAppointmentsEndpoint
{
    public static async Task<IResult> Handle(
        [AsParameters] GetPractitionerAppointmentRequest request,
        IMediator mediator,
        CancellationToken ct)
    {
        var result = await mediator.Send(GetPractitionerAppointmentsMapper.ToUseCase(request), ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }
}