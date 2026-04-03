using CC.AppointmentService.Domain.Feedbacks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.AppointmentService.Infrastructure.Persistence.Feedbacks.EntityTypeConfigurations;

public sealed class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.Property(e => e.AppointmentId).IsRequired();

        builder.Property(e => e.ComfortScore)
            .HasConversion(
                score => score.Value,
                value => ComfortScore.From(value))
            .IsRequired();

        builder.Property(e => e.ProfessionalismScore)
            .HasConversion(
                score => score.Value,
                value => ProfessionalismScore.From(value))
            .IsRequired();

        builder.Property(e => e.EmpathyScore)
            .HasConversion(
                score => score.Value,
                value => EmpathyScore.From(value))
            .IsRequired();
    }
}
