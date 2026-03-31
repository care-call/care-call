using CC.AppointmentService.Domain.Reviews;
using CC.AppointmentService.Domain.Reviews.Repositories;

namespace CC.AppointmentService.Infrastructure.Persistence.Reviews;

internal sealed class ReviewRepository(DatabaseContext db) : IReviewRepository
{
    public async Task AddAsync(Review review) => await db.Reviews.AddAsync(review);
}