using CC.PractitionerService.Application.UseCases.WorkSchedules.Dtos;
using CC.PractitionerService.Domain.Practitioners;
using Riok.Mapperly.Abstractions;

namespace CC.PractitionerService.Api.Mappers;

[Mapper]
public partial class PractitionerMapper
{
    [MapProperty(nameof(PractitionerProfile.Specializations.AgeGroups), nameof(PractitionerDto.AgeGroups))]
    [MapProperty(nameof(PractitionerProfile.Specializations.Languages), nameof(PractitionerDto.Languages))]
    [MapProperty(nameof(PractitionerProfile.Specializations.ProblemAreas), nameof(PractitionerDto.ProblemAreas))]
    public partial PractitionerDto ToDto(PractitionerProfile practitionerProfile);
}
