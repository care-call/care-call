using CC.HandbookService.Domain.Handbooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.HandbookService.Infrastructure.Persistence.EntityTypeConfigurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
    }
}
