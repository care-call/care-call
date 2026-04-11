using CC.AppointmentService.Application.Dependencies;
using CC.AppointmentService.Application.UseCases.Appointments.Getting;
using CC.AppointmentService.Domain.Appointments;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CC.AppointmentService.Infrastructure.Persistence.Appointments;

internal sealed class PractitionerAppointmentsQuery(
    DatabaseContext db,
    TimeProvider timeProvider) : IPractitionerAppointmentsQuery
{
    public Task<AppointmentListItem[]> GetAsync(GetPractitionerAppointments query, CancellationToken ct)
    {
        var todayStart = timeProvider.GetLocalNow().Date;

        return ApplyOrderByTimeSlot(db.Appointments
            .AsNoTracking()
            .Where(x => x.PractitionerId == query.PractitionerId)
            .Where(BuildDateFilter(query.DateFilter, todayStart))
            .Where(BuildStateFilter(query.StateFilter)),
                query.OrderFilter)
            .Select(x => new AppointmentListItem
            {
                StartedAt = x.TimeSlot.From,
                ClientFullName = x.ClientSnapshot.FullName,
                Status = x.Status
            })
            .ToArrayAsync(ct);
    }

    private static Expression<Func<Appointment, bool>> BuildDateFilter(
        PractitionerAppointmentsDateFilter filter,
        DateTime todayStart)
    {
        var today = todayStart.Date;
        var tomorrow = today.AddDays(1);
        var weekEnd = today.AddDays(7);

        return filter switch
        {
            PractitionerAppointmentsDateFilter.Today => x => x.TimeSlot.From.Date == today,
            PractitionerAppointmentsDateFilter.Tomorrow => x => x.TimeSlot.From.Date == tomorrow,
            PractitionerAppointmentsDateFilter.Week => x => x.TimeSlot.From.Date >= today && x.TimeSlot.From.Date <= weekEnd,
            _ => x => true
        };
    }

    private static IOrderedQueryable<Appointment> ApplyOrderByTimeSlot(
        IQueryable<Appointment> query,
        PractitionerAppointmentOrderFilter filter
    )
    {
        return filter switch
        {
            PractitionerAppointmentOrderFilter.Descending => query.OrderByDescending(x => x.TimeSlot.From),
            _ => query.OrderBy(x => x.TimeSlot.From)
        };
    }

    private static Expression<Func<Appointment, bool>> BuildStateFilter(
        PractitionerAppointmentsStateFilter filter)
    {
        return filter switch
        {
            PractitionerAppointmentsStateFilter.Upcoming =>
                x => x.Status == AppointmentStatus.Planned,
            PractitionerAppointmentsStateFilter.Completed =>
                x => x.Status == AppointmentStatus.Completed,
            PractitionerAppointmentsStateFilter.Cancel =>
                x => x.Status == AppointmentStatus.Cancelled,
            _ => x => true
        };
    }
}