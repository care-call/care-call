namespace CC.NotificationService.Domain.Dto;

public class ChannelDto
{
    public ChannelType Channel { get; set; }
    public string Content { get; set; } = string.Empty;
}