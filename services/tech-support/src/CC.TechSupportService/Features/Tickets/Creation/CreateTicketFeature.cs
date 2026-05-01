using CC.TechSupportService.Domain.Entities;
using CC.TechSupportService.Domain.Enums;
using CC.TechSupportService.Domain.ValueObjects.Ticket;
using CC.TechSupportService.Features.Tickets.Enums;
using CC.TechSupportService.Features.Tickets.Extensions;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;
using Wolverine.Persistence;

namespace CC.TechSupportService.Features.Tickets.Creation;

public class CreateTicketFeature
{
    [ProducesResponseType<CreateTicketResponse>(200)]
    [WolverinePost("api/v1/tickets")]
    public static (IResult, Insert<Ticket>) Handle(CreateTicketRequest command)
    {
        var ticketResult = Ticket.TryCreate(
            Guid.CreateVersion7(),
            command.Number,
            Reporter.TryCreate(command.ReporterId, command.ReporterType).Value,
            TicketSubject.From(command.Subject),
            TicketDescription.From(command.Description),
            command.Category.ToTicketCategory(),
            null,
            DateTime.UtcNow);
        
        return (Results.Ok(new CreateTicketResponse(ticketResult.Value.Number)), new Insert<Ticket>(ticketResult.Value));
    }
}

public sealed record CreateTicketResponse(int TicketNumber);

public sealed record CreateTicketRequest(
    string Subject, 
    TicketCategoryType Category,
    string Description,
    int Number,
    Guid ReporterId,
    ReporterType ReporterType);