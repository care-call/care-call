using CC.HandbookService.Domain.Handbooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CC.HandbookService.Infrastructure.Persistence.EntityTypeConfigurations;

public class ProblemAreaConfiguration : IEntityTypeConfiguration<ProblemArea>
{
    public void Configure(EntityTypeBuilder<ProblemArea> builder)
    {
    }
}
