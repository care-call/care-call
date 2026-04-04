using CC.HandbookService.Domain.Handbooks;
using FluentResults;

namespace CC.HandbookService.Application.Handbook;

public interface IHandbookLoader
{
    public Task<Result> LoadAsync<T>(Stream stream) where T : HandbookItem;
}