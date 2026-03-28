using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Errors;
using CC.AppointmentService.Domain.Reviews.Errors;
using CC.AppointmentService.Domain.Reviews.Repositories;
using CC.AppointmentService.Domain.Reviews.Rules;
using CC.AppointmentService.Domain.Reviews.ValueObjects;
using FluentResults;
using Mediator;

namespace CC.AppointmentService.Application.UseCases.SessionReview.Creation;

public sealed record CreateSessionReview : IRequest<Result>
{
    public required Guid AppointmentId { get; init; }
    public required Guid UserId { get; init; }
    public required EmpathyRating EmpathyRating { get; init; }
    public required ComfortRating ComfortRating { get; init; }
    public required ProfessionalismRating ProfessionalismRating { get; init; }
    public required ReviewComment ReviewComment { get; init; }
}

public class CreateSessionReviewUseCase(
    IUnitOfWork uow,
    IAppointmentsRepository appointmentRepository,
    ISessionReviewRepository sessionReviewRepository
) : IRequestHandler<CreateSessionReview, Result>
{
    public async ValueTask<Result> Handle(CreateSessionReview request, CancellationToken cancellationToken)
    {
        var appointment = await appointmentRepository.GetByIdAsync(request.AppointmentId);

        if (appointment == null || appointment.ClientId != request.UserId)
            return Result.Fail(AppointmentErrors.NotFound());

        var validateReview = SessionReviewRules.IsStatusCreatable(appointment);

        if (!validateReview)
            return Result.Fail(SessionReviewError.NotCompletedSessionError);
        
        validateReview = SessionReviewRules.IsWithinAllowedReviewing(appointment);
        
        if (!validateReview)
            return Result.Fail(SessionReviewError.TooLateReview);
        
        var findExistingAppointment = await sessionReviewRepository.GetByAppointmentIdAsync(request.AppointmentId);

        if (findExistingAppointment != null)
            return Result.Fail(SessionReviewError.ReviewAlreadyExists);
        
        var review = new Domain.Reviews.SessionReview(Guid.CreateVersion7())
        {
            AppointmentId = appointment.Id,
            EmpathyRating = request.EmpathyRating,
            ProfessionalismRating = request.ProfessionalismRating,
            ComfortRating = request.ComfortRating,
            ReviewComment = request.ReviewComment,
        };
        
        await sessionReviewRepository.AddAsync(review);
        
        await uow.SaveAsync(cancellationToken);
        
        return Result.Ok();
    }
}