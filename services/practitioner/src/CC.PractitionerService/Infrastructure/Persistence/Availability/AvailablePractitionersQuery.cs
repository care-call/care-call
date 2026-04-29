using CC.PractitionerService.Application.Dependencies;
using CC.PractitionerService.Application.UseCases.Practitioners;
using CC.PractitionerService.Application.UseCases.Practitioners.Dtos;
using CC.PractitionerService.Domain.Availability;
using CC.PractitionerService.Domain.Availability.Services;
using CC.PractitionerService.Domain.Practitioners;
using CC.PractitionerService.Domain.WorkSchedules;
using CC.Shared.Domain.TimeRanges;
using Microsoft.EntityFrameworkCore;

namespace CC.PractitionerService.Infrastructure.Persistence.Availability;

public sealed class AvailablePractitionersQuery(
    DatabaseContext databaseContext,
    TimeProvider timeProvider) : IAvailablePractitionersQuery
{
    private static readonly TimeSpan BookingAdvanceBuffer = TimeSpan.FromHours(4);
    private static readonly TimeSpan MaxPeriod = TimeSpan.FromDays(30);

    public async Task<PractitionerDto[]> FindAvailableAsync(AvailablePractitionerFilter filter, CancellationToken ct)
    {
        var query = BuildQuery(filter);
        query = ApplyPaging(query, filter);

        var period = GetPeriod(filter.PeriodFrom, filter.PeriodTo);

        var practitionersData = await FetchPractitionersDataAsync(query, ct);
        return MapAndFilterByAvailability(practitionersData, period);
    }

    private IQueryable<PractitionerProfile> BuildQuery(AvailablePractitionerFilter filter)
    {
        var query = databaseContext.PractitionerProfiles
            .AsNoTracking()
            .Where(x => x.Status == PractitionerProfileStatus.Approved);

        if (filter.AgeGroupIds?.Count > 0)
            query = query.Where(p => p.Specializations.AgeGroups.Any(ag => filter.AgeGroupIds.Contains(ag)));

        if (filter.ProblemAreas?.Count > 0)
            query = query.Where(p => p.Specializations.ProblemAreas.Any(pa => filter.ProblemAreas.Contains(pa)));

        if (filter.PractitionerLanguages?.Count > 0)
            query = query.Where(p => p.Specializations.Languages.Any(lang => filter.PractitionerLanguages.Contains(lang)));

        if (!string.IsNullOrWhiteSpace(filter.PractitionerFullName))
        {
            query = query.Where(p =>
                (p.FullName.Surname + " " + p.FullName.Name + " " + (p.FullName.Patronymic ?? ""))
                    .Contains(filter.PractitionerFullName)
            );
        }

        return query
            .OrderByDescending(x => x.Rating)
            .ThenBy(x => x.Id);
    }

    private static IQueryable<PractitionerProfile> ApplyPaging(
        IQueryable<PractitionerProfile> query,
        AvailablePractitionerFilter filter)
    {
        return query
            .Skip(filter.PageNumber * filter.PageSize)
            .Take(filter.PageSize);
    }

    private Task<PractitionerFetchData[]> FetchPractitionersDataAsync(
        IQueryable<PractitionerProfile> query,
        CancellationToken ct)
    {
        return query.Select(p => new PractitionerFetchData(
            p,
            databaseContext.WorkSchedules
                .Where(ws => ws.PractitionerId == p.Id)
                .Select(s => new ScheduleWithAdjustments(
                    s,
                    databaseContext.Adjustments.Where(a => a.WorkScheduleId == s.Id).ToArray()
                ))
                .ToArray(),
            databaseContext.Employments
                .Where(e => e.PractitionerId == p.Id && !e.IsDeleted)
                .Select(e => new EmploymentSnapshot(e.PractitionerId, e.ExternalEmploymentKey, e.Period, e.CreatedAt))
                .ToArray()
        ))
        .AsSplitQuery()
        .ToArrayAsync(ct);
    }

    private DateTimeRange GetPeriod(DateOnly periodFrom, DateOnly periodTo)
    {
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;
        var currentDay = DateOnly.FromDateTime(utcNow);

        if (periodFrom < currentDay)
            throw new InvalidOperationException("Дата начала выбранного периода не может быть раньше текущей даты");

        var period = new DateTimeRange(
            periodFrom == currentDay
                ? utcNow.Add(BookingAdvanceBuffer)
                : periodFrom.ToDateTime(TimeOnly.MinValue),
            periodTo.ToDateTime(TimeOnly.MaxValue));

        if ((period.To - period.From) > MaxPeriod)
            throw new InvalidOperationException($"Продолжительность периода превысила заданное значение: {MaxPeriod}");

        return period;
    }

    private static PractitionerDto[] MapAndFilterByAvailability(PractitionerFetchData[] data, DateTimeRange period)
    {
        var mapper = new PractitionerMapper();
        var availabilityCalculator = new PractitionerAvailabilityCalculator();
        return [.. data
            .Select(p => mapper.ToDto(
                p.Profile,
                availabilityCalculator.Calculate(
                    p.Schedules.Select(x => x.Schedule),
                    p.Schedules.SelectMany(x => x.Adjustments),
                    p.Employments,
                    period)))
            .Where(dto => dto.Availabilities.Count > 0)];
    }

    private sealed record PractitionerFetchData(
        PractitionerProfile Profile,
        ScheduleWithAdjustments[] Schedules,
        EmploymentSnapshot[] Employments);

    private sealed record ScheduleWithAdjustments(
        WorkSchedule Schedule,
        Adjustment[] Adjustments);
}