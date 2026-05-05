using System.Reflection;
using CC.TechSupportService.Domain.Entities;
using CC.TechSupportService.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace CC.TechSupportService.Infrastructure.Persistence;

public class DatabaseContext(
    DbContextOptions<DatabaseContext> options,
    IConnectionMultiplexer connectionMultiplexer) : DbContext(options)
{
    public const string SchemeName = "tech_support";
    
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketHistory> TicketHistories { get; set; }
    public DbSet<TicketAttachment> TicketAttachments { get; set; }
    public DbSet<FileDetails> FilesDetails { get; set; }
    public DbSet<TicketComment> TicketComments { get; set; }
    public DbSet<UserRequest> UsersRequest { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasDefaultSchema(SchemeName);
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(
            new ChangesInterceptor(),
            new TicketChangesInterceptor(connectionMultiplexer));
    }
}