using CC.Shared.Domain;
using CC.TechSupportService.Domain.Entities;
using CC.TechSupportService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.TechSupportService.Infrastructure.Persistence.Configurations;

public class FilesDetailsConfiguration : IEntityTypeConfiguration<FileDetails>
{
    public void Configure(EntityTypeBuilder<FileDetails> builder)
    {
        builder.HasKey(fd => fd.Id);

        builder.Property(fd => fd.Id)
            .HasConversion(id => id.Value, value => GuidId.From(value));

        builder.Property(fd => fd.FileName)
            .HasMaxLength(FileDetails.MaxFileNameLength)
            .IsRequired();
        
        builder.Property(fd => fd.FilePath)
            .HasMaxLength(FileDetails.MaxFilePathLength)
            .IsRequired();

        builder.Property(fd => fd.ContentType)
            .HasConversion(ct => ct.Type, value => ContentType.TryCreate(value).Value)
            .HasMaxLength(ContentType.MaxContentTypeLength)
            .IsRequired();

        builder.Property(fd => fd.ContentSize)
            .IsRequired();
    }
}