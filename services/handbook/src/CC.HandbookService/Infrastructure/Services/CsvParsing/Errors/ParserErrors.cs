using FluentResults;

namespace CC.HandbookService.Infrastructure.Services.CsvParsing.Errors;

public static class ParserErrors
{
    public static Error MissingField(string cellName)
        => new ($"Пропущенно поле в ячейке {cellName}");

    public static Error EmptyField(string cellName)
        => new ($"Не заполнено обязательное поле в ячейке {cellName}");
}