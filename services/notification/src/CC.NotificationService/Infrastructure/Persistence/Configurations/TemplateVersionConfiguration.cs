using CC.NotificationService.Domain.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.NotificationService.Infrastructure.Persistence.Configurations;

public class TemplateVersionConfiguration : IEntityTypeConfiguration<TemplateVersion>
{
    public void Configure(EntityTypeBuilder<TemplateVersion> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasOne(x => x.Template)
            .WithMany()
            .HasForeignKey(x => x.TemplateKey)
            .HasPrincipalKey(x => x.Key);

        builder.OwnsMany(t => t.Channels, builder =>
        {
            builder.ToJson();
        });
    }
}