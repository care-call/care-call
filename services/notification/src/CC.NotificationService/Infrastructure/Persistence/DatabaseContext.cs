using CC.NotificationService.Domain;
using Microsoft.EntityFrameworkCore;

namespace CC.NotificationService.Infrastructure.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<TemplateVersion> TemplateVersions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}