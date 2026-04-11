namespace CC.AppointmentService.Api.Endpoints.Appointments.Practitioner;

public static class PractitionerAppointmentEndpoints
{
    extension(IEndpointRouteBuilder builder)
    {
        public void MapPractitionerAppointmentEndpoints()
        {
            var v1Group = builder.MapGroup("api/v1/practitioners");
            
            v1Group.MapGet("{practitionerId}/appointments", GetPractitionerAppointmentsEndpoint.Handle);
        }
    }
}