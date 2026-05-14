namespace CC.HandbookService.Domain.Handbooks;

/// <summary>
/// Справочная информация о целевой возрастной группе.
/// </summary>
using FluentResults;

public class AgeGroup : HandbookItem
{
    public required int FromAge { get; set; }
    public required int ToAge { get; set; }

    public override Result Validate()
    {
        return FromAge > ToAge
            ? Result.Fail("FromAge не может быть больше ToAge")
            : Result.Ok();
    }
}