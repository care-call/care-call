using CC.PractitionerService.Api.Contracts;
using CC.PractitionerService.Application.UseCases.WorkSchedules.AddAdjustments;
using Mediator;

namespace CC.PractitionerService.Api.Endpoints.WorkSchedules;

public static class SaveWorkSheduleAdjustmentsEndpoint
{
    public static async Task<IResult> Handle(Guid id,
        SaveWorkScheduleAdjustmentsRequest request,
        IMediator mediator)
    {
        var command = new SaveWorkScheduleAdjustments()
        {
            NewAdjustments = request.NewAdjustments,
            RemovedAdjustments = request.RemovedAdjustments,
            WorkScheduleId = request.WorkScheduleId,
            WeeklyStartDate = request.WeeklyStartDate
        };

        var result = await mediator.Send(command, CancellationToken.None);

        return Results.Ok(result);
    }
}