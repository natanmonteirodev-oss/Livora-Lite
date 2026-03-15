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
        public DbSet<AppSettings> AppSettings { get; set; } = null!;
        public DbSet<Property> Properties { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<PropertyType> PropertyTypes { get; set; } = null!;
        public DbSet<PropertyStatus> PropertyStatuses { get; set; } = null!;

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

            // Configuração da entidade AppSettings
            modelBuilder.Entity<AppSettings>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Índice único para Key
                entity.HasIndex(e => e.Key)
                    .IsUnique()
                    .HasDatabaseName("IX_AppSettings_Key_Unique");
            });

            // Configuração da entidade Address
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Complement)
                    .HasMaxLength(100);

                entity.Property(e => e.Neighborhood)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);
            });

            // Configuração da entidade PropertyType
            modelBuilder.Entity<PropertyType>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Índice único para Name
                entity.HasIndex(e => e.Name)
                    .IsUnique()
                    .HasDatabaseName("IX_PropertyType_Name_Unique");
            });

            // Configuração da entidade PropertyStatus
            modelBuilder.Entity<PropertyStatus>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Índice único para Name
                entity.HasIndex(e => e.Name)
                    .IsUnique()
                    .HasDatabaseName("IX_PropertyStatus_Name_Unique");
            });

            // Configuração da entidade Property
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Area)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.SuggestedRentValue)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Foreign keys
                entity.HasOne(p => p.Address)
                    .WithMany()
                    .HasForeignKey(p => p.AddressId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.PropertyType)
                    .WithMany()
                    .HasForeignKey(p => p.PropertyTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.PropertyStatus)
                    .WithMany()
                    .HasForeignKey(p => p.PropertyStatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
