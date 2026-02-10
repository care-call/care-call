namespace CC.PractitionerService.Domain.Practitioners.Repositories;

public interface IPractitionerProfileRepository
{
    Task<PractitionerProfile?> GetAsync(Guid practitionerProfileId);
    Task AddAsync(PractitionerProfile practitionerProfile);
}