using CC.Common.Models;

namespace CC.AppointmentService.Application.Dependencies.AppointmentsQuery;

public interface IAppointmentQueryService
{
    public Task<PagedResult<AppointmentPageItemDto>> GetAppointmentsAsync(ClientAppointmentsQuery request,
        CancellationToken ct);
}