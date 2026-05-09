using CC.NotificationService.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.NotificationService.Infrastructure.Persistence.Notifications.EntityTypeConfigurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasIndex(e => e.CreatedAt);
        builder.Property(e => e.Payload).HasColumnType("jsonb");
        builder.Property(e => e.Trigger).HasMaxLength(500);

        builder.OwnsMany(n => n.Channels, ownedBuilder =>
        {
            ownedBuilder.Property(e => e.Error).HasMaxLength(2000);
        });
    }
}