namespace CC.NotificationService.Domain;

public class Template
{
    public Guid Id { get; set; }
    public string Key { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateAt { get; set; }
}