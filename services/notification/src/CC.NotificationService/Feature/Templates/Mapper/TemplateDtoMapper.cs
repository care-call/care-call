using CC.NotificationService.Domain.Templates;
using Riok.Mapperly.Abstractions;

namespace CC.NotificationService.Feature.Templates.Mapper;

public sealed record TemplateDto(Guid Id, string Key, bool IsActive, DateTime CreatedAt);

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class TemplateDtoMapper
{
    public static partial TemplateDto From(Template src);
    public static partial IQueryable<TemplateDto> MapToDto(this IQueryable<Template> src);
}