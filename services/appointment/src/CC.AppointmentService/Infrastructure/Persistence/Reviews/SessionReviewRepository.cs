using CC.AppointmentService.Domain.Reviews;
using CC.AppointmentService.Domain.Reviews.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CC.AppointmentService.Infrastructure.Persistence.Reviews;

public class SessionReviewRepository(DatabaseContext db) : ISessionReviewRepository
{
    public async Task AddAsync(SessionReview sessionReview) =>
        await db.SessionReviews.AddAsync(sessionReview);

    public async Task<SessionReview?> GetByIdAsync(Guid id) 
        => await db.SessionReviews.FirstOrDefaultAsync(s => s.Id == id);

    public async Task<SessionReview?> GetByAppointmentIdAsync(Guid appointmentId)
        => await db.SessionReviews.FirstOrDefaultAsync(u => u.AppointmentId == appointmentId);

    public async Task<IReadOnlyCollection<SessionReview>> GetPendingAsync(CancellationToken ct)
        => await db.SessionReviews
            .AsNoTracking()
            .Where(x => x.Status == SessionReviewStatus.Pending)
            .OrderBy(x => x.Id)
            .ToListAsync(ct);
}
