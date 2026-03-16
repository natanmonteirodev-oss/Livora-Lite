using Livora_Lite.Application.Interface;
using Livora_Lite.Application.Services;
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
builder.Services.AddControllersWithViews();

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LivoraDbContext>(options =>
    options.UseSqlite(connectionString)
);

// Add session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is required");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is required");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is required");

builder.Services.AddAuthentication("CookieAuth")
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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
    })
    .AddCookie("CookieAuth", options =>
    {
        options.Cookie.Name = "LivoraAuth";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(5); // Default 5 minutes inactivity
        options.SlidingExpiration = true; // Renew on activity
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Login";
    });

// Injeção de dependências
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
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IBillingStatusService, BillingStatusService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IMaintenanceRequestRepository, MaintenanceRequestRepository>();
builder.Services.AddScoped<IMaintenanceRequestService, MaintenanceRequestService>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IAuditService, AuditService>();

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LivoraDbContext>();
    dbContext.Database.Migrate();

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
    var existingTypes = await propertyTypeRepo.GetAllAsync();
    if (!existingTypes.Any())
    {
        await propertyTypeRepo.CreateAsync(new PropertyType { Name = "Casa" });
        await propertyTypeRepo.CreateAsync(new PropertyType { Name = "Apartamento" });
        await propertyTypeRepo.CreateAsync(new PropertyType { Name = "Kitnet" });
        await propertyTypeRepo.CreateAsync(new PropertyType { Name = "Salão Comercial" });
    }

    // Seed property statuses
    var propertyStatusRepo = scope.ServiceProvider.GetRequiredService<IPropertyStatusRepository>();
    var existingStatuses = await propertyStatusRepo.GetAllAsync();
    if (!existingStatuses.Any())
    {
        await propertyStatusRepo.CreateAsync(new PropertyStatus { Name = "Disponível" });
        await propertyStatusRepo.CreateAsync(new PropertyStatus { Name = "Alugado" });
        await propertyStatusRepo.CreateAsync(new PropertyStatus { Name = "Manutenção" });
        await propertyStatusRepo.CreateAsync(new PropertyStatus { Name = "Reservado" });
        await propertyStatusRepo.CreateAsync(new PropertyStatus { Name = "Inativo" });
    }

    // Seed tenant statuses
    var tenantStatusRepo = scope.ServiceProvider.GetRequiredService<ITenantStatusRepository>();
    var existingTenantStatuses = await tenantStatusRepo.GetAllAsync();
    if (!existingTenantStatuses.Any())
    {
        await tenantStatusRepo.CreateAsync(new TenantStatus { Name = "Ativo" });
        await tenantStatusRepo.CreateAsync(new TenantStatus { Name = "Inativo" });
        await tenantStatusRepo.CreateAsync(new TenantStatus { Name = "Em Processo de Mudança" });
        await tenantStatusRepo.CreateAsync(new TenantStatus { Name = "Desistente" });
    }

    // Seed contract statuses
    var contractStatusRepo = scope.ServiceProvider.GetRequiredService<IContractStatusRepository>();
    var existingContractStatuses = await contractStatusRepo.GetAllAsync();
    if (!existingContractStatuses.Any())
    {
        await contractStatusRepo.CreateAsync(new ContractStatus { Name = "Ativo" });
        await contractStatusRepo.CreateAsync(new ContractStatus { Name = "Encerrado" });
        await contractStatusRepo.CreateAsync(new ContractStatus { Name = "Cancelado" });
        await contractStatusRepo.CreateAsync(new ContractStatus { Name = "Em renovação" });
    }

    // Seed billing statuses
    var billingStatusRepo = scope.ServiceProvider.GetRequiredService<IBillingStatusRepository>();
    var existingBillingStatuses = await billingStatusRepo.GetAllAsync();
    if (!existingBillingStatuses.Any())
    {
        await billingStatusRepo.CreateAsync(new BillingStatus { Name = "Pendente" });
        await billingStatusRepo.CreateAsync(new BillingStatus { Name = "Pago" });
        await billingStatusRepo.CreateAsync(new BillingStatus { Name = "Atrasado" });
        await billingStatusRepo.CreateAsync(new BillingStatus { Name = "Cancelado" });
    }

    // Seed payment methods
    var paymentMethodRepo = scope.ServiceProvider.GetRequiredService<IPaymentMethodRepository>();
    var existingPaymentMethods = await paymentMethodRepo.GetAllAsync();
    if (!existingPaymentMethods.Any())
    {
        await paymentMethodRepo.CreateAsync(new PaymentMethod { Name = "Dinheiro" });
        await paymentMethodRepo.CreateAsync(new PaymentMethod { Name = "PIX" });
        await paymentMethodRepo.CreateAsync(new PaymentMethod { Name = "Transferência Bancária" });
        await paymentMethodRepo.CreateAsync(new PaymentMethod { Name = "Cartão de Crédito" });
        await paymentMethodRepo.CreateAsync(new PaymentMethod { Name = "Cartão de Débito" });
        await paymentMethodRepo.CreateAsync(new PaymentMethod { Name = "Cheque" });
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Use session
app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
