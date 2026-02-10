using CC.PractitionerService.Domain.WorkSchedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.PractitionerService.Infrastructure.Persistence.WorkSchedules.EntityTypeConfigurations;

public class WorkScheduleConfiguration : IEntityTypeConfiguration<WorkSchedule>
{
    public void Configure(EntityTypeBuilder<WorkSchedule> builder)
    {
        builder.ComplexProperty(e => e.ValidityPeriod);
        builder.ComplexProperty(e => e.SessionDuration, b => b.Property(p => p.Value).HasColumnName("session_duration"));

        builder.Ignore(p => p.TimeZone);
        builder.Property("_timeZoneId")
            .HasColumnName("time_zone_id")
            .IsRequired();

        builder.ComplexCollection(e => e.Recurrences, b => 
        {
            b.ToJson();
            b.ComplexProperty(r => r.TimeRange);
        });
    }
}