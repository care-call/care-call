using CC.AppointmentService.Domain.Appointments;
using CC.AppointmentService.Domain.Feedbacks;
using Microsoft.EntityFrameworkCore;

namespace CC.AppointmentService.Infrastructure.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
