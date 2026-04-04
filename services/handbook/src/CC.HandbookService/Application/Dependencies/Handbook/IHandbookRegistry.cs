using FluentResults;

namespace CC.HandbookService.Application.Handbook;

public interface IHandbookRegistry
{
    public Func<Stream, Task<Result>>? GetHandbookLoader(string handbookTitle);
}