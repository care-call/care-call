using CC.PractitionerService.Infrastructure.Persistence.Availability.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.PractitionerService.Infrastructure.Persistence.Availability.EntityTypeConfigurations;

public class EmploymentRecordConfiguration : IEntityTypeConfiguration<EmploymentRecord>
{
    public void Configure(EntityTypeBuilder<EmploymentRecord> builder)
    {
        builder.ComplexProperty(e => e.Period, period =>
        {
            period.Property(p => p.From).HasColumnName("period_from").IsRequired();
            period.Property(p => p.To).HasColumnName("period_to").IsRequired();
        });

        builder.Property(x => x.ExternalEmploymentKey).HasMaxLength(128);

        builder.HasIndex(x => x.ExternalEmploymentKey);
    }
}