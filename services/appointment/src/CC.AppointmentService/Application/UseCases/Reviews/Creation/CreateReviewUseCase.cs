using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Errors;
using CC.AppointmentService.Domain.Reviews;
using CC.AppointmentService.Domain.Reviews.Repositories;
using FluentResults;
using Mediator;

namespace CC.AppointmentService.Application.UseCases.Reviews.Creation;

public sealed record CreateReview : IRequest<Result>
{
    public required Guid AppointmentId { get; init; }
    public required byte ComfortScore { get; init; }
    public required byte ProfessionalismScore { get; init; }
    public required byte EmpathyScore { get; init; }
    public required List<Guid> Tags { get; init; }
}

public sealed class CreateReviewUseCase(
    IUnitOfWork unitOfWork,
    IAppointmentsRepository appointmentsRepository,
    IReviewRepository reviewRepository,
    TimeProvider timeProvider) : IRequestHandler<CreateReview, Result>
{
    public async ValueTask<Result> Handle(CreateReview command, CancellationToken cancellationToken)
    {
        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null) 
            return Result.Fail(AppointmentErrors.NotFound());
        
        if (appointment.EndedAt.HasValue && appointment.EndedAt.Value < timeProvider.GetUtcNow().AddDays(2))
            return Result.Fail(ReviewError.TooLate);
        
        var review = new Review(Guid.CreateVersion7())
        {
            AppointmentId = command.AppointmentId,
            ComfortScore = ComfortScore.From(command.ComfortScore),
            ProfessionalismScore = ProfessionalismScore.From(command.ProfessionalismScore),
            EmpathyScore = EmpathyScore.From(command.EmpathyScore),
            CreatedAt = timeProvider.GetUtcNow().DateTime
        };

        review.AddTags(command.Tags);
        await reviewRepository.AddAsync(review);
        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}