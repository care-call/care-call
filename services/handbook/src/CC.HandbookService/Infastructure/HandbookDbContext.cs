using Microsoft.EntityFrameworkCore;

namespace CC.HandbookService.Infastructure;

public class HandbookDbContext(DbContextOptions<HandbookDbContext> options) : DbContext(options)
{
    public DbSet<HandbookRecord> HandbookRecords => Set<HandbookRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<HandbookRecord>();

        entity.ToTable("handbook_records");
        entity.HasKey(x => x.Id);
        entity.Property(x => x.ExternalId).HasMaxLength(128).IsRequired();
        entity.Property(x => x.Name).HasMaxLength(512).IsRequired();
        entity.Property(x => x.Code).HasMaxLength(128).IsRequired();
        entity.HasIndex(x => new { x.HandbookType, x.ExternalId }).IsUnique();
        entity.HasIndex(x => new { x.HandbookType, x.Code }).IsUnique();
    }
}