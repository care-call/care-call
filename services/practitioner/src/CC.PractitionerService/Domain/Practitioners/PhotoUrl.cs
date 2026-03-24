namespace CC.PractitionerService.Domain.Practitioners;

public sealed record PhotoUrl
{
    public PhotoUrl(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }
    public string Value { get; private set; }

    public static implicit operator string?(PhotoUrl photoUrl) => photoUrl?.Value;
}