using CC.Common.Models;

namespace CC.AppointmentService.Application.Dependencies.AppointmentsQuery;

public sealed record ClientAppointmentsQuery(Guid ClientId, DateTime? From, DateTime? To, PageInfo PageInfo);