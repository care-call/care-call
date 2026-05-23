using CC.StorageService.Entities;
using CC.StorageService.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CC.StorageService.Persistence;

public class Db : DbContext
{
    public Db(DbContextOptions<Db> options)
       : base(options) { }

    public DbSet<FileMetadata> FileMetadata => Set<FileMetadata>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileMetadata>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.S3Key).IsRequired().HasMaxLength(500);
            entity.Property(e => e.OriginalName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);  // ← просто строка
            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.FieldName).HasMaxLength(50);

            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => new { e.EntityType, e.EntityId });
            entity.HasIndex(e => e.ExpiresAt);
        });
    }
}
