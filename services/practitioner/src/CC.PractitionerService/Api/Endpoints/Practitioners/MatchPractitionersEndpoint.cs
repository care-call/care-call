using CC.PractitionerService.Api.Contracts.Practitioners;
using CC.PractitionerService.Application.UseCases.Practitioners;
using CC.PractitionerService.Application.UseCases.Practitioners.Dtos;
using FluentResults;
using Wolverine;

namespace CC.PractitionerService.Api.Endpoints.Practitioners;

public static class MatchPractitionersEndpoint
{
    public static async Task<IResult> Handle(
        MatchPractitionersRequest request,
        IMessageBus bus)
    {
        var result = await bus.InvokeAsync<Result<PractitionerDto[]>>(new AvailablePractitionerFilter()
        {
            PeriodFrom = request.PeriodFrom,
            PeriodTo = request.PeriodTo,
            AgeGroupIds = request.AgeGroupIds,
            ProblemAreas = request.ProblemAreas,
            PractitionerFullName = request.PractitionerFullName,
            PractitionerLanguages = request.PractitionerLanguages,
        });
        return Results.Ok(result);
    }
}
