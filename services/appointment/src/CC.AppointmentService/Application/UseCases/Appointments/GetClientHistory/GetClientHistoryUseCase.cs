using CC.AppointmentService.Application.Dependencies.AppointmentsQuery;
using CC.Common.Models;

namespace CC.AppointmentService.Application.UseCases.Appointments.GetClientHistory;

public sealed record GetClientHistory(Guid ClientId, DateTime? From, DateTime? To, PageInfo PageInfo);

public class GetClientHistoryUseCase(IAppointmentQueryService service)
{
    public async ValueTask<PagedResult<AppointmentPageItemDto>> Handle(GetClientHistory command,
        CancellationToken cancellationToken)
    {
        var query = new ClientAppointmentsQuery(command.ClientId, command.From,
            command.To, command.PageInfo);
        var pagedResult = await service.GetAppointmentsAsync(query, cancellationToken);

        return pagedResult;
    }
}