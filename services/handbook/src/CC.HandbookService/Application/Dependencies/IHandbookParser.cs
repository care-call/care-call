using CC.HandbookService.Domain.Handbooks;
using FluentResults;

namespace CC.HandbookService.Application.Dependencies;

public interface IHandbookParser
{
    public Task<Result<IEnumerable<T>>> ParseAsync<T>(Stream stream) where T : HandbookItem;
}