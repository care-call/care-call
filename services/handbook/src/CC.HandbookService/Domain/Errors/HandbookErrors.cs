using FluentResults;

namespace CC.HandbookService.Domain.Errors;

public static class HandbookErrors
{
    public static Error NotExist(string handbookTitle) =>
        new($"Справочник «{handbookTitle}» не существует");
}