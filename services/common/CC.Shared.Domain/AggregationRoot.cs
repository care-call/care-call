namespace CC.Shared.Domain;

public abstract class AggregationRoot<TId>(TId id) : Entity<TId>(id);