namespace CC.AppointmentService.Api.Endpoints.Appointments;

public static class AppointmentsEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapAppointmentsEndpoints()
        {
            var v1Group = builder.MapGroup("api/v1/appointments");
            v1Group.MapPost("", CreateAppointmentEndpoint.Handle);
        }
    }
}