namespace CC.AppointmentService.Domain.Reviews.Repositories;

public interface ISessionReviewRepository
{
    Task AddAsync(SessionReview sessionReview);
    Task<SessionReview?> GetByIdAsync(Guid id);
    Task<SessionReview?> GetByAppointmentIdAsync(Guid appointmentId);
    Task<IReadOnlyCollection<SessionReview>> GetPendingAsync(CancellationToken ct);
}
