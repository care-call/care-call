namespace CC.PractitionerService.Infrastructure.OpenApi;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class OpenApiExampleAttribute : Attribute
{
    public OpenApiExampleAttribute(object value) => Value = value;
    public object Value { get; set; }
}