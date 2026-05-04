namespace CC.NotificationService.Domain.Templates;

public class ChannelContext
{
    public ChannelType Channel { get; set; }
    public string Content { get; set; } = string.Empty;
}