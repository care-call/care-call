using FluentResults;

namespace CC.HandbookService.Application.Dependencies.Handbook;

public interface IHandbookRegistry
{
    public Func<Stream, Task<Result>> GetHandbookLoader(HandbookType handbookType);
}