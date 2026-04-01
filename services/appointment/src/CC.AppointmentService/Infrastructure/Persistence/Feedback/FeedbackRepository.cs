using CC.AppointmentService.Domain.Feedback;
using CC.AppointmentService.Domain.Feedback.Repositories;

namespace CC.AppointmentService.Infrastructure.Persistence.Feedback;

internal sealed class FeedbackRepository(DatabaseContext db) : IReviewRepository
{
    public async Task AddAsync(Review review) => await db.Reviews.AddAsync(review).AsTask();
}