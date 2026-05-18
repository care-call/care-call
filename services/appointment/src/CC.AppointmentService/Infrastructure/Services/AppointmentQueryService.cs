using CC.AppointmentService.Application.Dependencies.AppointmentsQuery;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Infrastructure.Persistence;
using CC.Common.Models;
using CC.Common.Pagination;

namespace CC.AppointmentService.Infrastructure.Services;

public class AppointmentQueryService(DatabaseContext dbContext,
    TimeProvider timeProvider) : IAppointmentQueryService
{
    public async Task<PagedResult<AppointmentPageItemDto>> GetAppointmentsAsync(ClientAppointmentsQuery query,
        CancellationToken ct)
    {
        var now = timeProvider.GetUtcNow().DateTime;
        var appointmentsQuery = dbContext.Appointments.Where(a => a.ClientId == query.ClientId);

        appointmentsQuery = ApplySort(appointmentsQuery, now);
        appointmentsQuery = ApplyFilters(appointmentsQuery,new ClientHistoryFilterDto(query.From, query.To));
        
        var pagedResult = await appointmentsQuery.ToPagedResultAsync(
            query.PageInfo,
            a => new AppointmentPageItemDto
            {
                Id =  a.Id,
                PractitionerFullName = a.PractitionerSnapshot.FullName.GetFullName(),
                DateOfEvent = a.TimeSlot.From
            },
            ct);
        
        return pagedResult;
    }
    
    private IQueryable<Appointment> ApplyFilters(IQueryable<Appointment> query, ClientHistoryFilterDto clientHistoryFilters)
    {
        if(clientHistoryFilters.From is not null)
            query = query.Where(a => a.TimeSlot.From >= clientHistoryFilters.From);
        
        if(clientHistoryFilters.To is not null)
            query = query.Where(a => a.TimeSlot.From <= clientHistoryFilters.To);
        
        return query;
    }

    private IQueryable<Appointment> ApplySort(IQueryable<Appointment> query, DateTime now)
    {
        return query
            .OrderByDescending(a => a.TimeSlot.From >= now)
            .ThenBy(a => a.TimeSlot.From >= now ? a.TimeSlot.From : DateTime.MaxValue)
            .ThenByDescending(a => a.TimeSlot.From);
    }
}