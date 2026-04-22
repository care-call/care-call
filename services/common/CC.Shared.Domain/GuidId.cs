namespace CC.Shared.Domain;

public record GuidId : ID<GuidId?, Guid>
{
    private GuidId(Guid value) => Value = value;

    public static GuidId From(Guid value)
    {
        if (!TryFrom(value, out var result))
            throw new ArgumentException("Id cannot be empty");
            
            return result;
    }

    public static bool TryFrom(Guid value, out GuidId result)
    {
        if (value == Guid.Empty)
        {
            result = null;
            return false;
        }
        
        result = new GuidId(value);
        return true;
    }

    public Guid Value { get; }
}