using CC.Shared.Domain;
using CC.TechSupportService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.TechSupportService.Infrastructure.Persistence.Configurations;

public class TicketHistoryConfiguration : IEntityTypeConfiguration<TicketHistory>
{
    public void Configure(EntityTypeBuilder<TicketHistory> builder)
    {
        builder.HasKey(th => th.Id);

        builder.Property(th => th.Id)
            .HasConversion(id => id.Value, value => GuidId.From(value));
        
        builder.Property(th => th.TicketId)
            .HasConversion(id => id.Value, value => GuidId.From(value))
            .IsRequired();
        
        builder.Property(th => th.ActorId)
            .HasConversion(id => id.Value, value => GuidId.From(value))
            .IsRequired();

        builder.Property(th => th.ActionType)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(th => th.OldValue)
            .HasColumnType("jsonb")
            .IsRequired(false);
        
        builder.Property(th => th.NewValue)
            .HasColumnType("jsonb")
            .IsRequired(false);

        builder.Property(th => th.CreatedAt)
            .IsRequired();
    }
}