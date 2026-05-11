using CC.StorageService.Entities;
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

            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => new { e.EntityType, e.EntityId });
            entity.HasIndex(e => e.ExpiresAt);
        });
    }
}
