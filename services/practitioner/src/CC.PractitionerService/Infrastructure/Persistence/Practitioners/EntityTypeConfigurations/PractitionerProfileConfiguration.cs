using CC.PractitionerService.Domain.Practitioners;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.PractitionerService.Infrastructure.Persistence.Practitioners.EntityTypeConfigurations;

public class PractitionerProfileConfiguration : IEntityTypeConfiguration<PractitionerProfile>
{
    public void Configure(EntityTypeBuilder<PractitionerProfile> builder)
    {
        builder.ComplexProperty(e => e.FullName, b =>
        {
            b.Property(p => p.Name).HasColumnName("name");
            b.Property(p => p.Surname).HasColumnName("surname");
            b.Property(p => p.Patronymic).HasColumnName("patronymic");
        });
        builder.ComplexProperty(e => e.Specializations);
        builder.ComplexProperty(e => e.PhotoUrl, b => b.Property(p => p.Value).HasColumnName("photo_url"));
    }
}