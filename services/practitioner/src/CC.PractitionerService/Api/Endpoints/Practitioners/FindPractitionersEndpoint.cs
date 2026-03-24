using CC.PractitionerService.Api.Contracts.Practitioners;
using CC.PractitionerService.Application.UseCases.Practitioners;
using Mediator;

namespace CC.PractitionerService.Api.Endpoints.Practitioners;

public static class FindPractitionersEndpoint
{
    public static async Task<IResult> Handle(
        FindPractitionersRequest request,
        IMediator mediator)
    {
        var result = await mediator.Send(new FindPractitionersFilter()
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
