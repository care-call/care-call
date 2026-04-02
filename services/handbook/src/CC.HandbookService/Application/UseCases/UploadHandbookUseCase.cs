using CC.HandbookService.Application.Handbook;
using FluentResults;
using Mediator;

namespace CC.HandbookService.Application.UseCases;

public sealed record UploadHandbook : IRequest<Result>
{
    public required string HandbookTitle { get; init; }
    public required Stream HandbookFileStream { get; init; }
}

public class UploadHandbookUseCase(
    IHandbookRegistry registry,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UploadHandbook, Result>
{
    public async ValueTask<Result> Handle(UploadHandbook command, CancellationToken cancellationToken)
    {
        var handbookLoader = registry.GetHandbookLoader(command.HandbookTitle);
        if (handbookLoader == null)
            return Result.Fail($"Справочник «{command.HandbookTitle}» не существует");
        
        var result = await handbookLoader.Invoke(command.HandbookFileStream);
        if (!result.IsSuccess)
            return Result.Fail(result.Errors);

        await unitOfWork.SaveAsync(cancellationToken);
        
        return Result.Ok();
    }
}