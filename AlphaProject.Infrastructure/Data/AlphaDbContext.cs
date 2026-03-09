using AlphaProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlphaProject.Infrastructure.Data;

public class AlphaDbContext : DbContext
{
    public AlphaDbContext(DbContextOptions<AlphaDbContext> options) : base(options)
    {
    }

    public DbSet<Institution> Institutions { get; set; } = null!;
    public DbSet<TieredInterestRule> TieredInterestRules { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Institution>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<TieredInterestRule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LimitAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.PrimaryRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.FallbackRate).HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Institution)
                .WithMany(i => i.InterestRules)
                .HasForeignKey(e => e.InstitutionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
