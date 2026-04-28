using FluentResults;

namespace CC.HandbookService.Application.Dependencies;

public interface IHandbookLoader
{
    public Task<Result> LoadAsync(Stream stream);
}