using System.Text.Json;
using CC.TechSupportService.Domain.Entities;

namespace CC.TechSupportService.Domain.Services;

public class TicketUpdatesService
{
    public string CreateJsonFromTicket(Ticket ticket)
        => JsonSerializer.Serialize(ticket);
}