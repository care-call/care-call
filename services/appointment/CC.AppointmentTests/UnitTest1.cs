using CC.AppointmentService.Api.Contracts.Reviews;
using CC.AppointmentService.Application.UseCases.Reviews.Creation;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.Shared.Domain.TimeRanges;

namespace CC.AppointmentTests;

public class UnitTest1
{
    [Fact]
    public async Task ShouldFailWhenEndAtIsExceedTwoDays()
    {
        var useCase = new CreateReviewUseCase(null, new FakeRepository(new(2026, 03, 29)), null, new FakeTimeProvider());
        
        var result = await useCase.Handle(new CreateReview()
        {
            AppointmentId = Guid.Empty,
            ComfortScore = 5,
            EmpathyScore = 5,
            ProfessionalismScore = 5,
            Tags = []
        },default);
        
        Assert.Contains("Вы можете оставить отзыв только в течении двух суток", 
            result.Errors.Select(err => err.Message));
        Assert.False(result.IsSuccess);
    }

    public class FakeTimeProvider : TimeProvider
    {
        public override DateTimeOffset GetUtcNow()
        {
            return new DateTimeOffset(new DateTime(2026, 04, 01));
        }
    }

    public class FakeRepository(DateTime fakeCompleteTime) : IAppointmentsRepository
    {
        public Task AddAsync(Appointment appointment)
        {
            return Task.CompletedTask;
        }

        public async Task<Appointment?> GetByIdAsync(Guid id)
        {
            return new Appointment(Guid.Empty)
            {
                ClientId = id,
                CallUrl = default,
                ClientSnapshot = null,
                PractitionerId = default,
                PractitionerSnapshot = null,
                Status = default,
                TimeSlot = new DateTimeRange(),
                EndedAt = fakeCompleteTime
            };

        }

        public async Task<bool> HasInterceptsAsync(Appointment appointment)
        {
            return true;
        }

        public Task<Appointment?> GetLastAppointmentAsync(Guid clientId)
        {
            return Task.FromResult(default(Appointment));
        }
    }
}