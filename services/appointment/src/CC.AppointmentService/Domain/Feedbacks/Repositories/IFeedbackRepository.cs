using CC.AppointmentService.Domain.Feedbacks;

namespace CC.AppointmentService.Domain.Feedbacks.Repositories;

public interface IFeedbackRepository
{
    Task AddAsync(Feedback feedback);
    Task<bool> ExistsByAppointmentIdAsync(Guid appointmentId);
}
