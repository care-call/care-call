using CC.PractitionerService.Domain.Practitioners;
using Microsoft.EntityFrameworkCore;

namespace CC.PractitionerService.Infrastructure.Persistence.Seeding;

public class PractitionerProfileSeeder(DbContext ctx)
{
    private static readonly Guid PractitionerId = Guid.Parse("019c3c98-8e77-75b0-9316-66dd1fea6d30");
    public async Task SeedAsync(CancellationToken ct)
    {
        var dbSet = ctx.Set<PractitionerProfile>();
        var practitionerProfile = await dbSet.FindAsync([PractitionerId], ct);
        if (practitionerProfile is not null)
            return;

        practitionerProfile = new PractitionerProfile(PractitionerId)
        {
            Bio = "Я, Даниил Колбасенко, я люблю Стендоф",
            FullName = new FullName("Даниил", "Колбасенко", "Олегович"),
            Specializations = new PractitionerSpecializations([1], [1], [1]),
            Status = PractitionerProfileStatus.Approved
        };
        await dbSet.AddAsync(practitionerProfile, ct);
        await ctx.SaveChangesAsync(ct);
    }
}