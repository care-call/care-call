namespace CC.PractitionerService.Domain.Practitioners;

public sealed record FullName(string Name, string Surname, string? Patronymic)
{
    public static implicit operator string(FullName fullName)
        => $"{fullName.Surname} {fullName.Name}"
        + (string.IsNullOrWhiteSpace(fullName.Surname) ? "" : " " + fullName.Patronymic);
}