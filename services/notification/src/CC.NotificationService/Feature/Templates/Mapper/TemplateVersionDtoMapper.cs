using CC.NotificationService.Domain.Templates;
using CC.NotificationService.Feature.Templates.CreateVersion;
using Riok.Mapperly.Abstractions;

namespace CC.NotificationService.Feature.Templates.Mapper;

public sealed record TemplateVersionDto(Guid Id, string TemplateKey,
    bool IsActive, int Version, List<ChannelDto> Channels, DateTime CreatedAt);

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class TemplateVersionDtoMapper
{
    public static partial TemplateVersionDto From(TemplateVersion src);
    public static partial IQueryable<TemplateVersionDto> MapToDto(this IQueryable<TemplateVersion> src);
}