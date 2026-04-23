namespace CC.Shared.Domain;

public class LongId : ID<LongId, long>
{
    private LongId(long value) => Value = value;
    
    public long Value { get; }
    
    public static LongId From(long value)
    {
        if (!TryFrom(value, out var result))
            throw new ArgumentException("Id cannot be empty");

        return result;
    }

    public static bool TryFrom(long value, out LongId vo)
    {
        if (value < 0)
        {
            vo = null;
            return false;
        }

        vo = new LongId(value);
        return true;
    }
}