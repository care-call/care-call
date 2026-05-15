namespace CC.Shared.Domain;

public interface IDomainEventSource
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}

public abstract class AggregationRoot<TId>(TId id) : Entity<TId>(id), IDomainEventSource
{
    private readonly List<IDomainEvent> _domainEvents = [];

    IReadOnlyList<IDomainEvent> IDomainEventSource.DomainEvents => _domainEvents; 
    
    public void ClearDomainEvents()
        => _domainEvents.Clear();
    
    protected void AddDomainEvent(IDomainEvent @event)
        => _domainEvents.Add(@event);
}