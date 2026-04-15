using CC.Common.Models;

namespace CC.AppointmentService.Api.Contracts;

public sealed record GetClientHistoryRequest(PageInfo PageInfo, ClientHistoryFilterDto? Filters);