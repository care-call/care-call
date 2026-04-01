namespace CC.AppointmentService.Domain.Feedback.Repositories;

public interface IReviewRepository
{
    Task AddAsync(Review review);
}