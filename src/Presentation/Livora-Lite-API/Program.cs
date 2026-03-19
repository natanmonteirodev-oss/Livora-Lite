using Livora_Lite.Application.Interface;
using Livora_Lite.Application.Services;
using Livora_Lite.Application.Mappings;
using Livora_Lite.Domain.Interfaces;
using Livora_Lite.Domain.Entities;
using Livora_Lite.Infrastructure.Persistence;
using Livora_Lite.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add NSwag/Swagger UI
builder.Services.AddOpenApiDocument();

// Add CORS for Blazor and other clients
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LivoraDbContext>(options =>
    options.UseSqlite(connectionString)
);

// Add JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is required");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is required");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is required");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Repository Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAppSettingsRepository, AppSettingsRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
builder.Services.AddScoped<IPropertyStatusRepository, PropertyStatusRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<ITenantStatusRepository, TenantStatusRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IContractStatusRepository, ContractStatusRepository>();
builder.Services.AddScoped<IBillingRepository, BillingRepository>();
builder.Services.AddScoped<IBillingStatusRepository, BillingStatusRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IMaintenanceRequestRepository, MaintenanceRequestRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

// Service Dependency Injection
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IBillingStatusService, BillingStatusService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IMaintenanceRequestService, MaintenanceRequestService>();
builder.Services.AddScoped<IAuditService, AuditService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IReportService, ReportService>();

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LivoraDbContext>();
    dbContext.Database.Migrate();

    // Seed admin user
    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    await SeedData.SeedAdminUserAsync(userRepository);

    // Seed default settings
    var appSettingsRepo = scope.ServiceProvider.GetRequiredService<IAppSettingsRepository>();
    var timeoutSetting = await appSettingsRepo.GetByKeyAsync("SessionTimeoutMinutes");
    if (timeoutSetting == null)
    {
        await appSettingsRepo.CreateOrUpdateAsync(new AppSettings
        {
            Key = "SessionTimeoutMinutes",
            Value = "5"
        });
    }

    // Seed property types
    var propertyTypeRepo = scope.ServiceProvider.GetRequiredService<IPropertyTypeRepository>();
    var totalPropertyTypes = await dbContext.PropertyTypes.CountAsync();
    if (totalPropertyTypes == 0)
    {
        await propertyTypeRepo.CreateAsync(new PropertyType { Name = "Casa", IsActive = true });
        await propertyTypeRepo.CreateAsync(new PropertyType { Name = "Apartamento", IsActive = true });
        await propertyTypeRepo.CreateAsync(new PropertyType { Name = "Kitnet", IsActive = true });
        await propertyTypeRepo.CreateAsync(new PropertyType { Name = "Salão Comercial", IsActive = true });
    }
    else
    {
        var inactivePropertyTypes = dbContext.PropertyTypes.Where(ts => !ts.IsActive).ToList();
        foreach (var type in inactivePropertyTypes)
        {
            type.IsActive = true;
        }
        await dbContext.SaveChangesAsync();
    }

    // Seed property statuses
    var propertyStatusRepo = scope.ServiceProvider.GetRequiredService<IPropertyStatusRepository>();
    var totalPropertyStatuses = await dbContext.PropertyStatuses.CountAsync();
    if (totalPropertyStatuses == 0)
    {
        await propertyStatusRepo.CreateAsync(new PropertyStatus { Name = "Disponível", IsActive = true });
        await propertyStatusRepo.CreateAsync(new PropertyStatus { Name = "Alugado", IsActive = true });
        await propertyStatusRepo.CreateAsync(new PropertyStatus { Name = "Manutenção", IsActive = true });
        await propertyStatusRepo.CreateAsync(new PropertyStatus { Name = "Reservado", IsActive = true });
        await propertyStatusRepo.CreateAsync(new PropertyStatus { Name = "Inativo", IsActive = true });
    }

    // Seed tenant statuses
    var tenantStatusRepo = scope.ServiceProvider.GetRequiredService<ITenantStatusRepository>();
    var totalTenantStatuses = await dbContext.TenantStatuses.CountAsync();
    if (totalTenantStatuses == 0)
    {
        await tenantStatusRepo.CreateAsync(new TenantStatus { Name = "Ativo", IsActive = true });
        await tenantStatusRepo.CreateAsync(new TenantStatus { Name = "Inativo", IsActive = true });
        await tenantStatusRepo.CreateAsync(new TenantStatus { Name = "Suspenso", IsActive = true });
    }

    // Seed contract statuses
    var contractStatusRepo = scope.ServiceProvider.GetRequiredService<IContractStatusRepository>();
    var totalContractStatuses = await dbContext.ContractStatuses.CountAsync();
    if (totalContractStatuses == 0)
    {
        await contractStatusRepo.CreateAsync(new ContractStatus { Name = "Ativo", IsActive = true });
        await contractStatusRepo.CreateAsync(new ContractStatus { Name = "Encerrado", IsActive = true });
        await contractStatusRepo.CreateAsync(new ContractStatus { Name = "Suspenso", IsActive = true });
        await contractStatusRepo.CreateAsync(new ContractStatus { Name = "Pendente", IsActive = true });
    }

    // Seed billing statuses
    var billingStatusRepo = scope.ServiceProvider.GetRequiredService<IBillingStatusRepository>();
    var totalBillingStatuses = await dbContext.BillingStatuses.CountAsync();
    if (totalBillingStatuses == 0)
    {
        await billingStatusRepo.CreateAsync(new BillingStatus { Name = "Pendente", IsActive = true });
        await billingStatusRepo.CreateAsync(new BillingStatus { Name = "Pago", IsActive = true });
        await billingStatusRepo.CreateAsync(new BillingStatus { Name = "Atrasado", IsActive = true });
        await billingStatusRepo.CreateAsync(new BillingStatus { Name = "Cancelado", IsActive = true });
    }

    // Seed payment methods
    var paymentMethodRepo = scope.ServiceProvider.GetRequiredService<IPaymentMethodRepository>();
    var totalPaymentMethods = await dbContext.PaymentMethods.CountAsync();
    if (totalPaymentMethods == 0)
    {
        await paymentMethodRepo.CreateAsync(new PaymentMethod { Name = "Dinheiro", IsActive = true });
        await paymentMethodRepo.CreateAsync(new PaymentMethod { Name = "Cartão de Crédito", IsActive = true });
        await paymentMethodRepo.CreateAsync(new PaymentMethod { Name = "Cartão de Débito", IsActive = true });
        await paymentMethodRepo.CreateAsync(new PaymentMethod { Name = "Transferência Bancária", IsActive = true });
        await paymentMethodRepo.CreateAsync(new PaymentMethod { Name = "Boleto", IsActive = true });
        await paymentMethodRepo.CreateAsync(new PaymentMethod { Name = "PIX", IsActive = true });
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
