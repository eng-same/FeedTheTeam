
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Feed.Domain.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pool> Pools { get; set; }
    public DbSet<PoolOption> PoolOptions { get; set; }
    public DbSet<Vote> Votes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //  Pool 
        modelBuilder.Entity<Pool>()
            .HasOne(p => p.CreatedBy)
            .WithMany(u => u.PoolsCreated)
            .HasForeignKey(p => p.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pool>()
            .Property(p => p.Title)
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder.Entity<Pool>()
            .Property(p => p.Description)
            .HasMaxLength(1000)
            .IsRequired();

        //  PoolOption 
        modelBuilder.Entity<PoolOption>()
            .HasOne(po => po.Pool)
            .WithMany(p => p.Options)
            .HasForeignKey(po => po.PoolId)
            .OnDelete(DeleteBehavior.Cascade);

        //  Vote 
        modelBuilder.Entity<Vote>()
            .HasOne(v => v.Pool)
            .WithMany(p => p.Votes)
            .HasForeignKey(v => v.PoolId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vote>()
            .HasOne(v => v.PoolOption)
            .WithMany(po => po.Votes)
            .HasForeignKey(v => v.PoolOptionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vote>()
            .HasOne(v => v.User)
            .WithMany()
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Prevent duplicate votes (User can vote once per poll)
        modelBuilder.Entity<Vote>()
            .HasIndex(v => new { v.UserId, v.PoolId })
            .IsUnique();
    }
}
