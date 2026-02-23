namespace CC.PractitionerService.Api.Endpoints.WorkSchedules;

public static class WorkSchedulesEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapWorkSchedulesEndpoints()
        {
            var v1Group = builder.MapGroup("api/v1/work-schedules");
            v1Group.MapPost("", CreateWorkScheduleEndpoint.Handle);
            v1Group.MapPost("{id:guid}/adjustments", AddAdjustmentsEndpoint.Handle);
        }
    }
}