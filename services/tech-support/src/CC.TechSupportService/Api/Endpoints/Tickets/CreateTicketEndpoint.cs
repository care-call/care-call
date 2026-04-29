using CC.TechSupportService.Application.Interfaces;
using CC.TechSupportService.Domain.Abstractions;

namespace CC.TechSupportService.API.Endpoints.Tickets;

public static class CreateTicketEndpoint
{
    public static async Task<IResult> Handle(ITicketRepository repository, 
        IUnitOfWork unitOfWork)
    {
        return Results.Ok();
    }
}