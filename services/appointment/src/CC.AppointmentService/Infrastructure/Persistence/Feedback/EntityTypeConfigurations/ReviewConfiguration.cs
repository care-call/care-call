using CC.AppointmentService.Domain.Feedback;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ComfortScore = CC.AppointmentService.Domain.Feedback.ComfortScore;
using EmpathyScore = CC.AppointmentService.Domain.Feedback.EmpathyScore;
using ProfessionalismScore = CC.AppointmentService.Domain.Feedback.ProfessionalismScore;

namespace CC.AppointmentService.Infrastructure.Persistence.Feedback.EntityTypeConfigurations;

public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
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