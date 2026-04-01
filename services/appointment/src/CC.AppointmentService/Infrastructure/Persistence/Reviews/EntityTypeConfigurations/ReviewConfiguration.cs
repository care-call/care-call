using CC.AppointmentService.Domain.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.AppointmentService.Infrastructure.Persistence.Reviews.EntityTypeConfigurations;

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