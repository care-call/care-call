using CC.HandbookService.Domain.Handbooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.HandbookService.Infrastructure.Persistence.EntityTypeConfigurations;

public class AgeGroupConfiguration : IEntityTypeConfiguration<AgeGroup>
{
    public void Configure(EntityTypeBuilder<AgeGroup> builder)
    {
        builder.Property(x => x.FromAge);
        builder.Property(x => x.ToAge);
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
