using CC.PractitionerService.Infrastructure.Persistence.Availability.Models;

namespace CC.PractitionerService.Infrastructure.Persistence.Availability.Abstractions;

public interface IEmploymentRecordStorage
{
    Task<EmploymentRecord?> GetByKeyAsync(string key);
    Task CreateAsync(EmploymentRecord employment);
}
