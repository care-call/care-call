using System.Text.Json.Serialization;
using CC.HandbookService.Domain.Errors;
using CC.HandbookService.Domain.Handbooks;
using FluentResults;

namespace CC.HandbookService.Api.Binding;

public sealed record HandbookTypeParameter(HandbookType HandbookType) : IParsable<HandbookTypeParameter>
{
    [JsonIgnore]
    public Error? Error { get; private set; }
    
    public static HandbookTypeParameter Parse(
        string handbook, 
        IFormatProvider? provider)
    {
        if (Enum.TryParse<HandbookType>(handbook, true, out var result))
            return new HandbookTypeParameter(result);
        
        throw new Exception("Unknown handbook type: " + handbook);
    }

    public static bool TryParse(
        string? handbook, 
        IFormatProvider? provider, 
        out HandbookTypeParameter result)
    {
        if (Enum.TryParse<HandbookType>(handbook, true, out var value))
        {
            result = new HandbookTypeParameter(value);
            return true;
        }
        
        result = new HandbookTypeParameter(0)
        {
            Error = HandbookErrors.NotExist(handbook ?? string.Empty)
        };
            
        return true;
    }
}