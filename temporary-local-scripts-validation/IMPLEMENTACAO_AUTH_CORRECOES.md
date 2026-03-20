# 🛠️ Guia de Implementação - Correção de Auth

**Objetivo:** Resolver os 3 problemas críticos (P0) em 8-10 horas  
**Data Início:** 20.03.2026

---

## Sprint Roadmap

```
Dia 1 (4 horas):
  ├─ P0.1: LocalStorage Setup (1.5h)
  ├─ P0.3: Rate Limiting (1h)
  └─ Teste: Login + Refresh Page (1.5h)

Dia 2 (6 horas):
  ├─ P0.2.1: API - Refresh Token Endpoints (2.5h)
  ├─ P0.2.2: Blazor - Handler para 401 (2h)
  └─ Teste integrado: Completo flow (1.5h)
```

---

## P0.1: Persistência em LocalStorage (2-3 horas)

### Step 1: Adicionar Blazored.LocalStorage

```powershell
cd src/Presentation/Livora-Lite-Blazor
dotnet add package Blazored.LocalStorage
```

### Step 2: Registrar no Program.cs

```csharp
// ANTES
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IAuthService>(sp => sp.GetRequiredService<AuthService>());

// DEPOIS
builder.Services.AddBlazoredLocalStorage();  // ← Adicionar
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IAuthService>(sp => sp.GetRequiredService<AuthService>());
```

### Step 3: Atualizar AuthService

```csharp
using Blazored.LocalStorage;

public class AuthService : IAuthService
{
    private const string TOKEN_KEY = "auth_token";
    private const string USER_KEY = "auth_user";
    
    private readonly ILocalStorageService _localStorage;
    
    public AuthService(
        HttpClient httpClient, 
        IConfiguration configuration,
        ILocalStorageService localStorage)  // ← Adicionar
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _localStorage = localStorage;
        _httpClient.BaseAddress = new Uri(
            _configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5000");
    }

    // Modificar LoginAsync para persistir
    public async Task<AuthResponseDTO> LoginAsync(string email, string password)
    {
        var result = await /* ... existing logic ... */;
        
        if (result.Success && result.Token != null && result.User != null)
        {
            _token = result.Token;
            _currentUser = result.User;
            
            // ✅ NOVO: Persistir
            await _localStorage.SetItemAsync(TOKEN_KEY, _token);
            await _localStorage.SetItemAsync(USER_KEY, result.User);
            
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);
        }
        
        return result;
    }

    // Modificar LogoutAsync
    public async Task LogoutAsync()
    {
        _token = null;
        _currentUser = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
        
        // ✅ NOVO: Remover do localStorage
        await _localStorage.RemoveItemAsync(TOKEN_KEY);
        await _localStorage.RemoveItemAsync(USER_KEY);
    }

    // ✅ NOVO: Restaurar from localStorage
    public async Task RestoreSessionAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>(TOKEN_KEY);
            var user = await _localStorage.GetItemAsync<UserDTO>(USER_KEY);
            
            if (!string.IsNullOrEmpty(token) && user != null)
            {
                _token = token;
                _currentUser = user;
                
                // Validar token com API
                // TODO: Implementar endpoint de validação se necessário
                
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _token);
                
                Console.WriteLine($"[AuthService] ✓ Sessão restaurada de localStorage");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] ✗ Erro ao restaurar sessão: {ex.Message}");
        }
    }
}
```

### Step 4: Restaurar Sessão no App.razor

```razor
@* Components/App.razor *@
@page "/"
@using Livora_Lite_Blazor.Services
@inject IAuthService AuthService

<Routes />

@code {
    protected override async Task OnInitializedAsync()
    {
        // Restaurar sessão anterior (se existir)
        if (AuthService is AuthService authService)
        {
            await authService.RestoreSessionAsync();
        }
    }
}
```

### Teste P0.1
```
1. Fazer login
2. Abrir DevTools → Local Storage → Verificar "auth_token" e "auth_user"
3. F5 (Refresh page)
4. ✅ Esperado: Permanecer logado, Dashboard funciona
5. DevTools → Application → Clear All → F5
6. ✅ Esperado: Ir para Login page (limpo)
```

---

## P0.3: Rate Limiting (1-2 horas)

### Step 1: Adicionar NuGet

```powershell
cd src/Presentation/Livora-Lite-API
dotnet add package AspNetCoreRateLimit
```

### Step 2: Registrar no Program.cs

```csharp
// ANTES
var builder = WebApplication.CreateBuilder(args);

// DEPOIS - Adicionar após CreateBuilder
builder.Services.AddMemoryCache();
builder.Services.AddInMemoryRateLimiting();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*:/api/auth/login",
            Period = "15m",
            Limit = 5  // 5 attempts per 15 minutes
        }
    };
});
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();

// Aplicar middleware APÓS routing
var app = builder.Build();
app.UseIpRateLimiting();  // ← Adicionar ANTES de UseAuthentication
app.UseAuthentication();
```

### Step 3: Resposta 429 (Optional)

```csharp
// Criar classe de tratamento
public class RateLimitExceptionHandler
{
    public static Func<HttpContext, Task> GetRateLimitExceptionHandler()
    {
        return async context =>
        {
            context.Response.StatusCode = 429;
            context.Response.ContentType = "application/json";
            
            var response = new { 
                success = false, 
                message = "Muitas tentativas de login. Tente novamente em 15 minutos." 
            };
            
            await context.Response.WriteAsJsonAsync(response);
        };
    }
}

// No Program.cs
app.UseExceptionHandler(builder =>
{
    builder.Run(RateLimitExceptionHandler.GetRateLimitExceptionHandler());
});
```

### Teste P0.3
```powershell
# Script PowerShell para testar
for ($i = 1; $i -le 10; $i++) {
    $body = @{
        email = "test@example.com"
        password = "wrong"
    } | ConvertTo-Json
    
    $response = Invoke-WebRequest -Uri "http://localhost:5000/api/auth/login" `
        -Method POST `
        -ContentType "application/json" `
        -Body $body `
        -SkipHttpErrorCheck
    
    Write-Host "Tentativa $i : $($response.StatusCode)"
}
# ✅ Esperado: As 5 primeiras = 400, a 6ª+ = 429
```

---

## P0.2: Refresh Token Implementation (4-6 horas)

### Fase 1: Criar Models de Refresh Token

**Arquivo:** `src/Domain/Entities/RefreshToken.cs`

```csharp
namespace Livora_Lite.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        
        // Navigation
        public virtual User? User { get; set; }
    }
}
```

**Arquivo:** `src/Aplication/DTO/RefreshTokenRequestDTO.cs`

```csharp
namespace Livora_Lite.Application.DTO
{
    public class RefreshTokenRequestDTO
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
```

**Arquivo:** `src/Aplication/DTO/AuthResponseDTO.cs` (Modificado)

```csharp
public class AuthResponseDTO
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? AccessToken { get; set; }        // ← Renomeado de "Token"
    public string? RefreshToken { get; set; }       // ← Novo
    public UserDTO? User { get; set; }
}
```

### Fase 2: Implementar Repository

**Arquivo:** `src/Domain/Interfaces/IRefreshTokenRepository.cs`

```csharp
public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<IEnumerable<RefreshToken>> GetActiveByUserAsync(int userId);
    Task RevokeAsync(int id);
    Task RevokeAllByUserAsync(int userId);
}
```

**Arquivo:** `src/Infrastructure/Persistence/RefreshTokenRepository.cs`

```csharp
public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked);
    }

    public async Task<IEnumerable<RefreshToken>> GetActiveByUserAsync(int userId)
    {
        return await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task RevokeAsync(int id)
    {
        var token = await _context.RefreshTokens.FindAsync(id);
        if (token != null)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RevokeAllByUserAsync(int userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();
        
        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
        }
        
        await _context.SaveChangesAsync();
    }
}
```

### Fase 3: Modificar AuthService (API)

**Arquivo:** `src/Aplication/Services/AuthService.cs`

```csharp
public class AuthService : IAuthService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    
    public AuthService(
        IUserRepository userRepository,
        IConfiguration configuration,
        IMapper mapper,
        IPasswordHasher<User> passwordHasher,
        IRefreshTokenRepository refreshTokenRepository,  // ← Adicionar
        IAppSettingsRepository appSettingsRepository)
    {
        // ... existing code ...
        _refreshTokenRepository = refreshTokenRepository;  // ← Adicionar
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request)
    {
        // ... existing validation code ...

        if (!ValidatePassword(request.Password, user.PasswordHash))
        {
            return new AuthResponseDTO
            {
                Success = false,
                Message = "Senha incorreta."
            };
        }

        Console.WriteLine($"[AuthService] ✓ Senha validada com sucesso");
        Console.WriteLine($"[AuthService] Gerando tokens JWT...");

        // ✅ Gerar ambos os tokens
        var accessToken = await GenerateAccessToken(user);
        var refreshToken = await GenerateRefreshToken(user);

        var userDTO = _mapper.Map<UserDTO>(user);

        Console.WriteLine($"[AuthService] ✓ Login realizado com sucesso");

        return new AuthResponseDTO
        {
            Success = true,
            Message = "Login realizado com sucesso.",
            User = userDTO,
            AccessToken = accessToken,    // ← Mudado
            RefreshToken = refreshToken   // ← Novo
        };
    }

    // ✅ Novo método: Refresh Token
    public async Task<AuthResponseDTO> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            Console.WriteLine($"[AuthService] === REFRESHING TOKEN ===");

            // Validar refresh token
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            
            if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
            {
                Console.WriteLine($"[AuthService] ✗ Refresh token inválido ou expirado");
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "Refresh token inválido ou expirado"
                };
            }

            // Obter usuário
            var user = await _userRepository.GetByIdAsync(storedToken.UserId);
            if (user == null || !user.IsActive)
            {
                Console.WriteLine($"[AuthService] ✗ Usuário não encontrado");
                return new AuthResponseDTO
                {
                    Success = false,
                    Message = "Usuário não encontrado"
                };
            }

            // Gerar novo access token
            var newAccessToken = await GenerateAccessToken(user);
            
            // Optional: Rotar refresh token
            await _refreshTokenRepository.RevokeAsync(storedToken.Id);
            var newRefreshToken = await GenerateRefreshToken(user);

            Console.WriteLine($"[AuthService] ✓ Token renovado com sucesso");

            return new AuthResponseDTO
            {
                Success = true,
                Message = "Token renovado com sucesso",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                User = _mapper.Map<UserDTO>(user)
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] ✗ Erro ao renovar token: {ex.Message}");
            return new AuthResponseDTO
            {
                Success = false,
                Message = "Erro ao renovar token"
            };
        }
    }

    // ✅ Separar geração de access token (curto)
    private async Task<string> GenerateAccessToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured");
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "Livora-Lite-API";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "Livora-Lite-Client";

        var timeoutSetting = await _appSettingsRepository.GetByKeyAsync("SessionTimeoutMinutes");
        int timeoutMinutes = 5; // Default para access token
        if (timeoutSetting != null && int.TryParse(timeoutSetting.Value, out int parsed))
        {
            timeoutMinutes = parsed;
        }

        var roles = !string.IsNullOrEmpty(user.Role)
            ? user.Role.Split(',').Select(r => r.Trim())
            : new[] { "User" };

        var claimsList = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
        };

        foreach (var role in roles)
        {
            claimsList.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claimsList,
            expires: DateTime.UtcNow.AddMinutes(timeoutMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // ✅ Novo: Gerar refresh token (longo, salvar em BD)
    private async Task<string> GenerateRefreshToken(User user)
    {
        var refreshTokenString = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        
        var token = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddDays(7),  // 7 dias
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.AddAsync(token);
        await _refreshTokenRepository.SaveChangesAsync();

        return refreshTokenString;
    }

    // Renomear GenerateJwtToken para GenerateAccessToken (ou manter ambos)
    // ... rest of existing methods ...
}
```

### Fase 4: Adicionar Endpoint de Refresh na API

**Arquivo:** `src/Presentation/Livora-Lite-API/Controllers/AuthController.cs`

```csharp
[HttpPost("refresh")]
[AllowAnonymous]  // ← Sem [Authorize]
public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
{
    Console.WriteLine($"[AuthController] === REFRESH TOKEN REQUEST ===");
    
    if (string.IsNullOrEmpty(request.RefreshToken))
    {
        Console.WriteLine($"[AuthController] ✗ Refresh token vazio");
        return BadRequest(new AuthResponseDTO
        {
            Success = false,
            Message = "Refresh token é obrigatório"
        });
    }

    var response = await _authService.RefreshTokenAsync(request.RefreshToken);

    if (!response.Success)
    {
        Console.WriteLine($"[AuthController] ✗ Refresh falhou: {response.Message}");
        return Unauthorized(response);  // ← 401
    }

    Console.WriteLine($"[AuthController] ✓ Refresh bem-sucedido");
    return Ok(response);
}
```

### Fase 5: Modificar Blazor para Usar Access/Refresh

**Arquivo:** `Services/AuthService.cs` (Blazor)

```csharp
public class AuthService : IAuthService
{
    private const string ACCESS_TOKEN_KEY = "auth_access_token";
    private const string REFRESH_TOKEN_KEY = "auth_refresh_token";
    private const string USER_KEY = "auth_user";
    
    private string? _accessToken;
    private string? _refreshToken;
    private UserDTO? _currentUser;

    public async Task<AuthResponseDTO> LoginAsync(string email, string password)
    {
        var loginRequest = new LoginRequestDTO { Email = email, Password = password };
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();

            if (result != null && result.Success && result.AccessToken != null)
            {
                _accessToken = result.AccessToken;
                _refreshToken = result.RefreshToken;
                _currentUser = result.User;

                // Persistir
                await _localStorage.SetItemAsync(ACCESS_TOKEN_KEY, _accessToken);
                await _localStorage.SetItemAsync(REFRESH_TOKEN_KEY, _refreshToken);
                await _localStorage.SetItemAsync(USER_KEY, result.User);

                // Configurar header
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _accessToken);
            }

            return result ?? new AuthResponseDTO { Success = false };
        }

        return new AuthResponseDTO { Success = false };
    }

    // ✅ Novo: Refresh Token
    public async Task<bool> RefreshAccessTokenAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_refreshToken))
                return false;

            var request = new RefreshTokenRequestDTO { RefreshToken = _refreshToken };
            var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponseDTO>();
                
                if (result?.Success == true && result.AccessToken != null)
                {
                    _accessToken = result.AccessToken;
                    _refreshToken = result.RefreshToken;
                    _currentUser = result.User;

                    // Persistir novos tokens
                    await _localStorage.SetItemAsync(ACCESS_TOKEN_KEY, _accessToken);
                    await _localStorage.SetItemAsync(REFRESH_TOKEN_KEY, _refreshToken);

                    // Atualizar header
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", _accessToken);

                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService] Erro ao renovar token: {ex.Message}");
        }

        return false;
    }

    public string? GetToken()
    {
        return _accessToken;  // Retornar access token
    }

    // ... resto do código ...
}
```

### Fase 6: Modificar Handler para Renovar em 401

**Arquivo:** `Services/AuthorizationDelegatingHandler.cs`

```csharp
public class AuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly IAuthService _authService;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _authService.GetToken();

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        // ✅ NOVO: Se 401, tentar renovar
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            Console.WriteLine($"[AuthHandler] 401 Unauthorized - Tentando renovar token...");

            var authService = (AuthService)_authService;
            var refreshed = await authService.RefreshAccessTokenAsync();

            if (refreshed)
            {
                Console.WriteLine($"[AuthHandler] ✓ Token renovado com sucesso - Retry da requisição");

                // Retry com novo token
                var newToken = _authService.GetToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);

                response = await base.SendAsync(request, cancellationToken);
            }
            else
            {
                Console.WriteLine($"[AuthHandler] ✗ Falha ao renovar - Logout");
                await _authService.LogoutAsync();
            }
        }

        return response;
    }
}
```

### Teste P0.2 - Completo

```
1. Fazer login
   ✅ Esperado: AccessToken (5min) + RefreshToken (7 dias)
   
2. Guardar ambos em localStorage
   
3. Fazer logout e limpar localStorage
   
4. F5 (recarregar)
   ✅ Esperado: Restaurar ambos tokens
   
5. Aguardar 5.5 minutos (ou modificar expiração para 10s para teste)
   
6. Fazer uma requisição ao Dashboard
   ✅ Esperado: 
      - Handler intercepta 401
      - Chama /api/auth/refresh
      - Recebe novo AccessToken
      - Retry da requisição original
      - Dashboard carrega normalmente
```

---

## Checklist de Conclusão

```
FASE 1 - LocalStorage (P0.1)
- [ ] NuGet instalado
- [ ] Registrado em Program.cs
- [ ] AuthService modido (Set/Get/Restore)
- [ ] App.razor chama RestoreSessionAsync
- [ ] Teste: Refresh página → Permanecer logado

FASE 2 - Rate Limiting (P0.3)
- [ ] NuGet instalado
- [ ] Programa registrado
- [ ] Teste: 5 attempts OK, 6ª+ = 429

FASE 3 - Refresh Token (P0.2)
- [ ] 3.1: Models criados
- [ ] 3.2: Repository implementado
- [ ] 3.3: AuthService modificado
- [ ] 3.4: Endpoint /api/auth/refresh
- [ ] 3.5: AuthService Blazor atualizado
- [ ] 3.6: Handler atualizado para retry 401
- [ ] Teste: AccessToken expira, refresh automático

VALIDAÇÃO FINAL
- [ ] Login → Dashboard → Refresh página → Permanecer logado
- [ ] Aguardar 5min → Dashboard continua funcionando
- [ ] Brute force bloqueado após 5 tentativas
- [ ] Logout limpa localStorage
```

---

## Troubleshooting Comum

### "Seed must be at least 8 bytes" (Rate Limiting)
```
Adicionar ao appsettings.json:
{
  "IpRateLimitPolicies": {
    "IpWhitelist": []
  }
}
```

### "RefreshToken não salva em BD"
```
Verificar:
1. DbContext inclui RefreshToken
2. Migration foi executada
3. _refreshTokenRepository foi injetado corretamente
```

### "Handler não está interceptando"
```
Verificar Program.cs:
builder.Services
    .AddHttpClient<DashboardService>()
    .AddHttpMessageHandler<AuthorizationDelegatingHandler>(); // ← Ordem importa!
```

---

**Documentação completa em:** `AVALIACAO_AUTH_BLAZOR_2026.md`
