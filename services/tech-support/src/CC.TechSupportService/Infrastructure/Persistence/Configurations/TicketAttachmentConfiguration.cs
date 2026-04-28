using CC.Shared.Domain;
using CC.TechSupportService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.TechSupportService.Infrastructure.Persistence.Configurations;

public class TicketAttachmentConfiguration : IEntityTypeConfiguration<TicketAttachment>
{
    public void Configure(EntityTypeBuilder<TicketAttachment> builder)
    {
        builder.HasKey(ta => ta.Id);

        builder.Property(ta => ta.Id)
            .HasConversion(new VogenEfCoreConverters.GuidIdEfCoreValueConverter());

        builder.Property(ta => ta.TicketId)
            .HasConversion(new VogenEfCoreConverters.GuidIdEfCoreValueConverter())
            .IsRequired();

        builder.HasMany(x => x.FileDetails)
            .WithOne()
            .HasForeignKey(x => x.AttachmentId)
            .IsRequired();

        builder.Property(ta => ta.CreatedAt)
            .IsRequired();
    }
}