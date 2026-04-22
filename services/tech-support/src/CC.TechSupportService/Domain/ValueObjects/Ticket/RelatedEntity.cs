using CC.TechSupportService.Domain.Enums;
using FluentResults;

namespace CC.TechSupportService.Domain.ValueObjects.Ticket;

public record RelatedEntity
{
    private RelatedEntity(Guid relatedEntityId, RelatedEntityType relatedType)
    {
        RelatedEntityId = relatedEntityId;
        RelatedType = relatedType;
    }

    public static Result<RelatedEntity> TryCreate(Guid relatedEntityId, RelatedEntityType relatedType)
    {
        if (relatedEntityId == Guid.Empty)
            return Result.Fail("RelatedEntityId cannot be empty");
        
        return Result.Ok(new RelatedEntity(relatedEntityId, relatedType));
    }

    public Guid RelatedEntityId { get; private set; }
    
    public RelatedEntityType RelatedType { get; private set; }
}