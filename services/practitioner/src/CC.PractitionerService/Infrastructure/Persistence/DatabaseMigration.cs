using Microsoft.EntityFrameworkCore;

namespace CC.PractitionerService.Infrastructure.Persistence;

public static class DatabaseMigration
{
    extension(IServiceProvider serviceProvider)
    {
        public async Task ApplyMigrationsAsync()
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            await db.Database.MigrateAsync();
        }
    }
}
