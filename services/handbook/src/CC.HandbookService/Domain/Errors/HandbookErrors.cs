using FluentResults;

namespace CC.HandbookService.Domain.Errors;

public static class HandbookErrors
{
    public static Error NotExist(string handbookTitle) => 
        new ($"Справочник «{handbookTitle}» не существует");

    public static Error MissingField(string cellName)
        => new ($"Пропущенно поле в ячейке {cellName}");

    public static Error EmptyField(string cellName)
        => new ($"Не заполнено обязательное поле в ячейке {cellName}");

    public static Error Unexpected()
        => new ("Непредвиденная ошибка");
}