using CC.AppointmentService.Domain.Feedbacks;
using CC.AppointmentService.Domain.Feedbacks.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CC.AppointmentService.Infrastructure.Persistence.Feedbacks;

internal sealed class FeedbackRepository(DatabaseContext db) : IFeedbackRepository
{
    public async Task AddAsync(Feedback feedback) =>
        await db.Feedbacks.AddAsync(feedback);

    public Task<bool> ExistsByAppointmentIdAsync(Guid appointmentId) 
        => db.Feedbacks.AnyAsync(feedback => feedback.AppointmentId == appointmentId);
}