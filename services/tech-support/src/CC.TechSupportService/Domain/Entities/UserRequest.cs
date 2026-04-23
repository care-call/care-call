using CC.Shared.Domain;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public class UserRequest : Entity<LongId>
{
    private UserRequest(LongId id) : base(id) { }

    private UserRequest(LongId id, GuidId userId, DateTime lastRequestTime) : base(id)
    {
        UserId = userId;
        AmountOfRequestForLastHour = 0;
        LastRequestTime = lastRequestTime;
    }
    
    public GuidId UserId { get; private set; }
    
    public byte AmountOfRequestForLastHour { get; private set; }
    
    public DateTime LastRequestTime { get; private set; }

    public static Result<UserRequest> TryCreate(LongId id,
        GuidId userId,
        DateTime lastRequestTime)
    {
        if (lastRequestTime > DateTime.UtcNow)
            return Result.Fail("last request time cannot be in the future!");
        
        return Result.Ok(new UserRequest(id, userId, lastRequestTime));
    }
}