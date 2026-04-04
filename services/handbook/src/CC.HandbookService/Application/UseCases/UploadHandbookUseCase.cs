using CC.HandbookService.Application.Dependencies.Handbook;
using FluentResults;
using Mediator;

namespace CC.HandbookService.Application.UseCases;

public sealed record UploadHandbook : IRequest<Result>
{
    public required HandbookType HandbookType { get; init; }
    public required Stream HandbookFileStream { get; init; }
}

public class UploadHandbookUseCase(
    IHandbookRegistry registry,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UploadHandbook, Result>
{
    public async ValueTask<Result> Handle(UploadHandbook command, CancellationToken cancellationToken)
    {
        var handbookLoader = registry.GetHandbookLoader(command.HandbookType);
        
        await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await handbookLoader.Invoke(command.HandbookFileStream);
            if (!result.IsSuccess)
                return Result.Fail(result.Errors);

            await unitOfWork.SaveAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Fail("Ошибка при загрузке справочника");
        }
    }
}