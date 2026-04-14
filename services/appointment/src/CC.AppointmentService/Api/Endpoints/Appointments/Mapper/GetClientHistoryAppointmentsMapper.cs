using CC.AppointmentService.Api.Contracts;
using CC.AppointmentService.Application.UseCases.Appointments.GetClientHistory;

namespace CC.AppointmentService.Api.Endpoints.Appointments.Mapper;

public static class GetClientHistoryAppointmentsMapper
{
    public static GetClientHistory ToUseCase(Guid clientId, GetClientHistoryRequest request) =>
        new(clientId, request.Filters?.From, request.Filters?.To, request.PageInfo);
}