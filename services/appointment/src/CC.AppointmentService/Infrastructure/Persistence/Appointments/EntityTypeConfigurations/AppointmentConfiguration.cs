using CC.AppointmentService.Domain.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.AppointmentService.Infrastructure.Persistence.Appointments.EntityTypeConfigurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.Property(e => e.CancellationReason).HasMaxLength(2000);
        builder.ComplexProperty(e => e.TimeSlot);
        builder.ComplexProperty(e => e.ClientSnapshot, b => b.ToJson());
        builder.ComplexProperty(e => e.PractitionerSnapshot, b => b.ToJson());
    }
}