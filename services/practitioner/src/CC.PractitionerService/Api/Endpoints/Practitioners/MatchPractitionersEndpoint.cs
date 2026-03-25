using CC.PractitionerService.Api.Contracts.Practitioners;
using CC.PractitionerService.Application.UseCases.Practitioners;
using Mediator;

namespace CC.PractitionerService.Api.Endpoints.Practitioners;

public static class MatchPractitionersEndpoint
{
    public static async Task<IResult> Handle(
        MatchPractitionersRequest request,
        IMediator mediator)
    {
        var result = await mediator.Send(new MatchPractitionersFilter()
        {
            TargetDate = request.TargetDate,
            AgeGroupIds = request.AgeGroupIds,
            ProblemAreas = request.ProblemAreas,
            PractitionerFullName = request.PractitionerFullName,
            PractitionerLanguages = request.PractitionerLanguages,
        });
        return Results.Ok(result.Value);
    }
}
