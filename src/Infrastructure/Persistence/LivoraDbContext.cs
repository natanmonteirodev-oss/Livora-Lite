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
        public DbSet<Tenant> Tenants { get; set; } = null!;
        public DbSet<TenantStatus> TenantStatuses { get; set; } = null!;
        public DbSet<Contract> Contracts { get; set; } = null!;
        public DbSet<ContractStatus> ContractStatuses { get; set; } = null!;
        public DbSet<Billing> Billings { get; set; } = null!;
        public DbSet<BillingStatus> BillingStatuses { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

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

            // Configuração da entidade TenantStatus
            modelBuilder.Entity<TenantStatus>(entity =>
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
                    .HasDatabaseName("IX_TenantStatus_Name_Unique");
            });

            // Configuração da entidade Tenant
            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Document)
                    .IsRequired()
                    .HasMaxLength(20); // CPF or CNPJ

                entity.Property(e => e.Phone)
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .HasMaxLength(100);

                entity.Property(e => e.CurrentAddress)
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Foreign key
                entity.HasOne(t => t.TenantStatus)
                    .WithMany()
                    .HasForeignKey(t => t.TenantStatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração da entidade ContractStatus
            modelBuilder.Entity<ContractStatus>(entity =>
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
                    .HasDatabaseName("IX_ContractStatus_Name_Unique");
            });

            // Configuração da entidade Contract
            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.StartDate)
                    .IsRequired();

                entity.Property(e => e.RentValue)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.DueDay)
                    .IsRequired();

                entity.Property(e => e.LateFee)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(e => e.InterestRate)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(e => e.SecurityDeposit)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Foreign keys
                entity.HasOne(c => c.Property)
                    .WithMany()
                    .HasForeignKey(c => c.PropertyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Tenant)
                    .WithMany()
                    .HasForeignKey(c => c.TenantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.ContractStatus)
                    .WithMany()
                    .HasForeignKey(c => c.ContractStatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração da entidade BillingStatus
            modelBuilder.Entity<BillingStatus>(entity =>
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
                    .HasDatabaseName("IX_BillingStatus_Name_Unique");
            });

            // Configuração da entidade Billing
            modelBuilder.Entity<Billing>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Period)
                    .IsRequired()
                    .HasMaxLength(7); // MM/YYYY

                entity.Property(e => e.Amount)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.DueDate)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Foreign keys
                entity.HasOne(b => b.Contract)
                    .WithMany()
                    .HasForeignKey(b => b.ContractId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.BillingStatus)
                    .WithMany()
                    .HasForeignKey(b => b.BillingStatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Índice único para período por contrato
                entity.HasIndex(b => new { b.ContractId, b.Period })
                    .IsUnique()
                    .HasDatabaseName("IX_Billing_Contract_Period_Unique");
            });

            // Configuração da entidade PaymentMethod
            modelBuilder.Entity<PaymentMethod>(entity =>
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
                    .HasDatabaseName("IX_PaymentMethod_Name_Unique");
            });

            // Configuração da entidade Payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PaymentDate)
                    .IsRequired();

                entity.Property(e => e.AmountPaid)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Foreign keys
                entity.HasOne(p => p.Billing)
                    .WithMany(b => b.Payments)
                    .HasForeignKey(p => p.BillingId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.PaymentMethod)
                    .WithMany()
                    .HasForeignKey(p => p.PaymentMethodId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração da entidade MaintenanceRequest
            modelBuilder.Entity<MaintenanceRequest>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.RequestDate)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                // Foreign keys
                entity.HasOne(mr => mr.Property)
                    .WithMany()
                    .HasForeignKey(mr => mr.PropertyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(mr => mr.Contract)
                    .WithMany()
                    .HasForeignKey(mr => mr.ContractId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
