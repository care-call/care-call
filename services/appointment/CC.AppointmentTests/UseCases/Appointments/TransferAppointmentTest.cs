using CC.AppointmentService.Application.Dependencies.UnitOfWork;
using CC.AppointmentService.Application.UseCases.Appointments.Transferring;
using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Appointments.Repositories;
using CC.Shared.Domain.TimeRanges;
using FluentResults;
using NSubstitute;
using Shouldly;

namespace CC.AppointmentTests.UseCases.Appointments;

public class TransferAppointmentUseCaseTest
{
    private readonly IAppointmentsRepository _appointmentsRepository = Substitute.For<IAppointmentsRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    
    private readonly DateTime _now = new(2026, 04, 06, 10, 0, 0, DateTimeKind.Utc);
    private readonly Guid _clientId = Guid.NewGuid();
    private readonly Guid _appointmentId = Guid.NewGuid();
    
    private TransferAppointment DefaultTransferCommand => new()
    {
        AppointmentId = _appointmentId,
        ClientId = _clientId,
        TimeSlot = new DateTimeRange(new DateTime(2026, 04, 13, 10, 0, 0, DateTimeKind.Utc), 
            new DateTime(2026, 04, 13, 10, 30, 0, DateTimeKind.Utc)),
    };
    
    private Appointment MakeAppointment(
        Guid? clientId = null, 
        AppointmentStatus? status = null, 
        DateTimeRange? period = null,
        string? cancellationReason = null)
    {
        var builder = AppointmentBuilder.Create(_appointmentId, clientId ?? _clientId)
            .WithStatus(status ?? AppointmentStatus.Planned);
        
        if (period.HasValue)
            builder.WithTimeSlot(period.Value);

        var appointment = builder.Build();
        
        if (status == AppointmentStatus.Completed)
            appointment.Complete(_now.AddDays(-1));
        
        if (status == AppointmentStatus.Cancelled)
        {
            var reasonText = !string.IsNullOrWhiteSpace(cancellationReason) 
                ? cancellationReason 
                : "Default Test Reason";

            var reason = CancellationReason.From(reasonText); 
            appointment.Cancel(reason, _now.AddDays(-1)); 
        }
        
        return appointment;
    }
    
    private ValueTask<Result> Act(TransferAppointment? command = null) =>
        TransferAppointmentUseCase.Handle(
            command ?? DefaultTransferCommand, 
            _appointmentsRepository, _unitOfWork,
            _now, CancellationToken.None);

    [Fact]
    public async Task Appointment_transferring_for_nonexistent_appointment_is_rejected()
    {
        _appointmentsRepository.GetByIdAsync(_appointmentId).Returns((Appointment?)null);

        var result = await Act();
        
        result.ShouldHaveErrorCode("A202");
        await _unitOfWork.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Appointment_transferring_for_another_clients_appointment_is_rejected()
    {
        _appointmentsRepository.GetByIdAsync(_appointmentId).Returns(MakeAppointment(clientId: Guid.NewGuid()));
        
        var result = await Act();
        
        result.ShouldHaveErrorCode("A202");
        await _unitOfWork.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(AppointmentStatus.Completed, null)]       
    [InlineData(AppointmentStatus.Cancelled, "Client refused")]
    public async Task Appointment_transferring_for_unplanned_appointment_is_rejected(AppointmentStatus status, 
        string? cancellationReason)
    {
        _appointmentsRepository.GetByIdAsync(_appointmentId).Returns(MakeAppointment(status: status,
            cancellationReason: cancellationReason));
        
        var result = await Act();
        
        result.ShouldHaveErrorCode("A203");
        await _unitOfWork.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Appointment_transferring_with_insufficient_break_is_rejected()
    {
        var currAppointmentTimeSlot = new DateTimeRange(_now.AddDays(1), _now.AddDays(1).AddHours(1));
        _appointmentsRepository.GetByIdAsync(_appointmentId).Returns(MakeAppointment(period: currAppointmentTimeSlot));
        
        var newTimeSlot = new DateTimeRange(_now.AddDays(2), _now.AddDays(2).AddHours(1));
        var lastAppointmentTimeSlot = new DateTimeRange(
            newTimeSlot.From.Add(-AppointmentPolicy.Default.MinBreakBetweenAppointments).AddSeconds(1)
            - (newTimeSlot.To - newTimeSlot.From),
            newTimeSlot.From.Add(-AppointmentPolicy.Default.MinBreakBetweenAppointments).AddSeconds(1));
        
        _appointmentsRepository.GetLastAppointmentAsync(_clientId).Returns(MakeAppointment(period: lastAppointmentTimeSlot));
        
        var result = await Act();
        
        result.ShouldHaveErrorCode("A103");
        await _unitOfWork.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Transferring_is_rejected_when_min_lead_time_is_violated()
    {
        var existingPeriod = new DateTimeRange(_now.AddDays(1), _now.AddDays(1).AddMinutes(30));
        _appointmentsRepository.GetByIdAsync(_appointmentId)
            .Returns(MakeAppointment(period: existingPeriod));

        var result = await Act(new TransferAppointment
        {
            AppointmentId = _appointmentId,
            TimeSlot = new DateTimeRange(_now.AddHours(2), _now.AddHours(2).AddMinutes(30)),
            ClientId = _clientId,
        });
    
        result.ShouldHaveErrorCode("A101");
        await _unitOfWork.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Transferring_is_rejected_when_max_shift_exceeded()
    {
        var pastPeriod = new DateTimeRange(_now.AddDays(-5), _now.AddDays(-5).AddMinutes(30));
        _appointmentsRepository.GetByIdAsync(_appointmentId)
            .Returns(MakeAppointment(period: pastPeriod));
    
        var result = await Act(); 
    
        result.ShouldHaveErrorCode("A102");
        await _unitOfWork.DidNotReceive().SaveAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Valid_appointment_transferring_is_successful()
    {
        var existingPeriod = new DateTimeRange(_now.AddDays(1), _now.AddDays(1).AddMinutes(30));
        _appointmentsRepository.GetByIdAsync(_appointmentId)
            .Returns(MakeAppointment(period: existingPeriod));
        
        var validSlot = new DateTimeRange(_now.AddDays(3), _now.AddDays(3).AddMinutes(30));
    
        var command = new TransferAppointment
        {
            AppointmentId = _appointmentId,
            ClientId = _clientId,
            TimeSlot = validSlot
        };

        var result = await Act(command);

        result.IsSuccess.ShouldBeTrue();
        await _unitOfWork.Received(1).SaveAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Transferring_is_successful_when_min_interval_is_exactly_met()
    {
        var planedPeriod = new DateTimeRange(_now.AddDays(4), _now.AddDays(4).AddMinutes(30));
        _appointmentsRepository.GetByIdAsync(_appointmentId)
            .Returns(MakeAppointment(period: planedPeriod));
  
        var result = await Act(new TransferAppointment()
        {
            AppointmentId =  _appointmentId,
            ClientId = _clientId,
            TimeSlot = new DateTimeRange(_now.AddDays(6), _now.AddDays(6).AddMinutes(30)),
        });

        result.IsSuccess.ShouldBeTrue();
        await _unitOfWork.Received(1).SaveAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task Transferring_is_successful_when_max_shift_is_exactly_met()
    {
        var planedPeriod = new DateTimeRange(_now.AddDays(2), _now.AddDays(2).AddMinutes(30));
        _appointmentsRepository.GetByIdAsync(_appointmentId).Returns(MakeAppointment(period: planedPeriod));
        
        var result = await Act();

        result.IsSuccess.ShouldBeTrue();
        await _unitOfWork.Received(1).SaveAsync(Arg.Any<CancellationToken>());
    }
}
