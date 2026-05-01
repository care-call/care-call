using CC.Common.Models;
using CC.TechSupportService.Domain.Entities;
using CC.TechSupportService.Features.Tickets.Extensions;
using CC.TechSupportService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wolverine.Http;

namespace CC.TechSupportService.Features.Tickets.GetMyTickets;

public class GetMyTicketsFeature
{
    [ProducesResponseType<PagedResult<TicketItemDto>>(200)]
    [WolverineGet("api/v1/tickets/me")]
    public static async Task<IResult> Handle(
        [FromQuery] GetMyTicketsFilterDto? filterDto,
        int pageNumber,
        int pageSize,
        DatabaseContext dbContext)
    {
        var query = dbContext.Tickets.AsQueryable();
    
        query = ApplyFilters(query, filterDto);
        var pagedResult = await ApplyPagination(query, new PageInfo(pageNumber, pageSize));
       
        return Results.Ok(pagedResult);
    }
    
    private static async Task<PagedResult<TicketItemDto>> ApplyPagination(IQueryable<Ticket> query, PageInfo pageInfo)
    {
        var totalRows = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRows / (double)pageInfo.Size);

        var items = await query
            .Skip((pageInfo.Number - 1) * pageInfo.Size)
            .Take(pageInfo.Size)
            .ToListAsync();
        
        var pageItems = items
            .Select(i => new TicketItemDto
            {
                Id = i.Id,
                Status = i.Status.GetType().Name,
                Category = i.TicketCategory.GetType().Name,
                Description = i.Description.Value,
                Subject = i.Subject.Value,
                Number = i.Number
            })
            .ToList();
        
        var pagedResult = new PagedResult<TicketItemDto>(pageItems, pageInfo, totalRows, totalPages);
        
        return pagedResult;
    } 
    
    private static IQueryable<Ticket> ApplyFilters(
        IQueryable<Ticket> query,
        GetMyTicketsFilterDto? filterDto)
    {
        if (filterDto != null) 
            query = query.WhereStatus(filterDto.Status);
        return query;
    }

}

public sealed record TicketItemDto
{
    public required Guid Id { get; set; }
    public required int Number { get; set; }
    public required string Subject { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
    public required string Status { get; set; }
}

public sealed record GetMyTicketsFilterDto(string? Status);