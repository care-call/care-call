namespace CC.NotificationService.Domain;

public class TemplateVersion
{
    public Guid Id { get; init; }
    public string TemplateKey { get; init; }
    public Template Template { get; init; }
    public int Version { get; init; }
    public bool IsActive { get; init; }
    public List<ChannelContext> Channels { get; init; }
    public DateTime CreatedAt { get; init; }
}