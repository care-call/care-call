using CC.TechSupportService.API.Contracts.Enums;
using CC.TechSupportService.Domain.Enums;

namespace CC.TechSupportService.API.Contracts.Requests;

public sealed record CreateTicketRequest(string TicketSubject, TicketCategoryType Category,
    string TicketDescription, int TicketNumber, Guid ReporterId, ReporterType ReporterType);