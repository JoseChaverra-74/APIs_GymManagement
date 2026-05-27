using GymManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.DataAccess.Context;

public class GymDbContext : DbContext
{
    public GymDbContext(DbContextOptions<GymDbContext> options)
        : base(options)
    {
    }

    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<GymClass> GymClasses => Set<GymClass>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Name).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Type).IsRequired();
            entity.Property(m => m.MonthlyPrice).HasPrecision(10, 2);
            entity.Property(m => m.Description).HasMaxLength(500);
            entity.Property(m => m.CreatedAt).IsRequired();
            entity.HasIndex(m => m.Name).IsUnique();
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.FirstName).IsRequired().HasMaxLength(80);
            entity.Property(m => m.LastName).IsRequired().HasMaxLength(80);
            entity.Property(m => m.Email).IsRequired().HasMaxLength(150);
            entity.Property(m => m.Phone).HasMaxLength(20);
            entity.Property(m => m.Status).IsRequired();
            entity.Property(m => m.CreatedAt).IsRequired();

            entity.HasOne(m => m.Membership)
                .WithMany(ms => ms.Members)
                .HasForeignKey(m => m.MembershipId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(m => m.Email).IsUnique();
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.FirstName).IsRequired().HasMaxLength(80);
            entity.Property(t => t.LastName).IsRequired().HasMaxLength(80);
            entity.Property(t => t.Email).IsRequired().HasMaxLength(150);
            entity.Property(t => t.Phone).HasMaxLength(20);
            entity.Property(t => t.Specialty).IsRequired().HasMaxLength(100);
            entity.Property(t => t.CreatedAt).IsRequired();
            entity.HasIndex(t => t.Email).IsUnique();
        });

        modelBuilder.Entity<GymClass>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.Property(g => g.Name).IsRequired().HasMaxLength(150);
            entity.Property(g => g.Description).HasMaxLength(500);
            entity.Property(g => g.Status).IsRequired();
            entity.Property(g => g.CreatedAt).IsRequired();

            entity.HasOne(g => g.Trainer)
                .WithMany(t => t.GymClasses)
                .HasForeignKey(g => g.TrainerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EnrolledAt).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.GymClass)
                .WithMany(g => g.Enrollments)
                .HasForeignKey(e => e.GymClassId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Member)
                .WithMany(m => m.Enrollments)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.GymClassId, e.MemberId }).IsUnique();
        });
    }
}
