using CC.AppointmentService.Domain.Reviews;
using CC.AppointmentService.Domain.Reviews.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TagValue = CC.AppointmentService.Domain.Reviews.ValueObjects.Tags.Tag;

namespace CC.AppointmentService.Infrastructure.Persistence.Reviews.EntityTypeConfigurations;

public class SessionReviewConfiguration : IEntityTypeConfiguration<SessionReview>
{
    public void Configure(EntityTypeBuilder<SessionReview> builder)
    {
        var tagsComparer = new ValueComparer<List<TagValue>>(
            (left, right) => left != null && right != null && left.SequenceEqual(right),
            tags => tags == null
                ? 0
                : tags.Aggregate(0, (current, tag) => HashCode.Combine(current, tag.Value.GetHashCode())),
            tags => tags == null
                ? new List<TagValue>()
                : tags.Select(tag => TagValue.From(tag.Value)).ToList());

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
        
        var tagsProperty = builder.Property(x => x.Tags)
            .HasConversion(
                tags => tags.Select(tag => tag.Value).ToArray(),
                values => values.Select(TagValue.From).ToList())
            .HasColumnType("uuid[]")
            .IsRequired();
        tagsProperty.Metadata.SetValueComparer(tagsComparer);
    }
}
