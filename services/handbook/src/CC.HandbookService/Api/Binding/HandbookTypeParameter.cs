using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;
using CC.HandbookService.Domain.Errors;
using CC.HandbookService.Domain.Handbooks;
using FluentResults;

namespace CC.HandbookService.Api.Contracts;

public sealed record HandbookTypeParameter(HandbookType HandbookType) : IParsable<HandbookTypeParameter>
{
    public Error? Error { get; private set; }
    
    public static HandbookTypeParameter Parse(
        string handbook, 
        IFormatProvider? provider)
    {
        if (Enum.TryParse<HandbookType>(handbook, true, out var result))
            return new HandbookTypeParameter(result);
        
        if (int.TryParse(handbook, out var handbookNumber))
            return new HandbookTypeParameter((HandbookType)handbookNumber);

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