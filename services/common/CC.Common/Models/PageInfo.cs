using System.ComponentModel.DataAnnotations;

namespace CC.Common.Models;

public readonly record struct PageInfo(
    [property: Range(1, 1000, ErrorMessage = "Номер страницы должен быть в диапазоне от 1 до 1000")] int Number,
    [property: Range(1, 50, ErrorMessage = "Размер страницы должен быть в диапазоне от 1 до 50")] int Size)
{
    public PageInfo() : this(1, 10) { }

    public int Skip => checked((Number - 1) * Size);
}