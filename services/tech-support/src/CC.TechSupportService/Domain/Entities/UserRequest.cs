namespace CC.TechSupportService.Domain.Entities;

public class UserRequest
{
    public long Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public byte AmountOfRequestForLastHour { get; set; }
    
    public DateTime LastRequestTime { get; set; }
}