using CC.TechSupportService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CC.TechSupportService.Infrastructure.Persistence.Interceptors;

public class ChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var dbContext = eventData.Context;
        
        if(dbContext == null)
            return base.SavingChanges(eventData, result);

        var enumerable = dbContext.ChangeTracker.Entries<Ticket>();

        foreach (var entry in enumerable)
        {
            if (entry.State is EntityState.Added)
            {
                entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.Now;
            }
            else if (entry.State is EntityState.Modified)
            {
                entry.Property(x => x.UpdatedAt).CurrentValue = DateTime.Now;
            }
        }
        
        return base.SavingChanges(eventData, result);
    }
}