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
    public Guid AppointmentId { get; init; }
    public Guid ClientId { get; init; }
    public byte ComfortScore { get; init; }
    public byte ProfessionalismScore { get; init; }
    public byte EmpathyScore { get; init; }
    public IReadOnlyCollection<Guid> Tags { get; init; }
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
        if (appointment is null || appointment.ClientId != command.ClientId) 
            return Result.Fail(AppointmentErrors.NotFound());

        if (appointment.EndedAt.HasValue && timeProvider.GetUtcNow().Date >= appointment.EndedAt.Value.Date.AddDays(2))
            return Result.Fail(ReviewError.TooLate);

        var review = new Review(
            Guid.CreateVersion7(),
            appointmentId: command.AppointmentId,
            comfortScore: ComfortScore.From(command.ComfortScore),
            professionalismScore: ProfessionalismScore.From(command.ProfessionalismScore),
            empathyScore: EmpathyScore.From(command.EmpathyScore),
            timeProvider.GetUtcNow().DateTime
        );

        review.AddTags(command.Tags);
        await reviewRepository.AddAsync(review);
        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}