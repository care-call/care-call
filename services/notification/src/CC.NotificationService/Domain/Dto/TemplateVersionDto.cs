namespace CC.NotificationService.Domain.Dto;

public class TemplateVersionDto
{
    public int Version { get; set; }
    public string TemplateKey { get; set; }
    public bool IsActive { get; set; }
    public List<ChannelDto> Channels { get; set; }
}