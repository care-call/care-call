using CC.PractitionerService.Api.Contracts;
using CC.PractitionerService.Application.UseCases.WorkSchedules.AddAdjustments;
using Riok.Mapperly.Abstractions;

namespace CC.PractitionerService.Api.Mappers;

[Mapper]
public partial class SaveWorkScheduleAdjustmentsMapper
{
    public partial SaveWorkScheduleAdjustments MapFrom(SaveWorkScheduleAdjustmentsRequest request, Guid workScheduleId);
}