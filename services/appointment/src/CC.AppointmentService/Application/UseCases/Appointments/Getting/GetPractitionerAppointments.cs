using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Application.Dependencies;
using CC.Shared.Domain;
using FluentResults;
using Mediator;

namespace CC.AppointmentService.Application.UseCases.Appointments.Getting;

public sealed record GetPractitionerAppointments : IRequest<Result<AppointmentListItem[]>>
{
    public Guid PractitionerId { get; init; }
    public PractitionerAppointmentsDateFilter DateFilter { get; init; }
    public PractitionerAppointmentsStateFilter StateFilter { get; init; }
    public PractitionerAppointmentOrderFilter OrderFilter { get; init; }
}

public sealed record AppointmentListItem
{
    public DateTime StartedAt { get; init; }
    public required FullName ClientFullName { get; init; }
    public AppointmentStatus Status { get; init; }
}

public sealed class GetPractitionerAppointmentsUseCase(
    IPractitionerAppointmentsQuery appointmentsQuery) :
    IRequestHandler<GetPractitionerAppointments, Result<AppointmentListItem[]>>
{
    public async ValueTask<Result<AppointmentListItem[]>> Handle(
        GetPractitionerAppointments request, 
        CancellationToken cancellationToken)
    {
        return Result.Ok(await appointmentsQuery.GetAsync(request, cancellationToken));
    }
}