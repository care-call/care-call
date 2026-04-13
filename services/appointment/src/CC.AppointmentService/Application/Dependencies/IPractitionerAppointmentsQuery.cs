using CC.AppointmentService.Application.UseCases.Appointments.Getting;

namespace CC.AppointmentService.Application.Dependencies;

public interface IPractitionerAppointmentsQuery
{
    Task<AppointmentListItem[]> GetAsync(
        GetPractitionerAppointments query,
        CancellationToken ct);
}