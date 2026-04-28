using CC.Shared.Domain;
using CC.TechSupportService.Domain.Entities;
using CC.TechSupportService.Domain.ValueObjects.Comment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.TechSupportService.Infrastructure.Persistence.Configurations;

public class TicketCommentConfiguration : IEntityTypeConfiguration<TicketComment>
{
    public void Configure(EntityTypeBuilder<TicketComment> builder)
    {
        builder.HasKey(tc => tc.Id);

        builder.Property(tc => tc.Id)
            .HasConversion(new VogenEfCoreConverters.GuidIdEfCoreValueConverter());
        
        builder.Property(tc => tc.TicketId)
            .HasConversion(new VogenEfCoreConverters.GuidIdEfCoreValueConverter())
            .IsRequired();

        builder.ComplexProperty(tc => tc.Author, au =>
        {
            au.Property(id => id.Id)
                .HasConversion(new VogenEfCoreConverters.GuidIdEfCoreValueConverter())
                .IsRequired();

            au.Property(at => at.Type)
                .HasConversion<string>()
                .IsRequired();
        });

        builder.Property(tc => tc.Body)
            .HasConversion(new VogenEfCoreConverters.CommentBodyEfCoreValueConverter())
            .HasMaxLength(CommentBody.MaxBodyLenght)
            .IsRequired();
        
        builder.Property(tc => tc.IsInternal)
            .IsRequired();

        builder.Property(tc => tc.CreatedAt)
            .IsRequired();
    }
}