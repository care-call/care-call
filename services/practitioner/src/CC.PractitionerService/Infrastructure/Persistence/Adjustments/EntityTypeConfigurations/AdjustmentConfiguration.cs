using CC.PractitionerService.Domain.WorkSchedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.PractitionerService.Infrastructure.Persistence.Adjustments.EntityTypeConfigurations;

public class AdjustmentConfiguration : IEntityTypeConfiguration<Adjustment>
{
    public void Configure(EntityTypeBuilder<Adjustment> builder)
    {
        builder.ComplexProperty(e => e.Period, period =>
        {
            period.Property(p => p.From).HasColumnName("period_from").IsRequired();
            period.Property(p => p.To).HasColumnName("period_to").IsRequired();
        });
    }
}