using CC.TechSupportService.Domain.Entities;
using CC.TechSupportService.Domain.ValueObjects.Comment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.TechSupportService.Infrastructure.Persistence.Configurations;

public sealed class TicketCommentConfiguration : IEntityTypeConfiguration<TicketComment>
{
    public void Configure(EntityTypeBuilder<TicketComment> builder)
    {
        builder.HasKey(tc => tc.Id);
        
        builder.Property(tc => tc.TicketId)
            .IsRequired();

        builder.ComplexProperty(tc => tc.Author, au =>
        {
            au.Property(id => id.Id)
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
            .HasColumnType("timestamp with time zone")
            .IsRequired();
    }
}