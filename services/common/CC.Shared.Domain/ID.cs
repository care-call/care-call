namespace CC.Shared.Domain;

public interface ID<TSelf, TPrimitive>
{
    static abstract TSelf From(TPrimitive value);
    static abstract bool TryFrom(TPrimitive value, out TSelf vo);
    
    TPrimitive Value { get; }
}