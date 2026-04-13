using CC.HandbookService.Application.Dependencies;
using CC.HandbookService.Domain.Handbooks;
using FluentResults;

namespace CC.HandbookService.Application.UseCases;

public sealed record UploadHandbook
{
    public required HandbookType HandbookType { get; init; }
    public required Stream HandbookFileStream { get; init; }
}

public class UploadHandbookUseCase(
    IServiceProvider keyedProvider,
    IUnitOfWork unitOfWork)
{
    public async Task<Result> Handle(UploadHandbook command, CancellationToken cancellationToken)
    {
        var handbookLoader = keyedProvider.GetRequiredKeyedService<IHandbookLoader>(command.HandbookType);
        
        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        var result = await handbookLoader.LoadAsync(command.HandbookFileStream);
        if (!result.IsSuccess)
            return Result.Fail(result.Errors);

        await unitOfWork.SaveAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        
        return Result.Ok();
    }
}