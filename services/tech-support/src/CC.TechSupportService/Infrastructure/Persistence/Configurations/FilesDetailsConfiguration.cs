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
            .HasConversion(new VogenEfCoreConverters.GuidIdEfCoreValueConverter());

        builder.Property(fd => fd.FileName)
            .HasMaxLength(FileDetails.MaxFileNameLength)
            .IsRequired();
        
        builder.Property(fd => fd.FilePath)
            .HasMaxLength(FileDetails.MaxFilePathLength)
            .IsRequired();

        builder.Property(fd => fd.ContentType)
            .HasConversion(new VogenEfCoreConverters.ContentTypeEfCoreValueConverter())
            .HasMaxLength(ContentType.MaxContentTypeLength)
            .IsRequired();

        builder.Property(fd => fd.ContentSize)
            .IsRequired();
    }
}