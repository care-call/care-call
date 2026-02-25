using System.Text.Json.Nodes;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CC.PractitionerService.Infrastructure.OpenApi;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class OpenApiExampleAttribute : Attribute
{
    public OpenApiExampleAttribute(object value) => Value = value;
    public object Value { get; set; }
}