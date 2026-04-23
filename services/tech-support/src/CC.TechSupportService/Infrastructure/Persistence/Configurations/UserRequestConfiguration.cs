using CC.Shared.Domain;
using CC.TechSupportService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.TechSupportService.Infrastructure.Persistence.Configurations;

public class UserRequestConfiguration : IEntityTypeConfiguration<UserRequest>
{
    public void Configure(EntityTypeBuilder<UserRequest> builder)
    {
        builder.HasKey(usr => usr.Id);

        builder.Property(usr => usr.Id)
            .HasConversion(id => id.Value, value => LongId.From(value))
            .ValueGeneratedOnAdd();

        builder.Property(usr => usr.UserId)
            .HasConversion(userId => userId.Value, value => GuidId.From(value))
            .IsRequired();

        builder.Property(usr => usr.AmountOfRequestForLastHour)
            .IsRequired();

        builder.Property(usr => usr.LastRequestTime)
            .IsRequired();
    }
}