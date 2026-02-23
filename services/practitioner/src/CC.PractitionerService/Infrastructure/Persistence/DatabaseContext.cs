using CC.PractitionerService.Domain.Practitioners;
using CC.PractitionerService.Domain.WorkSchedules;
using Microsoft.EntityFrameworkCore;

namespace CC.PractitionerService.Infrastructure.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<PractitionerProfile> PractitionerProfiles { get; set; }
    public DbSet<WorkSchedule> WorkSchedules { get; set; }
    public DbSet<Adjustment> Adjustments { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
       base.OnModelCreating(modelBuilder);
   }
}