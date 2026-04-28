using CC.TechSupportService.API.Contracts.Requests;
using CC.TechSupportService.API.Contracts.Responces;
using CC.TechSupportService.Application.Common.Extensions;
using CC.TechSupportService.Application.UseCases.Tickets.Creation;
using CC.TechSupportService.Domain.ValueObjects.Ticket;
using Wolverine;

namespace CC.TechSupportService.API.Endpoints.Tickets;

public static class CreateTicketEndpoint
{
    public static async Task<IResult> Handle(
        CreateTicketRequest request,
        IMessageBus bus)
    {
        var result = await bus.InvokeAsync<CreateTicketResponce>(new CreateTicket
        {
            Subject = TicketSubject.From(request.TicketSubject),
            Category = request.Category.ToDomain(),
            Description = TicketDescription.From(request.TicketDescription),
            Number = request.TicketNumber,
            ReporterId = request.ReporterId,
            ReporterType = request.ReporterType
        });
        
        return Results.Ok(result);
    }
}