using CC.Shared.Domain;

namespace CC.NotificationService.Domain;

public sealed class NotificationChannel : Entity<Guid>
{
    
    #pragma warning disable CS8618 // Для EF core.
    private NotificationChannel(Guid id) : base(id)
    {
    }

    public NotificationChannel(Guid id, Guid notificationId, NotificationChannelType type) : base(id)
    {
        NotificationId = notificationId;
        Type = type;
        Status = NotificationChannelStatus.Pending;
    }

    public Guid NotificationId { get; private set; }
    public NotificationChannelType Type { get; private set; }
    public NotificationChannelStatus Status { get; private set; }

    public DateTime? SentAt { get; private set; }
    public string? Error { get; private set; }

    public void MarkAsSent(DateTime sentAt)
    {
        Status = NotificationChannelStatus.Sent;
        SentAt = sentAt;
    }

    public void MarkAsFailed(string error)
    {
        Status = NotificationChannelStatus.Failed;
        Error = error;
    }
}