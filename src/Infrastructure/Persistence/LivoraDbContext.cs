using Microsoft.EntityFrameworkCore;
using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Infrastructure.Persistence
{
    public class LivoraDbContext : DbContext
    {
        public LivoraDbContext(DbContextOptions<LivoraDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Índice único para email
                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_User_Email_Unique");

                // Índice para IsActive
                entity.HasIndex(e => e.IsActive)
                    .HasDatabaseName("IX_User_IsActive");
            });
        }
    }
}
