using Livora_Lite_Blazor.Components;
using Livora_Lite_Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register Auth Service (NO handler - this is the service that handles auth)
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IAuthService>(sp => sp.GetRequiredService<AuthService>());
builder.Services.AddHttpClient<AuthService>();

// Register Dashboard Service with authorization handler
// Register handler AFTER IAuthService is available
builder.Services.AddScoped<AuthorizationDelegatingHandler>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<IDashboardService>(sp => sp.GetRequiredService<DashboardService>());
builder.Services.AddHttpClient<DashboardService>()
    .AddHttpMessageHandler<AuthorizationDelegatingHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
