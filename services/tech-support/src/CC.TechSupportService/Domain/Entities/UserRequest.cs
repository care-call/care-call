using CC.Shared.Domain;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public class UserRequest : Entity<long>
{
    private UserRequest() : base(0) { }

    private UserRequest(GuidId userId, DateTime lastRequestTime) : base(0)
    {
        UserId = userId;
        AmountOfRequestForLastHour = 0;
        LastRequestTime = lastRequestTime;
    }
    
    public GuidId UserId { get; private set; }
    
    public byte AmountOfRequestForLastHour { get; private set; }
    
    public DateTime LastRequestTime { get; private set; }

    public static Result<UserRequest> TryCreate(GuidId userId,
        DateTime lastRequestTime)
    {
        if (lastRequestTime > DateTime.UtcNow)
            return Result.Fail("last request time cannot be in the future!");
        
        return Result.Ok(new UserRequest(userId, lastRequestTime));
    }
}