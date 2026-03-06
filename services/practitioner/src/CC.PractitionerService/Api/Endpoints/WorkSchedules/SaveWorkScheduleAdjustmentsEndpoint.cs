using CC.PractitionerService.Api.Contracts;
using CC.PractitionerService.Api.Mappers;
using Mediator;

namespace CC.PractitionerService.Api.Endpoints.WorkSchedules;

public static class SaveWorkScheduleAdjustmentsEndpoint
{
    public static async Task<IResult> Handle(Guid id,
        SaveWorkScheduleAdjustmentsRequest request,
        IMediator mediator)
    {
        var mapper = new SaveWorkScheduleAdjustmentsMapper();
        var command = mapper.MapFrom(request);
        var result = await mediator.Send(command, CancellationToken.None);

        return Results.Ok(result);
    }
}