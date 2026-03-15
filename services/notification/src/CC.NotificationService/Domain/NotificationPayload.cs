namespace CC.NotificationService.Domain;

public sealed record NotificationPayload(Dictionary<string, string> Data);