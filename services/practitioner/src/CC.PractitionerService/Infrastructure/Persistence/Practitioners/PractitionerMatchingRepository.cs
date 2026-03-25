using CC.PractitionerService.Api.Mappers;
using CC.PractitionerService.Application.UseCases.Practitioners;
using CC.PractitionerService.Application.UseCases.Practitioners.Dtos;
using CC.PractitionerService.Domain.Practitioners;
using Microsoft.EntityFrameworkCore;

namespace CC.PractitionerService.Infrastructure.Persistence.Practitioners;

public sealed class PractitionerMatchingRepository(DatabaseContext _db) : IPractitionerMatchingRepository
{
    public Task<PractitionerDto[]> FindAvailableForBookingAsync(MatchPractitionersFilter filter, CancellationToken ct)
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
            query = query.Where(p =>
                (p.FullName.Surname + " " + p.FullName.Name + " " + (p.FullName.Patronymic ?? ""))
                    .Contains(filter.PractitionerFullName)
            );
        }

        query = query
            .OrderByDescending(x => x.Rating)
            .Skip(filter.PageSize * filter.PageNumber)
            .Take(filter.PageSize);

        var mapper = new PractitionerMapper();

        return query.Select(x => mapper.ToDto(x)).ToArrayAsync(cancellationToken: ct);
    }
}