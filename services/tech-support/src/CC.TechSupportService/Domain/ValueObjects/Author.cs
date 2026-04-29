using CC.TechSupportService.Domain.Enums;

namespace CC.TechSupportService.Domain.ValueObjects;

public record Author
{
    public Author(Guid id, AuthorType type)
    {
        Id = id;
        Type = type;
    }

    public Guid Id { get; private set; }
    
    public AuthorType Type { get; private set; }
}