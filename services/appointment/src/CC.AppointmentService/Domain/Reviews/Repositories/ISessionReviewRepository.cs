using CC.AppointmentService.Domain.Appointments;

namespace CC.AppointmentService.Domain.Reviews.Repositories;

public interface ISessionReviewRepository
{
    Task AddAsync(SessionReview sessionReview);
    Task<SessionReview?> GetByIdAsync(Guid id);
}