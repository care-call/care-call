using CC.Shared.Domain;

namespace CC.NotificationService.Domain.Notifications;

public sealed partial class Notification : AggregationRoot<Guid>
{
    #pragma warning disable CS8618 // Для EF core.
    private Notification(Guid id) : base(id)
    {
    }

    private Notification(
        Guid id,
        string trigger,
        DateTime createdAt,
        NotificationPayload payload,
        ICollection<NotificationChannel> channels) : base(id)
    {
        Trigger = trigger;
        CreatedAt = createdAt;
        Payload = payload;
        Channels = channels;
    }

    public string Trigger { get; private set; }

    public DateTime? ValidUntil { get; private set; }
    public DateTime? SentAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public NotificationPayload Payload { get; private set; }
    public ICollection<NotificationChannel> Channels { get; }

    public NotificationBuilder Create(
        Guid id,
        string trigger,
        DateTime createdAt,
        NotificationPayload payload,
        ICollection<NotificationChannel> channels)
    {
        var notification = new Notification(id, trigger, createdAt, payload, channels);
        return CreateBuilder(notification);
    }

    public void MarkAsSent(DateTime sentAt)
    {
        if (Channels.Any(ch => ch.Status != NotificationChannelStatus.Sent))
            return;
        SentAt = sentAt;
    }

    public void AddChannel(Guid channelId, NotificationChannelType type)
    {
        Channels.Add(new NotificationChannel(channelId, notificationId: Id, type));
    }
}

public sealed partial class Notification
{
    private static NotificationBuilder CreateBuilder(Notification notification)
    {
        return new NotificationBuilder(notification);
    }

    public class NotificationBuilder(Notification notification)
    {
        public NotificationBuilder ValidUntil(DateTime value)
        {
            notification.ValidUntil = value;
            return this;
        }
        
        public NotificationBuilder WithChannel(Guid channelId, NotificationChannelType type)
        {
            notification.AddChannel(channelId, type);
            return this;
        }

        public Notification Build()
        {
            return notification;
        }
    }
}