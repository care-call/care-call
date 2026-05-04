namespace CC.NotificationService.Domain.Notifications;

public sealed record NotificationPayload(Dictionary<string, string> Data);