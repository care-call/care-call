using CC.Shared.Domain;
using CC.TechSupportService.Domain.Enums;
using FluentResults;

namespace CC.TechSupportService.Domain.Entities;

public sealed class UserRequest : Entity<long>
{
    private UserRequest() : base(0) { }

    private UserRequest(Guid userId, DateTime lastRequestTime) : base(0)
    {
        UserId = userId;
        AmountOfRequestForLastHour = 0;
        LastRequestTime = lastRequestTime;
    }
    
    public Guid UserId { get; private set; }
    
    public byte AmountOfRequestForLastHour { get; private set; }
    
    public DateTime LastRequestTime { get; private set; }

    public static Result<UserRequest> TryCreate(Guid userId,
        DateTime lastRequestTime)
    {
        if (lastRequestTime > DateTime.UtcNow)
            return Result.Fail(TechServiceErrors.PassingLastRequestTimeFromFuture);
        
        return Result.Ok(new UserRequest(userId, lastRequestTime));
    }
}