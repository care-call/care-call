using CC.TechSupportService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.TechSupportService.Infrastructure.Persistence.Configurations;

public sealed class TicketAttachmentConfiguration : IEntityTypeConfiguration<TicketAttachment>
{
    public void Configure(EntityTypeBuilder<TicketAttachment> builder)
    {
        builder.HasKey(ta => ta.Id);
        
        builder.Property(ta => ta.TicketId)
            .IsRequired();

        builder.HasMany(x => x.FileDetails)
            .WithOne()
            .HasForeignKey(x => x.AttachmentId)
            .IsRequired();

        builder.Property(ta => ta.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired();
    }
}