using CC.PractitionerService.Api.Contracts;
using CC.PractitionerService.Api.Mappers;
using FluentResults;
using Wolverine;

namespace CC.PractitionerService.Api.Endpoints.WorkSchedules;

public static class SaveWorkScheduleAdjustmentsEndpoint
{
    public static async Task<IResult> Handle(Guid id,
        SaveWorkScheduleAdjustmentsRequest request,
        IMessageBus bus)
    {
        var mapper = new SaveWorkScheduleAdjustmentsMapper();
        var command = mapper.MapFrom(request, id);
        var result = await bus.InvokeAsync<Result>(command);

        return Results.Ok(result);
    }
}