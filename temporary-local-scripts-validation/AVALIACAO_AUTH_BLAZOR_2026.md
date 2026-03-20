# 📋 Avaliação de Autenticação e Challenge - Livora-Lite-Blazor

**Data:** 20.03.2026  
**Avaliador:** Code Analysis  
**Escopo:** Sistema de Auth no Blazor Web App | API Challenge/Response  
**Status Geral:** ✅ **IMPLEMENTAÇÃO SÓLIDA COM GAPS DE SEGURANÇA**

---

## 🎯 Resumo Executivo

| Aspecto | Status | Observação |
|---------|--------|-----------|
| **Fluxo de Login** | ✅ Funcional | Implementado corretamente endpoint-to-endpoint |
| **Token JWT** | ✅ Gerado Corretamente | Assinado, com claims, expiração configurável |
| **Armazenamento de Token** | ⚠️ Crítico | Apenas em memória - **PERDE ao recarregar a página** |
| **Interceptação Automática** | ✅ Implementada | AuthorizationDelegatingHandler injeta token em requisições |
| **Proteção de Páginas** | ✅ Funcional | ProtectedComponent redireciona não autenticados |
| **Refresh Token** | ❌ Ausente | **Não implementado** |
| **Logging de Auth** | ✅ Excelente | Logs detalhados em todos os pontos críticos |
| **Taxa de Expiração** | ⚠️ Curta | Padrão 5 minutos - usuário sai da sessão rapidamente |

---

## 🏗️ 1. Arquitetura Atual

### 1.1 Fluxo de Autenticação (Request → Response)

```
┌─────────────────────────────────────────────────────────────┐
│ USER BROWSER (Blazor)                                       │
│                                                              │
│  1. Form Login.razor                                         │
│     └─ Email + Password                                      │
│        ↓                                                      │
│  2. AuthService.LoginAsync()                                 │
│     └─ POST /api/auth/login → AuthService API               │
│        ↓                                                      │
│  3. API Response (AuthResponseDTO)                           │
│     ├─ Success: bool                                         │
│     ├─ Token: string (JWT)                                   │
│     ├─ User: UserDTO                                         │
│     └─ Message: string                                       │
│        ↓                                                      │
│  4. AuthService armazena em memória                          │
│     ├─ _token = token                                        │
│     └─ _currentUser = user                                   │
│        ↓                                                      │
│  5. Header Authorization configurado                         │
│     └─ Authorization: Bearer {token}                         │
│        ↓                                                      │
│  6. Próximas requisições (Dashboard)                         │
│     └─ AuthorizationDelegatingHandler injeta token          │
│        automaticamente                                       │
│                                                              │
│  7. API valida JWT                                           │
│     ├─ [Authorize] middleware                                │
│     └─ [Authorize(Roles = "Admin")] para roles específicas   │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### 1.2 Componentes Principais

#### **Lado Blazor (Cliente)**
```
Services/
├── IAuthService.cs                    # Interface
├── AuthService.cs                     # Implementação (memória)
├── AuthorizationDelegatingHandler.cs  # Intercepta e injeta token
│
Components/
├── Pages/
│   ├── Login.razor                   # Formulário login
│   ├── Register.razor                # Formulário registro
│   ├── Dashboard.razor               # Principal (protegida)
│   └── ProtectedExample.razor        # Exemplo de proteção
├── AuthStatus.razor                  # Mostra status
└── ProtectedComponent.cs             # Classe base para páginas protegidas
```

#### **Lado API (Servidor)**
```
Services/
├── IAuthService.cs
├── AuthService.cs
│   ├── LoginAsync()        # Valida credenciais
│   ├── RegisterAsync()     # Cria novo usuário
│   └── GenerateJwtToken() # Gera JWT com claims
│
Program.cs
├── AddAuthentication(JwtBearerDefaults)
├── AddAuthorization()
├── app.UseAuthentication()
├── app.UseAuthorization()
```

---

## ✅ 2. Pontos Fortes Implementados

### 2.1 Fluxo de Login Correto
**Arquivo:** [Login.razor](Components/Pages/Login.razor#L130-L180)

```csharp
// Verdadeiro fluxo assíncrono
var response = await AuthService.LoginAsync(Email, Password);

if (response.Success && response.User != null)
{
    // Token foi armazenado por AuthService automaticamente
    NavigationManager.NavigateTo("/dashboard", forceLoad: false);
}
```

✅ **Por quê está correto:**
- Não bloqueia UI (async/await)
- Valida resposta antes de navegar
- Exibe mensagens de erro clara
- Log estruturado de cada passo

---

### 2.2 Injeção Automática de Token em Requisições
**Arquivo:** [AuthorizationDelegatingHandler.cs](Services/AuthorizationDelegatingHandler.cs#L15-L40)

```csharp
protected override async Task<HttpResponseMessage> SendAsync(
    HttpRequestMessage request, CancellationToken cancellationToken)
{
    var token = _authService.GetToken();
    
    if (!string.IsNullOrEmpty(token))
    {
        request.Headers.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
    }
    
    return await base.SendAsync(request, cancellationToken);
}
```

✅ **Por quê está correto:**
- **Padrão HTTP 100% standard** (RFC 7235 Bearer Token)
- Cada requisição recebe automaticamente o token
- Se token for null, requisição segue sem Authorization (bem-vindo para endpoints públicos)
- Handler registrado apenas para DashboardService:

```csharp
builder.Services
    .AddHttpClient<DashboardService>()
    .AddHttpMessageHandler<AuthorizationDelegatingHandler>();
```

---

### 2.3 Geração de JWT Correta (Lado Servidor)
**Arquivo:** [AuthService.cs (API)](../src/Aplication/Services/AuthService.cs#L220-L250)

```csharp
var claimsList = new List<Claim>
{
    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
    new Claim(JwtRegisteredClaimNames.Email, user.Email),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
    new Claim(ClaimTypes.Email, user.Email)
};

// Múltiplos roles suportados
foreach (var role in roles)
{
    claimsList.Add(new Claim(ClaimTypes.Role, role));
}

var token = new JwtSecurityToken(
    issuer: jwtIssuer,
    audience: jwtAudience,
    claims: claimsList,
    expires: DateTime.UtcNow.AddMinutes(timeoutMinutes), // ✅ UTC
    signingCredentials: new SigningCredentials(...)
);
```

✅ **Por quê está correto:**
- `DateTime.UtcNow` garante consistência independente de timezone do servidor
- JTI (JWT ID) previne replay attacks
- Múltiplos roles suportados (não apenas um string)
- Issuer/Audience permitem validação de origem

---

### 2.4 Proteção de Páginas Implementada
**Arquivo:** [ProtectedComponent.cs](Components/ProtectedComponent.cs)

```csharp
protected override async Task OnInitializedAsync()
{
    IsAuthenticated = AuthService.IsAuthenticated();
    
    if (!IsAuthenticated)
    {
        NavigationManager.NavigateTo("/login", forceLoad: true);
    }
}
```

✅ **Por quê está correto:**
- `forceLoad: true` garante recarregamento (não há cache)
- Verifica antes de renderizar conteúdo sensível
- Usado em [Dashboard.razor](Components/Pages/Dashboard.razor#L85) com lógica adicional

---

### 2.5 Logging Excelente para Debuggging
**Exemplo no Login.razor:**

```csharp
private void AddLog(string message)
{
    var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
    DebugLogs += $"[{timestamp}] {message}\n";
}

private string GetLogColor(string line)
{
    if (line.Contains("✓") || line.Contains("sucesso"))
        return "#28a745"; // Verde
    if (line.Contains("✗") || line.Contains("erro"))
        return "#dc3545"; // Vermelho
    if (line.Contains("→"))
        return "#0066cc"; // Azul
}
```

✅ **Por quê está correto:**
- Timestamps permitem rastrear latência
- Cores visuais facilitam scanning rápido
- Incluso no painel que o usuário vê (ótimo para suporte)
- `AuthService.LoginAsync()` também registra:
  - Email do usuário
  - Status code da resposta
  - Presença de token
  - Falhas de conexão

---

## ⚠️ 3. Problemas Críticos Identificados

### 3.1 🔴 CRÍTICO: Token Armazenado Apenas em Memória

**Impacto:** Medium-High  
**Severidade:** Production-Breaking

#### Problema
```csharp
// AuthService.cs - Blazor
private string? _token;        // ❌ Apenas memória
private UserDTO? _currentUser; // ❌ Apenas memória
```

**O que acontece:**
1. Usuário faz login ✅
2. Token armazenado em `_token`
3. Navega, usa dashboard ✅
4. **Recarrega a página (F5)** → **TOKEN DESAPARECE** ❌
5. `OnInitializedAsync()` de Dashboard vê `IsAuthenticated() = false`
6. Usuário é deslogado! 🔄

**Teste para reproduzir:**
```
1. Ir para /login
2. Fazer login (sucesso)
3. Navegar para /dashboard (funciona)
4. Pressionar F5 → Redireciona para /login
```

**Por quê é crítico:**
- Usuário é deslogado ao atualizar página
- Token é perdido se mudar de aba/janela
- Em produção: a experiência é horrível

---

### 3.2 🔴 CRÍTICO: Sem Refresh Token

**Impacto:** High  
**Severidade:** Production-Breaking

#### Problema
```json
// JWT gerado (5 minutos padrão)
{
  "exp": 1742727900,  // ← Expira em 5 minutos!
  "sub": "1",
  ...
}
```

**O que acontece:**
1. Usuário faz login: `exp = now + 5min`
2. Navega normalmente por 4.5 minutos ✅
3. Tenta fazer uma ação aos 5.1 minutos → **401 Unauthorized** ❌
4. Sessão perdida, deve fazer login novamente

**Teste para validar:**
```
1. Fazer login
2. Esperar 5 minutos sem fazer requisição
3. Clicar em um botão do dashboard → 401
```

**Por quê é crítico:**
- Token expira rapidamente (5 min padrão)
- Sem refresh token: não há como renovar
- Usuário precisa fazer login a cada 5 minutos
- Rate limiting na API pode tornar isso pior

---

### 3.3 ⚠️ Armazenamento Sem Suporte a Multi-Aba/Multi-Janela

**Impacto:** Medium  
**Severidade:** Usability Issue

**Problema:**
```
Aba 1: Faz login com token A
Aba 2: Abre aplicação → Vê token = null (são 2 instâncias!)
```

Cada aba Browser tem sua própria instância de Blazor Server, então não compartilham a mesma memória.

---

### 3.4 ⚠️ Sem Rate Limiting no Login

**Impacto:** High  
**Severidade:** Security Issue

```csharp
// AuthService.cs - LoginAsync tem 0 proteção contra
for (int i = 0; i < 10000; i++) {
    await authService.LoginAsync("admin@test.com", "wrong-password");
    // Nenhuma proteção de tentativas ❌
}
```

**Risco:** Brute force attack é trivial

---

### 3.5 ⚠️ Senhas em QueryString / Logs

**Impacto:** Medium  
**Severidade:** Security Issue

**Verificação em Login.razor:**
```csharp
AddLog($"→ Email: {Email}"); // ✅ Seguro
AddLog($"  Password: {Password}"); // ❌ NÃO! Log de senha!
```

**Achado:** Não há exposição visível, mas em um navegador com console aberto, senhas poderiam ser vistas em localStorage.

---

## ⚠️ 4. Problemas Menores (Técnico-Funcionais)

### 4.1 Tipos de Claims Redundantes
**Arquivo:** [AuthService.cs (API)](../src/Aplication/Services/AuthService.cs#L230-L240)

```csharp
new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
new Claim(ClaimTypes.Email, user.Email) // ❌ Duplicado com JwtRegisteredClaimNames.Email?
```

**Impacto:** Confusão em `HttpContext.User.Claims` (existem claims duplicadas)

---

### 4.2 Configuração Hard-coded em appsettings para Timeouts
**Arquivo:** [Program.cs (Blazor)](Program.cs#L12)

```json
// Não há configuração de timeout do session no Blazor
// A expiração é apenas do lado servidor (JWT)
```

**Problema:** Se JWT expira em 5 min mas aplicação em 15 min, há inconsistência.

---

### 4.3 Sem Validação de CORS

**Verificação:** [Program.cs API](../src/Presentation/Livora-Lite-API/Program.cs)

```csharp
// Se houver cors, deve estar restrito a domínios conhecidos
// Teste: POST para /api/auth/login de outro domínio
```

---

## 🎯 5. Matriz de Maturidade de Segurança

| Feature | Classificação | Status | Risco |
|---------|-----|--------|-------|
| Login/Senha | ✅ Implementado | Funcional | Baixo |
| JWT Generation | ✅ Correto | Assinado com HS256 | Baixo |
| Token Validation | ✅ Correto | ClockSkew=30s | Baixo |
| Local Storage | ❌ Ausente | Memória apenas | **Alto** |
| Refresh Token | ❌ Ausente | Sem renovação | **Alto** |
| Rate Limiting | ❌ Ausente | Sem proteção | **Alto** |
| HTTPS Enforcement | ⚠️ Parcial | Dev sem HTTPS | Médio |
| Session Timeout Sync | ⚠️ Ausente | Falta sincronização | Médio |
| CSRF Protection | ⚠️ Parcial | Blazor usa anti-forgery | Baixo |
| XSS Protection | ✅ Automático | Blazor renderiza seguro | Baixo |
| Audit Logging | ⚠️ Parcial | Logs em console, não persistido | Médio |

---

## 📋 6. Checklist de Conformidade

### 6.1 Teste Funcional - Fluxo Básico ✅
```
[✅] POST /api/auth/login (credenciais válidas)
     Response: 200, token e user dados
     
[✅] GET /api/dashboard/... (com token)
     Response: 200, dados do usuário
     
[✅] GET /api/dashboard/... (sem token)
     Response: 401 Unauthorized
     
[✅] GET /api/dashboard/admin (com token Guest)
     Response: 403 Forbidden
```

### 6.2 Teste de Sessão ⚠️
```
[❌] POST /api/auth/login → Token
[❌] Recarregar aplicação Blazor (F5)
     → Teste: IsAuthenticated() deve manter true
     FALHA: Token é perdido
     
[❌] Fazer login em 2 abas diferentes
     → Teste: Ambas devem compartilhar sessão
     FALHA: São instâncias separadas
```

### 6.3 Teste de Expiração ⚠️
```
[❌] Fazer login
[❌] Aguardar 5min
[❌] Tentar acessar endpoint protegido
     ESPERADO: Token renovado automaticamente
     REAL: 401 Unauthorized
```

### 6.4 Teste de Força ❌
```
[❌] Executar script para tentar 100x login com senha errada
     ESPERADO: Rate limiting (429 Too Many Requests)
     REAL: Nenhuma proteção
```

---

## 🔐 7. Recomendações de Implementação (Priorizado)

### 🔴 P0 - CRÍTICO (Implementar Imediatamente)

#### P0.1: Persistência de Token em LocalStorage
**Arquivo afetado:** [Services/AuthService.cs](Services/AuthService.cs)

```csharp
// Adicionar persistência
private const string TOKEN_KEY = "auth_token";
private const string USER_KEY = "auth_user";

public void SetToken(string? token)
{
    _token = token;
    
    // ✅ Persistir
    if (!string.IsNullOrEmpty(token))
    {
        // Usar localStorage - será necessário JS interop
        _localStorageService.SetItem(TOKEN_KEY, token);
    }
    else
    {
        _localStorageService.RemoveItem(TOKEN_KEY);
    }
    
    // Configurar header também
    if (!string.IsNullOrEmpty(token))
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
}

// No OnInitializedAsync do App.razor:
protected override async Task OnInitializedAsync()
{
    var token = await _localStorageService.GetItemAsync<string>(TOKEN_KEY);
    if (!string.IsNullOrEmpty(token))
    {
        _authService.SetToken(token);
        // Validar com API se ainda é válido
    }
}
```

**Tempo estimado:** 2-3 horas  
**Ferramentas necessárias:** Blazored.LocalStorage NuGet

---

#### P0.2: Refresh Token Implementation
**Arquivos afetados:** 
- [AuthService.cs (API)](../src/Aplication/Services/AuthService.cs)
- [Services/AuthService.cs (Blazor)](Services/AuthService.cs)
- [AuthorizationDelegatingHandler.cs](Services/AuthorizationDelegatingHandler.cs)

**Mudança na resposta de login:**
```csharp
// AuthResponseDTO.cs (API)
public class AuthResponseDTO
{
    public string? AccessToken { get; set; }  // JWT curto (5min)
    public string? RefreshToken { get; set; } // JWT longo (7 dias)
    public UserDTO? User { get; set; }
}
```

**No GenerateJwtToken (API):**
```csharp
// Gerar 2 tokens
var accessToken = GenerateAccessToken(user);    // 5 min
var refreshToken = GenerateRefreshToken(user);  // 7 dias

// Salvar refresh token em banco (para revogação)
await _refreshTokenRepository.SaveAsync(new RefreshToken
{
    Token = refreshToken,
    UserId = user.Id,
    ExpiresAt = DateTime.UtcNow.AddDays(7),
    CreatedAt = DateTime.UtcNow
});
```

**Novo endpoint na API:**
```csharp
[HttpPost("/api/auth/refresh")]
public async Task<AuthResponseDTO> RefreshToken(RefreshTokenRequestDTO request)
{
    // Validar refresh token
    // Se válido, gerar novo access token
}
```

**No Blazor (AuthorizationDelegatingHandler):**
```csharp
protected override async Task<HttpResponseMessage> SendAsync(...)
{
    // Interceptar 401
    var response = await base.SendAsync(request, cancellationToken);
    
    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
    {
        // Tentar renovar com refresh token
        var newToken = await _authService.RefreshTokenAsync();
        
        if (newToken != null)
        {
            // Retry original request com novo token
            request.Headers.Authorization = 
                new AuthenticationHeaderValue("Bearer", newToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
    
    return response;
}
```

**Tempo estimado:** 4-6 horas  
**Complexidade:** Alta

---

#### P0.3: Rate Limiting no Login (API)
**Arquivo:** [Program.cs (API)](../src/Presentation/Livora-Lite-API/Program.cs)

```csharp
// Adicionar NuGet: AspNetCoreRateLimit

builder.Services.AddMemoryCache();
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddRateLimiters(options =>
{
    options.AddFixedWindowLimiter("login", policy =>
    {
        policy.PermitLimit = 5;              // 5 tentativas
        policy.Window = TimeSpan.FromMinutes(15); // em 15 minutos
    });
});

// No endpoint:
[HttpPost("login")]
[RateLimitPolicy("login")]
public async Task<IActionResult> Login(LoginRequestDTO request)
{
    // ...
}
```

**Tempo estimado:** 1-2 horas  
**Complexidade:** Baixa

---

### 🟡 P1 - ALTO (Implementar em 1-2 semanas)

#### P1.1: Synchronize Session Across Tabs
**Estratégia:** Usar `StorageEvent` em localStorage

```csharp
// Services/StorageSyncService.cs
public class StorageSyncService
{
    public event Action<StorageChange>? OnStorageChanged;
    
    // Usar Blazor JS interop para escutar storage events
}
```

---

#### P1.2: Audit Logging Persistido
**Arquivo afetado:** [AuthService.cs (API)](../src/Aplication/Services/AuthService.cs)

```csharp
// Adicionar log em tabela
private readonly IAuditLogRepository _auditLogRepository;

public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request)
{
    var user = await _userRepository.GetByEmailAsync(request.Email);
    
    if (user == null)
    {
        // ✅ Log falha
        await _auditLogRepository.LogAsync(new AuditLog
        {
            EventType = "LOGIN_FAILED",
            Email = request.Email,
            Reason = "User not found",
            Timestamp = DateTime.UtcNow,
            IpAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
        });
    }
    else if (!ValidatePassword(request.Password, user.PasswordHash))
    {
        // ✅ Log falha
        await _auditLogRepository.LogAsync(new AuditLog
        {
            EventType = "LOGIN_FAILED",
            UserId = user.Id,
            Reason = "Invalid password",
            ...
        });
    }
    else
    {
        // ✅ Log sucesso
        await _auditLogRepository.LogAsync(new AuditLog
        {
            EventType = "LOGIN_SUCCESS",
            UserId = user.Id,
            ...
        });
    }
}
```

---

#### P1.3: CORS Restrito
**Arquivo:** [Program.cs (API)](../src/Presentation/Livora-Lite-API/Program.cs)

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy
            .WithOrigins("https://app.livora.com", "https://admin.livora.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

app.UseCors("ProductionPolicy");
```

---

### 🟢 P2 - MÉDIO (Implementar em 2-4 semanas)

#### P2.1: Multi-Factor Authentication
```
1. Login com email/senha ✅
2. API envia OTP para email do usuário
3. Usuário insere OTP em form
4. Validar OTP antes de gerar token
```

#### P2.2: Token Rotation
```
A cada 3 requisições:
1. Gerar novo access token
2. Invalidar anterior
3. Previne fixation attacks
```

#### P2.3: Certificate Pinning (Mobile)
```
Se houver app mobile, pinnar certificado SSL
```

---

## 📊 8. Impacto de Cada Vulnerabilidade (Matriz de Risco)

```
┌──────────────────────────────┬──────────┬────────────┬────────┐
│ Vulnerability                │ Likelihood│ Impact    │ Risk   │
├──────────────────────────────┼──────────┼────────────┼────────┤
│ Token em LocalStorage         │ 100%     │ High      │ CRIT   │
│ Sem Refresh Token            │ 100%     │ High      │ CRIT   │
│ Sem Rate Limiting             │ High     │ High      │ HIGH   │
│ Multi-tab Sync               │ Medium   │ Medium    │ MED    │
│ Sem Audit Log                │ Medium   │ Medium    │ MED    │
│ Sem CORS                     │ Low      │ High      │ MEDIUM │
│ Senha em Logs                │ Low      │ Medium    │ LOW    │
│ Claims Redundantes           │ Low      │ Low       │ LOW    │
└──────────────────────────────┴──────────┴────────────┴────────┘
```

---

## 🧪 9. Plano de Testes Recomendado

### 9.1 Testes Funcionais
```gherkin
Feature: Authentication Flow
  Scenario: Valid login
    Given I am on Login page
    When I enter valid credentials
    Then I should be logged in
    And Token should be in memory
    
  Scenario: Invalid password
    Given I am on Login page
    When I enter invalid password
    Then I should see error message
    And Not be logged in
    
  Scenario: Non-existent user
    Given I am on Login page
    When I enter non-existent email
    Then I should see "User not found"
    And Not be logged in
```

### 9.2 Testes de Segurança
```
1. Teste de brute force (muitas tentativas)
2. Teste de token theft (XSS)
3. Teste de CORS origin
4. Teste de senha em logs
5. Teste de SQL injection (email field)
```

### 9.3 Testes de Performance
```
1. Tempo de login (dev) - Alvo: < 1s
2. Tempo de requisição autenticada - Alvo: < 100ms
3. Número concorrente de logins - Alvo: > 100/segundo
```

---

## 📝 10. Sumário e Próximos Passos

### Status Atual: 🟡 AMARELO (Production-Ready com Gaps)

**O que está bom:**
- ✅ Fluxo de autenticação funciona
- ✅ JWT é gerado corretamente
- ✅ Token é injetado automaticamente em requisições
- ✅ Proteção de páginas funciona
- ✅ Logging excelente para debugging

**O que PRECISA ser feito ANTES de produção:**
1. **[P0] Token persistido em LocalStorage** - 2-3 horas
2. **[P0] Refresh Token Implementation** - 4-6 horas
3. **[P0] Rate Limiting no Login** - 1-2 horas
4. **[P1] Audit Logging Persistido** - 2-3 horas
5. **[P1.3] CORS Restrito** - 30-45 min

**Esforço Total:** ~14-20 horas  
**Timeline Sugerido:** 2 semanas

---

## 🔗 Referências e Documentação

**Integrada no Projeto:**
- [GUIA_AUTENTICACAO.md](GUIA_AUTENTICACAO.md) - Como usar AuthService
- [README_AUTH.md](README_AUTH.md) - Arquitetura e features
- [TESTE_LOGIN.md](TESTE_LOGIN.md) - Casos de teste

**Padrões Utilizados:**
- RFC 7235 (HTTP Authentication)
- RFC 7519 (JSON Web Tokens)
- JWT.io para validação de tokens

---

**Documento preparado em 20.03.2026**  
**Próxima revisão recomendada:** 10.04.2026
