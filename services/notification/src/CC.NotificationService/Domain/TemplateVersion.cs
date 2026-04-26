namespace CC.NotificationService.Domain;

public class TemplateVersion
{
    public Guid Id { get; set; }
    public string TemplateKey { get; set; }
    public Template Template { get; set; }
    public int Version { get; set; }
    public bool IsActive { get; set; }
    public List<ChannelContext> Channels { get; set; }
    public DateTime CreateAt { get; set; }
}