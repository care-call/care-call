using CC.PractitionerService.Api.Contracts;
using CC.PractitionerService.Application.UseCases.WorkSchedules.AddAdjustments;
using Mediator;

namespace CC.PractitionerService.Api.Endpoints.WorkSchedules;

public static class AddAdjustmentsEndpoint
{
    public static async Task<IResult> Handle(Guid id,
        SaveAdjustmentsForScheduleRequest request,
        IMediator mediator)
    {
        var command = new SaveAdjustmentsForSchedule()
        {
            NewAdjustments = request.NewAdjustments,
            RemovedAdjustments = request.RemovedAdjustments,
            WorkScheduleId = request.WorkScheduleId,
            WeeklyStartDate = request.StartDate
        };

        var result = await mediator.Send(command, CancellationToken.None);

        return Results.Ok(result);
    }
}