namespace CC.AppointmentService.Domain.Reviews.Repositories;

public interface IReviewRepository
{
    Task AddAsync(Review review);
}