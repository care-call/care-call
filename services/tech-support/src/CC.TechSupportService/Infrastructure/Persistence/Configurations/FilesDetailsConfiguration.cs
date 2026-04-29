using CC.TechSupportService.Domain.Entities;
using CC.TechSupportService.Domain.ValueObjects.FileDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.TechSupportService.Infrastructure.Persistence.Configurations;

public sealed class FilesDetailsConfiguration : IEntityTypeConfiguration<FileDetails>
{
    public void Configure(EntityTypeBuilder<FileDetails> builder)
    {
        builder.HasKey(fd => fd.Id);
        
        builder.Property(fd => fd.FileName)
            .HasConversion(x => x.Value, x => FileName.From(x))
            .HasMaxLength(FileName.MaxFileNameLength)
            .IsRequired();
        
        builder.Property(fd => fd.FilePath)
            .HasConversion(x => x.Value, x => FilePath.From(x))
            .HasMaxLength(FilePath.MaxFilePathLength)
            .IsRequired();

        builder.Property(fd => fd.ContentType)
            .HasConversion(new VogenEfCoreConverters.ContentTypeEfCoreValueConverter())
            .HasMaxLength(ContentType.MaxContentTypeLength)
            .IsRequired();

        builder.Property(fd => fd.ContentSize)
            .IsRequired();
    }
}