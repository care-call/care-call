using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Errors;
using CC.AppointmentService.Domain.Feedbacks;
using CC.AppointmentService.Domain.Feedbacks.Repositories;
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

public static class CreateFeedbackUseCase
{
    public static async ValueTask<Result> Handle(
        CreateFeedback command,
        IUnitOfWork unitOfWork,
        IAppointmentsRepository appointmentsRepository,
        IFeedbackRepository feedbackRepository,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var appointment = await appointmentsRepository.GetByIdAsync(command.AppointmentId);
        if (appointment is null || appointment.ClientId != command.ClientId)
            return Result.Fail(AppointmentErrors.NotFound());

        var feedbackExists = await feedbackRepository.ExistsByAppointmentIdAsync(command.AppointmentId);
        if (feedbackExists)
            return Result.Fail(FeedbackError.AlreadyExists);

        var feedbackResult = Feedback.Create(
            appointment,
            ComfortScore.From(command.ComfortScore),
            ProfessionalismScore.From(command.ProfessionalismScore),
            EmpathyScore.From(command.EmpathyScore),
            now,
            command.Tags);

        if (feedbackResult.IsFailed)
            return feedbackResult.ToResult();

        await feedbackRepository.AddAsync(feedbackResult.Value);
        await unitOfWork.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
