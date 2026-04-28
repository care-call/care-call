using CC.Shared.Domain;
using CC.TechSupportService.API.Contracts.Responces;
using CC.TechSupportService.Domain.Abstractions;
using CC.TechSupportService.Domain.Entities;
using CC.TechSupportService.Domain.Enums;
using CC.TechSupportService.Domain.ValueObjects.Ticket;

namespace CC.TechSupportService.Application.UseCases.Tickets.Creation;

public sealed record CreateTicket
{
    public required TicketSubject Subject { get; init; }
    public required TicketCategory Category { get; init; }
    public required TicketDescription Description { get; init; }
    public required Guid ReporterId { get; init; }
    public required ReporterType ReporterType { get; init; }
    public required int Number { get; init; }
}

public static class CreateTicketUseCase
{
    public static async ValueTask<CreateTicketResponce> Handle(
        CreateTicket command,
        IUnitOfWork unitOfWork,
        ITicketRepository ticketRepository,
        CancellationToken cancellationToken)
    {
        var ticketResult = Ticket.TryCreate(
            GuidId.From(Guid.CreateVersion7()),
            [],
            [],
            command.Number,
            Reporter.TryCreate(command.ReporterId, command.ReporterType).Value,
            command.Subject,
            command.Description,
            command.Category,
            null,
            DateTime.UtcNow);
       
        await ticketRepository.AddAsync(ticketResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new CreateTicketResponce(ticketResult.Value.Number);
    }

}