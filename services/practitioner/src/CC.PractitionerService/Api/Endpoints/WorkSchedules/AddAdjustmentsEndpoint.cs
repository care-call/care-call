using CC.PractitionerService.Api.Contracts;
using CC.PractitionerService.Application.UseCases;
using CC.PractitionerService.Application.UseCases.WorkSchedules.AddAdjustments;
using Mediator;
using AdjustmentDto = CC.PractitionerService.Api.Contracts.Common.AdjustmentDto;

namespace CC.PractitionerService.Api.Endpoints.WorkSchedules;

public static class AddAdjustmentsEndpoint
{
    public static async Task<IResult> Handle(Guid id, 
        CreateAdjustmentsForScheduleRequest request,
        IMediator mediator)
    {
        var command = new CreateAdjustmentsForSchedule()
        {
           WorkScheduleId = id,
           Adjustments = request.Adjustments.Select(a => new AdjustmentDto
           {
               AdjustmentType = a.AdjustmentType,
               StartDate = a.StartDate,
               EndDate = a.EndDate
           }).ToList()
        };
        
        var result = await mediator.Send(command, CancellationToken.None);
        
        return Results.Ok(result);
    }
}