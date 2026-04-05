using CC.HandbookService.Domain.Handbooks;
using Microsoft.EntityFrameworkCore;

namespace CC.HandbookService.Infrastructure.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Language> Languages { get; set; }
    public DbSet<AgeGroup> AgeGroups { get; set; }
    public DbSet<ProblemArea> ProblemAreas { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        
        modelBuilder.Entity<HandbookItem>().UseTpcMappingStrategy();
        
        base.OnModelCreating(modelBuilder);
    }
}