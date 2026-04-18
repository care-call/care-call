using CC.PractitionerService.Infrastructure.Persistence.Availability.Abstractions;
using CC.PractitionerService.Infrastructure.Persistence.Availability.Models;
using Microsoft.EntityFrameworkCore;

namespace CC.PractitionerService.Infrastructure.Persistence.Availability;

public sealed class EmploymentRecordStorage(DatabaseContext databaseContext) : IEmploymentRecordStorage
{
    public Task CreateAsync(EmploymentRecord employment)
        => databaseContext.Employments.AddAsync(employment).AsTask();

    public Task<EmploymentRecord?> GetByKeyAsync(string key)
        => databaseContext.Employments.FirstOrDefaultAsync(x => x.ExternalEmploymentKey == key);
}