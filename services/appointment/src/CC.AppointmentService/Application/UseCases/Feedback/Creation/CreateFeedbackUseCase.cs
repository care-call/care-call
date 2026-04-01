using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Errors;
using CC.AppointmentService.Domain.Feedback;
using CC.AppointmentService.Domain.Feedback.Repositories;
using FluentResults;
using Mediator;
using ComfortScore = CC.AppointmentService.Domain.Feedback.ComfortScore;
using EmpathyScore = CC.AppointmentService.Domain.Feedback.EmpathyScore;
using ProfessionalismScore = CC.AppointmentService.Domain.Feedback.ProfessionalismScore;

namespace CC.AppointmentService.Application.UseCases.Feedback.Creation;

public sealed record CreateReview : IRequest<Result>
{
    public Guid AppointmentId { get; init; }
    public Guid ClientId { get; init; }
    public byte ComfortScore { get; init; }
    public byte ProfessionalismScore { get; init; }
    public byte EmpathyScore { get; init; }
    public IReadOnlyCollection<Guid>? Tags { get; init; }
}

public sealed class CreateFeedbackUseCase(
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
        
        if(command.Tags is not null)
            review.AddTags(command.Tags);
        
        await reviewRepository.AddAsync(review);
        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}