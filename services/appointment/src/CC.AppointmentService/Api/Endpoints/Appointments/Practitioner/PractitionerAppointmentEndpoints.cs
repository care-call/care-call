namespace CC.AppointmentService.Api.Endpoints.Appointments.Practitioner;

public static class PractitionerAppointmentEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapPractitionerAppointmentEndpoints()
        {
            var v1Group = builder.MapGroup("api/v1/practitioner");
            
            v1Group.MapGet("", GetPractitionerAppointmentsEndpoint.Handle);
        }
    }
}