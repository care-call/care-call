namespace CC.Shared.Domain;

public abstract class Entity<TId>(TId id)
{
    public TId Id { get; protected set; } = id;
}