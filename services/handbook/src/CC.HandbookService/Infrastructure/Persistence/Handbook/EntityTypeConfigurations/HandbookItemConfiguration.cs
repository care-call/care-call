using CC.HandbookService.Domain.Handbooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.HandbookService.Infrastructure.Persistence.Handbooks.EntityTypeConfigurations;

public class HandbookItemConfiguration : IEntityTypeConfiguration<HandbookItem>
{
    public void Configure(EntityTypeBuilder<HandbookItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(32);
        builder.Property(x => x.DisplayName).HasMaxLength(64);
    }
}