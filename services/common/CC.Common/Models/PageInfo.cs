using System.ComponentModel.DataAnnotations;

namespace CC.Common.Models;

public readonly record struct PageInfo(
    [property: Range(1, 1000)] int Number,
    [property: Range(1, 50)] int Size)
{
    public PageInfo() : this(1, 10) { }
}