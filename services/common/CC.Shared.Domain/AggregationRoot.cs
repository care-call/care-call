namespace CC.Shared.Domain;

public abstract class AggregationRoot<TId>(TId id) : Entity<TId>(id)
{
    private List<IDomainEvent> _events { get; set; } = [];

    public IReadOnlyList<IDomainEvent> Events => _events; 
    
    public void ClearDomainEvents()
        => _events.Clear();
    
    protected void AddDomainEvent(IDomainEvent @event)
        => _events.Add(@event);
}