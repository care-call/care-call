namespace CC.PractitionerService.Infrastructure.OpenApi;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class OpenApiExample : Attribute
{
    public OpenApiExample(object value) => Value = value;
    public object Value { get; set; }
}