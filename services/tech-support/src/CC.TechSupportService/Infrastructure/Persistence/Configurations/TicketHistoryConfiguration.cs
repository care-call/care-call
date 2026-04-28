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
            .HasConversion(new VogenEfCoreConverters.GuidIdEfCoreValueConverter());
        
        builder.Property(th => th.TicketId)
            .HasConversion(new VogenEfCoreConverters.GuidIdEfCoreValueConverter())
            .IsRequired();
        
        builder.Property(th => th.ActorId)
            .HasConversion(new VogenEfCoreConverters.GuidIdEfCoreValueConverter())
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