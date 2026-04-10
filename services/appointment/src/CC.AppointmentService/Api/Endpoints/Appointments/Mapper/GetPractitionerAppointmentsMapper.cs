using CC.AppointmentService.Api.Contracts.Appointments.Practitioner;
using CC.AppointmentService.Application.UseCases.Appointments.Getting;
using Riok.Mapperly.Abstractions;

namespace CC.AppointmentService.Api.Endpoints.Appointments.Mapper;

[Mapper]
public static partial class GetPractitionerAppointmentsMapper
{
    public static partial GetPractitionerAppointments ToUseCase(GetPractitionerAppointmentRequest request);
}