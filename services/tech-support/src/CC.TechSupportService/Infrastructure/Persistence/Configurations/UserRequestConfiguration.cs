using CC.TechSupportService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.TechSupportService.Infrastructure.Persistence.Configurations;

public sealed class UserRequestConfiguration : IEntityTypeConfiguration<UserRequest>
{
    public void Configure(EntityTypeBuilder<UserRequest> builder)
    {
        builder.HasKey(usr => usr.Id);

        builder.Property(usr => usr.Id)
            .ValueGeneratedOnAdd();

        builder.Property(usr => usr.UserId)
            .IsRequired();

        builder.Property(usr => usr.AmountOfRequestForLastHour)
            .IsRequired();

        builder.Property(usr => usr.LastRequestTime)
            .IsRequired();
    }
}