using CC.AppointmentService.Domain.Reviews;
using CC.AppointmentService.Domain.Reviews.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.AppointmentService.Infrastructure.Persistence.Reviews.EntityTypeConfigurations;

public class SessionReviewConfiguration : IEntityTypeConfiguration<SessionReview>
{
    public void Configure(EntityTypeBuilder<SessionReview> builder)
    {
        builder.Property(x => x.EmpathyRating)
            .HasConversion(
                rating => rating.Value,
                value => EmpathyRating.From(value))
            .IsRequired();

        builder.Property(x => x.ProfessionalismRating)
            .HasConversion(
                rating => rating.Value,
                value => ProfessionalismRating.From(value))
            .IsRequired();

        builder.Property(x => x.ComfortRating)
            .HasConversion(
                rating => rating.Value,
                value => ComfortRating.From(value))
            .IsRequired();

        builder.Property(x => x.ReviewComment)
            .HasConversion(
                comment => comment.HasValue ? comment.Value.Value : null,
                value => value == null ? null : ReviewComment.From(value))
            .HasMaxLength(1000)
            .IsRequired(false);
    }
}