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
    
    public Guid RelatedEntityId { get; private set; }
    public RelatedEntityType RelatedType { get; private set; }
    
    public static Result<RelatedEntity> TryCreate(Guid relatedEntityId, RelatedEntityType relatedType)
        => Result.Ok(new RelatedEntity(relatedEntityId, relatedType));
}