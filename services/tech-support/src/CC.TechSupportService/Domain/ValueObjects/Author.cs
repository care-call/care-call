using CC.Shared.Domain;
using CC.TechSupportService.Domain.Enums;

namespace CC.TechSupportService.Domain.ValueObjects;

public record Author
{
    public Author(GuidId id, AuthorType type)
    {
        Id = id;
        Type = type;
    }

    public GuidId Id { get; private set; }
    
    public AuthorType Type { get; private set; }
}