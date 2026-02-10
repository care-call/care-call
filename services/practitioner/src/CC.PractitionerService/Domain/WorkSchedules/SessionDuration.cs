namespace CC.PractitionerService.Domain.WorkSchedules;

public readonly record struct SessionDuration(TimeSpan Value)
{
    public static readonly SessionDuration Default = new(TimeSpan.FromMinutes(40));
    public TimeSpan Value { get; private init; } = Value;
}