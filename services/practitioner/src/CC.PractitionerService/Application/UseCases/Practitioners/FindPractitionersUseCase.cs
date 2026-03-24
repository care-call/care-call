using CC.PractitionerService.Api.Mappers;
using CC.PractitionerService.Application.UseCases.WorkSchedules.Dtos;
using CC.PractitionerService.Domain.Practitioners;
using CC.PractitionerService.Infrastructure.Persistence;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using Mediator;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CC.PractitionerService.Application.UseCases.Practitioners;

public sealed record FindPractitionersFilter : IRequest<Result<PractitionerDto[]>>
{
    public DateOnly? TargetDate { get; init; }
    public IReadOnlyCollection<int>? AgeGroupIds { get; init; }
    public IReadOnlyCollection<int>? ProblemAreas { get; init; }
    public IReadOnlyCollection<int>? PractitionerLanguages { get; init; }
    public string? PractitionerFullName { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
}

public interface IPractitionerReadRepository
{
    Task<PractitionerDto[]> FindByFilterAsync(
        FindPractitionersFilter filter,
        CancellationToken ct);
}

public sealed class FindPractitionersUseCase(
    IPractitionerReadRepository practitionerReadRepository,
    ISlotsReadRepository _slotsRepository) : IRequestHandler<FindPractitionersFilter, Result<PractitionerDto[]>>
{
    public async ValueTask<Result<PractitionerDto[]>> Handle(FindPractitionersFilter filter, CancellationToken ct)
    {
        var practitioners = await practitionerReadRepository.FindByFilterAsync(filter, ct);

        if (filter.TargetDate is null)
            return Result.Ok(practitioners);

        var ids = practitioners.Select(x => x.Id).ToList();
        var slotsMap = await _slotsRepository.GetFreeSlotsAsync(ids, filter.TargetDate.Value, ct);

        var result = practitioners
            .Where(p => slotsMap.TryGetValue(p.Id, out var slots) && slots.Count > 0)
            .Select(p => p with { FreeSlots = slotsMap.GetValueOrDefault(p.Id, []) })
            .ToArray();

        return Result.Ok(result);
    }
}

public sealed class PractitionerReadRepository(DatabaseContext _db) : IPractitionerReadRepository
{
    public Task<PractitionerDto[]> FindByFilterAsync(FindPractitionersFilter filter, CancellationToken ct)
    {
        var query = _db.PractitionerProfiles.Where(x => x.Status == PractitionerProfileStatus.Approved);

        if (filter.AgeGroupIds?.Count > 0)
            query = query.Where(p => p.Specializations.AgeGroups.Any(pa => filter.AgeGroupIds.Contains(pa)));

        if (filter.ProblemAreas?.Count > 0)
            query = query.Where(p => p.Specializations.ProblemAreas.Any(pa => filter.ProblemAreas.Contains(pa)));

        if (filter.PractitionerLanguages?.Count > 0)
            query = query.Where(p => p.Specializations.Languages.Any(pa => filter.PractitionerLanguages.Contains(pa)));

        if (!string.IsNullOrWhiteSpace(filter.PractitionerFullName))
        {
            var name = filter.PractitionerFullName;

            query = query.Where(p =>
                (p.FullName.Surname + " " + p.FullName.Name + " " + (p.FullName.Patronymic ?? ""))
                    .Contains(name)
            );
        }

        query = query.OrderByDescending(x => x.Rating);

        query = query.Skip(filter.PageSize * filter.PageNumber)
            .Take(filter.PageSize);

        var mapper = new PractitionerMapper();


        return query.Select(x => mapper.ToDto(x)).ToArrayAsync(cancellationToken: ct);
    }
}