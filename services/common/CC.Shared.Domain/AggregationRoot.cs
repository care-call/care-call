namespace CC.Shared.Domain;

public abstract class AggregationRoot<TId>(TId id) : Entity<TId>(id)
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    public void ClearDomainEvents()
        => _domainEvents.Clear();

    protected void AddDomainEvent(IDomainEvent @event)
        => _domainEvents.Add(@event);
}