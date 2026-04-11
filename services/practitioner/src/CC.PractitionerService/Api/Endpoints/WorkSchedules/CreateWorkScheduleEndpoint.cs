using CC.PractitionerService.Api.Contracts;
using CC.PractitionerService.Application.UseCases.WorkSchedules.Creation;
using CC.PractitionerService.Domain.WorkSchedules;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Wolverine;

namespace CC.PractitionerService.Api.Endpoints.WorkSchedules;

public static class CreateWorkScheduleEndpoint
{
    public static async Task<IResult> Handle(
        CreateWorkScheduleRequest request,
        Guid practitionerId,
        IMessageBus bus)
    {
        var result = await bus.InvokeAsync<Result>(new CreateWorkSchedule
        {
            PractitionerId = practitionerId,
            TimeZoneId = request.TimeZoneId,
            Recurrences = request.Recurrences
                .Select(x => new WeeklyRecurrence(x.Day, new TimeRange(x.StartTime, x.EndTime)))
                .ToList(),
            SessionDuration = new SessionDuration(request.SessionDuration),
            ValidityPeriod = request.ValidityPeriod.To.HasValue
                ? new WorkScheduleValidityPeriod(request.ValidityPeriod.From, request.ValidityPeriod.To.Value)
                : new WorkScheduleValidityPeriod(request.ValidityPeriod.From)
        });
        return Results.Ok(result);
    }
}