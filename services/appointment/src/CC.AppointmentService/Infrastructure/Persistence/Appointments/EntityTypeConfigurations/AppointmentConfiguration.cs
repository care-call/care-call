using CC.AppointmentService.Domain.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vogen;

namespace CC.AppointmentService.Infrastructure.Persistence.Appointments.EntityTypeConfigurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.Property(e => e.CancellationReason)
            .HasConversion(new VogenEfCoreConverters.CancellationReasonEfCoreValueConverter())
            .HasMaxLength(2000);
        builder.ComplexProperty(e => e.Completion, b =>
        {
            b.Property(c => c.EndedAt).HasColumnName("ended_at");
        });
        builder.ComplexProperty(e => e.TimeSlot);
        builder.ComplexProperty(e => e.ClientSnapshot, b => b.ToJson());
        builder.ComplexProperty(e => e.PractitionerSnapshot, b => b.ToJson());
    }
}

[EfCoreConverter<CancellationReason>]
internal partial class VogenEfCoreConverters;