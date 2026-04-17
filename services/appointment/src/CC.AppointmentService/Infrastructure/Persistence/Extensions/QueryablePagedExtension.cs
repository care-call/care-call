using CC.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CC.AppointmentService.Infrastructure.Persistence.Extensions;

public static class QueryablePagedExtension
{
    extension<T>(IQueryable<T> source)
    {
        public async Task<PagedResult<T>> ToPagedResultAsync(int pageNumber,
            int pageSize, CancellationToken ct)
        {
            var totalRows = await source.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalRows / (double)pageSize);
            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        
            return new PagedResult<T>(items, new PageInfo(pageNumber, pageSize), totalRows, totalPages);
        }
    }
    
}