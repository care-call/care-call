using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Errors;
using CC.AppointmentService.Domain.Feedbacks;
using CC.AppointmentService.Domain.Feedbacks.Repositories;
using CC.AppointmentService.Domain.Feedbacks.Rules;
using FluentResults;

namespace CC.AppointmentService.Application.UseCases.Feedbacks.Creation;

public sealed record CreateFeedback
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
    IFeedbackRepository feedbackRepository,
    TimeProvider timeProvider)
{
    public async ValueTask<Result> Handle(CreateFeedback command, CancellationToken cancellationToken)
    {

        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null || appointment.ClientId != command.ClientId) 
            return Result.Fail(AppointmentErrors.NotFound());

        if (appointment.Status != AppointmentStatus.Completed)
            return Result.Fail(FeedbackError.AppointmentNotCompleted);

        var feedbackExists = await feedbackRepository.ExistsByAppointmentIdAsync(command.AppointmentId);
        if (feedbackExists)
            return Result.Fail(FeedbackError.AlreadyExists);

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
        if (!FeedbackCreationRules.IsWithinCreationWindow(appointment, nowUtc))
            return Result.Fail(FeedbackError.TooLate);

        var feedback = new Feedback(
            Guid.CreateVersion7(),
            appointmentId: command.AppointmentId,
            comfortScore: ComfortScore.From(command.ComfortScore),
            professionalismScore: ProfessionalismScore.From(command.ProfessionalismScore),
            empathyScore: EmpathyScore.From(command.EmpathyScore),
            nowUtc
        );
        
        if(command.Tags is not null)
            feedback.AddTags(command.Tags);
        
        await feedbackRepository.AddAsync(feedback);
        await unitOfWork.SaveAsync(cancellationToken);

        return Result.Ok();
    }
}
