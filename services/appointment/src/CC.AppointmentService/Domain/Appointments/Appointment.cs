using CC.AppointmentService.Domain.Errors;
using CC.Shared.Domain;
using CC.Shared.Domain.TimeRanges;
using FluentResults;

namespace CC.AppointmentService.Domain.Appointments;

public sealed class Appointment(Guid id) : AggregationRoot<Guid>(id)
{
    private static readonly AppointmentPolicy Policy = AppointmentPolicy.Default;

    public required Guid ClientId { get; init; }
    public required Guid PractitionerId { get; init; }
    public required DateTimeRange TimeSlot { get; set; }
    public required AppointmentStatus Status { get; set; }
    public required ClientSnapshot ClientSnapshot { get; init; }
    public required PractitionerSnapshot PractitionerSnapshot { get; init; }
    public Uri? CallUrl { get; set; }
    public string? CancellationReason { get; private set; }
    public DateTime? EndedAt { get; private set; }

    public static Result<Appointment> Create(
        Guid clientId,
        Guid practitionerId,
        DateTimeRange timeSlot,
        ClientSnapshot clientSnapshot,
        PractitionerSnapshot practitionerSnapshot,
        DateTime now)
    {
        if (timeSlot.From - now < Policy.MinLeadTime)
            return Result.Fail(AppointmentErrors.TooSoon);

        return new Appointment(Guid.CreateVersion7())
        {
            ClientId = clientId,
            PractitionerId = practitionerId,
            TimeSlot = timeSlot,
            Status = AppointmentStatus.Planned,
            ClientSnapshot = clientSnapshot,
            PractitionerSnapshot = practitionerSnapshot
        };
    }

    public Result Cancel(string reason, DateTime now)
    {
        if (Status != AppointmentStatus.Planned)
            return Result.Fail(AppointmentErrors.InvalidStatusForCancel());
        if (TimeSlot.From - now <= Policy.MinCancellationNotice)
            return Result.Fail(AppointmentErrors.CancellationDeadlineExceeded);

        CancellationReason = reason;
        Status = AppointmentStatus.Cancelled;
        return Result.Ok();
    }

    public Result Complete(DateTime endedAt)
    {
        if (Status is not AppointmentStatus.InProgress)
            return Result.Fail(AppointmentErrors.InvalidStatusForComplete());

        Status = AppointmentStatus.Completed;
        EndedAt = endedAt;
        return Result.Ok();
    }

    public Result Transfer(DateTimeRange newTimeSlot, DateTime now)
    {
        if (Status != AppointmentStatus.Planned)
            return Result.Fail(AppointmentErrors.InvalidStatusForTransfer());
        if ((newTimeSlot.From - TimeSlot.From).Duration() > Policy.MaxTransferShift)
            return Result.Fail(AppointmentErrors.NotWithinAllowedShift);
        if (newTimeSlot.From - now < Policy.MinLeadTime)
            return Result.Fail(AppointmentErrors.TooSoon);

        TimeSlot = newTimeSlot;
        return Result.Ok();
    }

    public bool HasInsufficientBreakAfter(Appointment previous) =>
        TimeSlot.From - previous.TimeSlot.From < Policy.MinBreakBetweenAppointments;
}
