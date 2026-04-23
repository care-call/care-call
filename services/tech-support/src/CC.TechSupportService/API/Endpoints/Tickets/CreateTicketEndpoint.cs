using CC.TechSupportService.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CC.TechSupportService.API.Endpoints.Tickets;

public static class CreateTicketEndpoint
{
    public static async Task<IResult> Handle([FromServices] ITicketRepository repository, 
        [FromServices] IUnitOfWork unitOfWork)
    {
        
        return Results.Ok();
    }
}