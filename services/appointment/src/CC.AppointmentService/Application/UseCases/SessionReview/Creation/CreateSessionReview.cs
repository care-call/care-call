using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.AppointmentService.Domain.Reviews;
using CC.AppointmentService.Domain.Reviews.Repositories;
using CC.AppointmentService.Domain.Reviews.Rules;
using CC.AppointmentService.Domain.Reviews.ValueObjects;
using FluentResults;
using Mediator;

namespace CC.AppointmentService.Application.UseCases;

public sealed record CreateSessionReview : IRequest<Result>
{
    public required Guid AppointmentId { get; init; }
    public required Guid UserId { get; init; }
    public required EmpathyRating EmpthyRating { get; init; }
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

        if (appointment == null)
            return Result.Fail("Не существует такой сессии");

        var validateReview = SessionReviewRules.CanCreate(appointment);

        if (!validateReview.IsSuccess)
            return validateReview;

        var review = new SessionReview(Guid.CreateVersion7())
        {
            AppointmentId = appointment.Id,
            EmpathyRating = request.EmpthyRating,
            ProfessionalismRating = request.ProfessionalismRating,
            ComfortRating = request.ComfortRating,
            ReviewComment = request.ReviewComment,
        };
        
        await sessionReviewRepository.AddAsync(review);
        
        await uow.SaveAsync(cancellationToken);
        
        return Result.Ok();
    }
}