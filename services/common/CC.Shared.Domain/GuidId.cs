using Vogen;

namespace CC.Shared.Domain;

[ValueObject<Guid>]
public partial record GuidId
{
    private static Validation Validate(Guid value)
    {
        if (value == Guid.Empty)
            return Validation.Invalid();

        return Validation.Ok;
    }
}