using CC.Shared.Domain;

namespace CC.PractitionerService.Domain.Practitioners;

public sealed class PractitionerProfile(Guid id) : AggregationRoot<Guid>(id)
{
    public PhotoUrl? PhotoUrl { get; init; }
    public string Bio { get; init; }
    public FullName FullName { get; init; }
    public PractitionerProfileStatus Status { get; init; } = PractitionerProfileStatus.Draft;
    public PractitionerSpecializations Specializations { get; init; }
}