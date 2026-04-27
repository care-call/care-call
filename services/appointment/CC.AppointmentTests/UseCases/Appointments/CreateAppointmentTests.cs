using CC.AppointmentService.Application.Dependencies.BackgroundJobs;
using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Application.UseCases.Appointments.Creation;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.Shared.Domain;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using NSubstitute;
using Shouldly;

namespace CC.AppointmentTests.UseCases.Appointments;

public class CreateAppointmentUseCaseTests
{
    private readonly IAppointmentsRepository _appointmentsRepository = Substitute.For<IAppointmentsRepository>();
    private readonly IAppointmentBackgroundTasks _backgroundTasks = Substitute.For<IAppointmentBackgroundTasks>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    private readonly DateTime _now = new(2026, 04, 06, 10, 0, 0, DateTimeKind.Utc);
    private readonly Guid _appointmentId = Guid.NewGuid();
    private readonly Guid _clientId = Guid.NewGuid();
    
    private CreateAppointment DefaultCommand => new()
    {
        ClientId = _clientId,
        PractitionerId = Guid.NewGuid(),
        TimeSlot = new DateTimeRange(_now.AddDays(1), _now.AddDays(1).AddHours(1)),   
        ClientSnapshot = new() { FullName = new FullName("1", "2", "3") },
        PractitionerSnapshot = new() { FullName = new FullName("1", "2", "3") }
    };
    
    private ValueTask<Result> Act(CreateAppointment? command = null) =>
        CreateAppointmentUseCase.Handle(
            command ?? DefaultCommand,
            _unitOfWork, _appointmentsRepository, _backgroundTasks,
            _now, CancellationToken.None);

    [Fact]
    public async Task Appointment_create_with_insufficient_break_is_rejected()
    {
        var lastAppointmentTimeSlot =
            new DateTimeRange(
                DefaultCommand.TimeSlot.From.Add(-AppointmentPolicy.Default.MinBreakBetweenAppointments).AddSeconds(1)
                    - (DefaultCommand.TimeSlot.To - DefaultCommand.TimeSlot.From),
                DefaultCommand.TimeSlot.From.Add(-AppointmentPolicy.Default.MinBreakBetweenAppointments).AddSeconds(1));
        
        var appointmentWithInsufficientBreak = AppointmentBuilder
            .Create(_appointmentId, _clientId)
            .WithTimeSlot(lastAppointmentTimeSlot)
            .Build();
        
        _appointmentsRepository.GetLastAppointmentAsync(_clientId).Returns(appointmentWithInsufficientBreak);

        var result = await Act();
        
        result.ShouldHaveErrorCode("A103");
        await _unitOfWork.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Appointment_create_with_intercepted_timeslots_is_rejected()
    {
        _appointmentsRepository.GetLastAppointmentAsync(_clientId).Returns((Appointment?)null);
        _appointmentsRepository.HasInterceptsAsync(Arg.Any<Appointment>()).Returns(true);

        var result = await Act();
        
        result.ShouldHaveErrorCode("A201");
        await _unitOfWork.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Appointment_create_with_exact_minimum_break_is_successful()
    {
        var lastAppointmentTimeSlot =
            new DateTimeRange(
                DefaultCommand.TimeSlot.From.Add(-AppointmentPolicy.Default.MinBreakBetweenAppointments) 
                - (DefaultCommand.TimeSlot.To - DefaultCommand.TimeSlot.From),
                DefaultCommand.TimeSlot.From.Add(-AppointmentPolicy.Default.MinBreakBetweenAppointments));

        var lastAppointment = AppointmentBuilder
            .Create(_appointmentId, _clientId)
            .WithTimeSlot(lastAppointmentTimeSlot)
            .Build();

        _appointmentsRepository.GetLastAppointmentAsync(_clientId).Returns(lastAppointment);

        var result = await Act();

        result.IsSuccess.ShouldBeTrue();
        await _unitOfWork.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        await _backgroundTasks.Received(1).ScheduleCallCreationAsync(Arg.Any<Appointment>());
    }

    [Fact]
    public async Task Valid_appointment_create_is_successful()
    {
        _appointmentsRepository.GetLastAppointmentAsync(_clientId).Returns((Appointment?)null);
        _appointmentsRepository.HasInterceptsAsync(Arg.Any<Appointment>()).Returns(false);
        
        var result = await Act();
        
        result.IsSuccess.ShouldBeTrue();
        await _unitOfWork.Received(1).SaveAsync(Arg.Any<CancellationToken>());
        await _backgroundTasks.Received(1).ScheduleCallCreationAsync(Arg.Any<Appointment>());
    }
} 