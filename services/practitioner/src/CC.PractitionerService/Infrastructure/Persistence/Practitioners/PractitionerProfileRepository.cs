using CC.PractitionerService.Domain.Practitioners;
using CC.PractitionerService.Domain.Practitioners.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CC.PractitionerService.Infrastructure.Persistence.Practitioners;

internal class PractitionerProfileRepository(DatabaseContext db) : IPractitionerProfileRepository
{
    public Task<PractitionerProfile?> GetAsync(Guid practitionerProfileId) =>
         db.PractitionerProfiles.FirstOrDefaultAsync(p => p.Id == practitionerProfileId);

    public async Task AddAsync(PractitionerProfile practitionerProfile) =>
        await db.PractitionerProfiles.AddAsync(practitionerProfile);
}