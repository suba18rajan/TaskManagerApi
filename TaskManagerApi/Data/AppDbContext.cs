using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;

namespace TaskManagerApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(x => x.Username)
                    .IsUnique();

                entity.Property(x => x.Username)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.PasswordHash)
                    .IsRequired();

                entity.Property(x => x.Role)
                    .HasMaxLength(20)
                    .IsRequired();
            });

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.Property(x => x.Title)
                    .HasMaxLength(100)
                    .IsRequired();
            });
        }
    }
}