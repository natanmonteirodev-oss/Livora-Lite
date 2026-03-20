# 🎯 Quick Reference - Auth Challenge & Response Flow

## 1️⃣ Fluxo Atual (Com Problemas)

```
┌─────────────────────────────────────────────────────────────────────┐
│ CLIENTE (Blazor)                                                    │
├─────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  USER ACTION: Click "Login Button"                                  │
│       ↓                                                              │
│  Form.Email = "user@test.com"                                       │
│  Form.Password = "senha123"                                         │
│       ↓                                                              │
│  AuthService.LoginAsync(email, password)                            │
│       ├─ POST /api/auth/login (body: credentials)                   │
│       │                                                              │
│       │  ┌──────────────────────────────────────────────┐           │
│       │  │ SERVIDOR (API)                               │           │
│       │  ├──────────────────────────────────────────────┤           │
│       │  │ 1. Valida email/senha                         │           │
│       │  │ 2. Procura usuário no BD                      │           │
│       │  │ 3. Verifica password hash                     │           │
│       │  │ 4. GenerateJwtToken(user)                     │           │
│       │  │    ├─ Cria Claims:                            │           │
│       │  │    │  - sub: user.Id                          │           │
│       │  │    │  - email: user.Email                     │           │
│       │  │    │  - role: user.Role                       │           │
│       │  │    │  - jti: guid-random                      │           │
│       │  │    │  - exp: now.AddMinutes(5)                │           │
│       │  │    └─ Assina com HS256                        │           │
│       │  │ 5. Retorna JsonResponse:                      │           │
│       │  │    {                                           │           │
│       │  │      success: true,                            │           │
│       │  │      token: "eyJhbGciOi...",  ← JWT           │           │
│       │  │      user: { id, email, role }                │           │
│       │  │    }                                           │           │
│       │  │                                                │           │
│       │  └── (200 OK) ──────────────────────┐           │           │
│       │                                       │           │           │
│       ├───────────────────────────────────────┤           │           │
│       │  ← Response received                  │           │           │
│       │    AuthResponseDTO result             │           │           │
│       │                                       │           │           │
│       │  ✅ success==true && token!=null      │           │           │
│       │       ↓                               │           │           │
│       │  _token = result.token  ← MEMÓRIA!   │❌ PROBLEMA│           │
│       │  _currentUser = result.user           │           │           │
│       │       ↓                               │           │           │
│       │  Header Authorization =               │           │           │
│       │    "Bearer {_token}"                  │           │           │
│       │       ↓                               │           │           │
│       │  NavigateTo("/dashboard") ✅           │           │           │
│                                               │           │           │
│  ┌─────────────────────────────────────────┘           │           │
│  │                                                      │           │
│  ├─ USER ACTION: Click "View Orders"                    │           │
│  │       ↓                                              │           │
│  │  DashboardService.GetOrdersAsync()                  │           │
│  │    (HttpClient com AuthorizationDelegatingHandler) │           │
│  │       ├─ GET /api/dashboard/orders                  │           │
│  │       │  Header: "Authorization: Bearer {token}"    │           │
│  │       │             ↑ Injetado automaticamente ✅    │           │
│  │       │                                              │           │
│  │       │  ┌──────────────────────────────────────┐   │           │
│  │       │  │ SERVIDOR (API)                       │   │           │
│  │       │  ├──────────────────────────────────────┤   │           │
│  │       │  │ [Authorize] middleware               │   │           │
│  │       │  │  ├─ Valida token (assinatura)       │   │           │
│  │       │  │  ├─ Verifica exp (now < exp) ✅      │   │           │
│  │       │  │  ├─ Extrai claims                   │   │           │
│  │       │  │  └─ HttpContext.User populated       │   │           │
│  │       │  └─ (200 OK) → {orders: [...]}         │   │           │
│  │       │                                         │   │           │
│  │       ├─ Response recebida                      │   │           │
│  │       └─ Dashboard mostra dados ✅               │   │           │
│  │                                                 │   │           │
│  └─ USER ACTION: Pressiona F5 (Refresh Page) ❌      │   │           │
│  │       ↓                                         │   │           │
│  │  App.razor reinicializa                        │   │           │
│  │  AuthService.IsAuthenticated()?                │   │           │
│  │    return !string.IsNullOrEmpty(_token)        │   │           │
│  │           && _currentUser != null              │   │           │
│  │                                                 │   │           │
│  │  ❌ PROBLEMA: _token = null (foi limpo!)       │   │           │
│  │     Dashboard vê IsAuthenticated() = false      │   │           │
│  │     Redireciona para /login                    │   │           │
│  │     USUÁRIO FOI DESLOGADO! 🔴                  │   │           │
│                                                   │   │           │
└─────────────────────────────────────────────────────────────────────┘

PROBLEMAS IDENTIFICADOS:
❌ 1. Token em memória desaparece em F5
❌ 2. Sem refresh token → expira em 5min
❌ 3. 401 não trata automaticamente
```

---

## 2️⃣ Fluxo Proposto (Com Correções P0)

```
┌────────────────────────────────────────────────────────────────────────┐
│ CLIENTE (Blazor)                                                       │
├────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│ USER ACTION: Click "Login Button"                                      │
│      ↓                                                                  │
│ AuthService.LoginAsync(email, password)                                │
│      ├─ POST /api/auth/login                                           │
│      │                                                                  │
│      │  ┌────────────────────────────────────────────────┐            │
│      │  │ SERVIDOR (API)                                 │            │
│      │  ├────────────────────────────────────────────────┤            │
│      │  │ 1. Valida credenciais                          │            │
│      │  │ 2. GenerateAccessToken(user)  ← 5 min         │            │
│      │  │ 3. GenerateRefreshToken(user) ← 7 dias        │            │
│      │  │ 4. Salva RefreshToken em BD                   │            │
│      │  │ 5. Retorna ambos tokens                        │            │
│      │  │    {                                            │            │
│      │  │      accessToken: "...",  ← Curto              │            │
│      │  │      refreshToken: "...", ← Longo + no BD      │            │
│      │  │      user: {...}                               │            │
│      │  │    }                                            │            │
│      │  └─ (200 OK) ───────────────────────┐            │            │
│      │                                      │            │            │
│      ├──────────────────────────────────────┤            │            │
│      │ ← Response received                  │            │            │
│      │   _accessToken = result.accessToken  │            │            │
│      │   _refreshToken = result.refreshToken│            │            │
│      │   _currentUser = result.user         │            │            │
│      │        ↓                             │            │            │
│      │   await _localStorage.SetItemAsync(  │            │            │
│      │     "auth_access_token",             │            │            │
│      │     _accessToken)  ✅ PERSISTIR      │            │            │
│      │   await _localStorage.SetItemAsync(  │            │            │
│      │     "auth_refresh_token",            │            │            │
│      │     _refreshToken)  ✅ PERSISTIR     │            │            │
│      │        ↓                             │            │            │
│      │   Header Authorization =             │            │            │
│      │     "Bearer {_accessToken}"          │            │            │
│      │        ↓                             │            │            │
│      │   NavigateTo("/dashboard")           │            │            │
│                                             │            │            │
│ ┌────────────────────────────────────────┘            │            │
│ │                                                       │            │
│ ├─ USER ACTION: Pressiona F5 (Refresh Page) ✅         │            │
│ │      ↓                                               │            │
│ │ App.razor OnInitialized:                            │            │
│ │  AuthService.RestoreSessionAsync()                  │            │
│ │    ├─ token = localStorage.GetItem("auth_access")   │            │
│ │    ├─ refreshToken = localStorage.GetItem("...")    │            │
│ │    ├─ _accessToken = token                          │            │
│ │    ├─ _refreshToken = refreshToken                  │            │
│ │    └─ Header Authorization = "Bearer {token}"       │            │
│ │         ↓ (IsAuthenticated() = true) ✅             │            │
│ │  Dashboard carrega normalmente 🟢                    │            │
│                                                        │            │
│ ├─ Passam 4.5 minutos de inatividade                   │            │
│ │      ↓                                               │            │
│ │ USER ACTION: Click "View Orders"                    │            │
│ │      ↓                                               │            │
│ │ GET /api/dashboard/orders                           │            │
│ │  Header: "Authorization: Bearer {access_token}"     │            │
│ │         (AccessToken agora expirado!) ❌             │            │
│ │                                                      │            │
│ │  ┌────────────────────────────────────────┐         │            │
│ │  │ SERVIDOR (API)                         │         │            │
│ │  ├────────────────────────────────────────┤         │            │
│ │  │ [Authorize] middleware:                │         │            │
│ │  │  ├─ Valida token                       │         │            │
│ │  │  ├─ exp: validate (now > exp)          │         │            │
│ │  │  └─ ✗ Token expirado → 401             │         │            │
│ │  │ Response: 401 Unauthorized              │         │            │
│ │  └─────────────────────────────┐          │         │            │
│ │                                │          │         │            │
│ ├────────────────────────────────┤          │         │            │
│ │ ← 401 Response recebida        │          │         │            │
│ │                                │          │         │            │
│ │ AuthorizationDelegatingHandler │          │         │            │
│ │ INTERCEPTA 401:                │          │         │            │
│ │   ├─ Verifica _refreshToken    │          │         │            │
│ │   ├─ POST /api/auth/refresh    │          │         │            │
│ │   │  Body: {                   │          │         │            │
│ │   │    refreshToken: "..."     │          │         │            │
│ │   │  }                          │          │         │            │
│ │   │                             │          │         │            │
│ │   │  ┌────────────────────────┐ │         │         │            │
│ │   │  │ SERVIDOR (API)         │ │         │         │            │
│ │   │  ├────────────────────────┤ │         │         │            │
│ │   │  │ [POST /api/auth/refresh]│ │         │         │            │
│ │   │  │ ├─ Valida refresh token  │ │         │         │            │
│ │   │  │ ├─ Verifica em BD se OK  │ │         │         │            │
│ │   │  │ ├─ GenerateAssToken(user)│ │         │         │            │
│ │   │  │ ├─ Revoga antigo refresh │ │         │         │            │
│ │   │  │ ├─ GenerateRfreshToken() │ │         │         │            │
│ │   │  │ └─ (200 OK) → novos tokens│ │         │         │            │
│ │   │  └──────────────┬──────────┘ │         │         │            │
│ │   │                 │            │         │         │            │
│ │   ├─ Response recebida:          │         │         │            │
│ │   │  _accessToken = novo         │         │         │            │
│ │   │  _refreshToken = novo        │         │         │            │
│ │   │  UpdateLocalStorage()        │         │         │            │
│ │   │  UpdateHeader()              │         │         │            │
│ │   │  RetryRequest() ✅            │         │         │            │
│ │   │  (Re-envia GET /api/... com novo token)│         │            │
│ │   │       ↓                      │         │         │            │
│ │   │  ✅ (200 OK) → dados carregam│         │         │            │
│ │   │                              │         │         │            │
│ │   └─ Usuário continua usando 🟢 │         │         │            │
│                                     │         │         │            │
│ ├─ Tentativa de Brute Force         │         │         │            │
│ │  for (i=1; i<=10; i++) {         │         │         │            │
│ │    LoginAsync("admin", "wrong") │         │         │            │
│ │  }                               │         │         │            │
│ │       ↓                          │         │         │            │
│ │  [Rate Limiting] AspNetRateLimit │         │         │            │
│ │  ├─ Tentativa 1-5: 400 erro     │         │         │            │
│ │  ├─ Tentativa 6+: 429 Too Many   │         │         │            │
│ │  │  Requests                    │         │         │            │
│ │  └─ Bloqueado por 15 minutos     │         │         │            │
│ │     ✅ Seguro contra brute force  │         │         │            │
│                                     │         │         │            │
└─────────────────────────────────────────────────────────────────────────┘

MELHORIAS IMPLEMENTADAS:
✅ 1. Token persistido em localStorage → Sobrevive F5
✅ 2. Refresh token renova automaticamente
✅ 3. 401 é interceptado e trata transparente
✅ 4. Brute force bloqueado com 429
```

---

## 3️⃣ Comparação: Antes vs Depois

| Cenário | Antes ❌ | Depois ✅ |
|---------|---------|---------|
| **User faz login** | 5 min exp | 5 min access + 7 dias refresh |
| **Recarrega página (F5)** | DESLOGADO | Continua logado (localStorage) |
| **Espera 5+ minutos sem ação** | 401 - Logout | Silencioso refresh → OK |
| **Muda para outra aba** | Nova instância (não compartilha) | Mesmo localStorage (TODO: sync) |
| **Brute force 100x login** | Permite tudo ⚠️ | Bloqueia após 5 (429) ✅ |
| **Logout** | Limpa _token | Limpa tokens + localStorage |

---

## 4️⃣ Diagrama de Estados - Sessão do Usuário

```
                        ┌─────────────────────┐
                        │   Não Autenticado   │
                        │   (Initial State)   │
                        └──────────┬──────────┘
                                   │
                                   │ LoginAsync(email, pwd)
                                   ↓
                        ┌─────────────────────┐
                        │  Credenciais OK?    │
                        └──┬──────────────┬───┘
                    SIM ────┤          NÃO │
                            │             └──→ Permanecer em "Não Autenticado"
                            ↓
                   ┌────────────────────┐
                   │  Gerar Tokens      │
                   │ • AccessToken(5m)  │
                   │ • RefreshToken(7d) │
                   │ Salvar em BD       │
                   └────────┬───────────┘
                            │
                            ↓
                   ┌────────────────────┐
                   │  Armazenar localStorage
                   │  • access_token    │
                   │  • refresh_token   │
                   │  • user_data       │
                   └────────┬───────────┘
                            │
                            ↓
                   ┌────────────────────┐
                   │  Autenticado 🟢    │  ← Dashboard carrega
                   │  (Sessão Válida)   │
                   └────────┬───────────┘
                   ┌────────┴───────┬───────────┬──────────┐
                   │                │           │          │
        ┌──────────┴───────────┐   │           │          │
        │ USER ACTION: LogoutAsync   │           │          │
        │ • Limpa localStorage      │           │          │
        │ • Limpa _accessToken      │           │          │
        │                           │           │          │
        │ Vai para "Não Autenticado"│           │          │
        └───────────────────────┘   │           │          │
                                    │           │          │
                    ┌───────────────┴─────┐     │          │
                    │ 4.5 min passou sem   │     │          │
                    │ requisição           │     │          │
                    │ + User clica em      │     │          │
                    │   Dashboard button   │     │          │
                    │                      │     │          │
                    │ ✗ AccessToken expirou│     │          │
                    └──────────┬───────────┘     │          │
                               │                 │          │
                               ↓                 │          │
                    ┌────────────────────────┐   │          │
                    │  401 Unauthorized      │   │          │
                    │  [Interceptado by      │   │          │
                    │   Handler]             │   │          │
                    └──────────┬─────────────┘   │          │
                               │                 │          │
                               ├─ Tem RefreshToken? ✓ │ ✗
                               │                 │     │    │
                           SIM │               NÃO    │    │
                               ↓                 ↓    ↓    │
                    ┌────────────────────┐  Logout   │    │
                    │  RefreshTokenAsync  │          │    │
                    │  POST /api/auth/retry          │    │
                    │                     │          │    │
                    │  Válido em BD?      │          │    │
                    └──┬─────────────┬────┘          │    │
                   SIM │           NÃO               │    │
                       ↓             └──────┬────────┼────┘
                    ┌──────────────┐       │        │
                    │  Novos Tokens │   Logout    │
                    │  Atualizar    │              │
                    │  localStorage │              │
                    │  Retry Request│              │
                    └──────┬───────┘              │
                           │                      │
                           └────────┬─────────────┴────┐
                                    │                  │
                                    ↓                  ↓
                           ┌────────────────┐ ┌──────────────────┐
                           │ Autenticado 🟢 │ │ Não Autenticado  │
                           │ (Session OK)   │ │  → /login page   │
                           └────────────────┘ └──────────────────┘
```

---

## 5️⃣ Estrutura de JWT (Access Token vs Refresh Token)

```
ACCESS TOKEN (5 minutos)
═════════════════════════
Header:
{
  "alg": "HS256",
  "typ": "JWT"
}

Payload:
{
  "sub": "1",                          ← User ID
  "email": "user@example.com",
  "unique_name": "João Silva",
  "role": ["User", "Admin"],
  "jti": "a1b2c3d4",                   ← JWT ID (previne replay)
  "exp": 1742727900,                   ← EXPIRA RÁPIDO (5 min)
  "iat": 1742727600,
  "iss": "Livora-Lite-API",
  "aud": "Livora-Lite-Client"
}

Signature: HMACSHA256(header+payload, secret)


REFRESH TOKEN (7 dias)
════════════════════════
Apenas um string aleatório:
  "a1b2c3d4e5f6g7h8j9k0l1m2n3o4p5q6r7s8t9u0"
  
Salvo em BD com:
  • UserId: 1
  • Token: a1b2c3d4...
  • ExpiresAt: +7 dias
  • CreatedAt: agora
  • IsRevoked: false
  • RevokedAt: null

NÃO É UM JWT! Apenas um string.
Validação: Procura em BD se existe, válido e não revogado.
```

---

## 6️⃣ Checklist - O que funciona agora E O que não

### ✅ Já Implementado
- [x] Login com email/senha
- [x] JWT geração correta (HS256, claims)
- [x] Token injetado automaticamente em requisições
- [x] Proteção de páginas (ProtectedComponent)
- [x] Logout (limpa memória)
- [x] Logging excelente (timestamps, cores)
- [x] Validação de expiração (ClockSkew=30s)

### ❌ Falta Implementar (P0 - Crítico)
- [ ] LocalStorage para persistência
- [ ] Refresh Token (7 dias)
- [ ] Rate Limiting (brute force)

### ⚠️ Recomendado Depois (P1-P2)
- [ ] Sincronização multi-aba (StorageEvent)
- [ ] Audit logging em BD
- [ ] CORS restrito
- [ ] 2FA (MFA)
- [ ] Certificate pinning (mobile)
- [ ] Token rotation automático

---

## 7️⃣ URLs e Endpoints Críticos

### 🔒 Endpoints de Auth (Públicos - sem [Authorize])
```
POST /api/auth/login
  Request: { email, password }
  Response: { success, message, accessToken, refreshToken, user }

POST /api/auth/register
  Request: { firstName, lastName, email, password, confirmPassword }
  Response: { success, message, accessToken, refreshToken, user }

POST /api/auth/refresh
  Request: { refreshToken }
  Response: { success, message, accessToken, refreshToken, user }
```

### 🛡️ Endpoints Protegidos (requer [Authorize])
```
GET /api/dashboard/admin       [Authorize(Roles = "Admin")]
GET /api/dashboard/owner/:id   [Authorize(Roles = "Owner")]
GET /api/dashboard/tenant/:id  [Authorize(Roles = "Tenant")]
GET /api/users                 [Authorize]
GET /api/properties            [Authorize]
```

---

## 8️⃣ Headers HTTP Críticos

### Request (Cliente → Servidor)
```
POST /api/auth/login
Content-Type: application/json
Body: {"email": "...", "password": "..."}

GET /api/dashboard
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

### Response (Servidor → Cliente)
```
POST /api/auth/login
200 OK
Content-Type: application/json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "a1b2c3d4e5f6...",
  "user": {...}
}

GET /api/dashboard
401 Unauthorized
WWW-Authenticate: Bearer realm="api", error="invalid_token"
```

---

## Resumo: Onde está cada componente

```
┌─────────────────────────────────────┐
│ SRC/PRESENTATION/LIVORA-LITE-BLAZOR │
├─────────────────────────────────────┤
│ • Program.cs                        │ ← Registra DI
│ • Services/                         │
│   ├─ AuthService.cs   ← CORE       │
│   ├─ IAuthService.cs               │
│   └─ AuthorizationDelegatingHandler│ ← Intercepta 401
│ • Components/                       │
│   ├─ Pages/Login.razor  ← UI       │
│   ├─ Pages/Dashboard.razor         │
│   └─ ProtectedComponent.cs         │
│ • appsettings.json                 │
│   └─ ApiSettings:BaseUrl           │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ SRC/APPLICATION                     │
├─────────────────────────────────────┤
│ • Services/                         │
│   ├─ AuthService.cs   ← Lógica     │
│   └─ IAuthService.cs               │
│ • DTO/                              │
│   ├─ AuthResponseDTO               │
│   ├─ LoginRequestDTO               │
│   └─ RefreshTokenRequestDTO (TODO) │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ SRC/INFRASTRUCTURE/PERSISTENCE      │
├─────────────────────────────────────┤
│ • RefreshTokenRepository (TODO)     │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ SRC/DOMAIN                          │
├─────────────────────────────────────┤
│ • Entities/                         │
│   ├─ User.cs                       │
│   └─ RefreshToken.cs (TODO)        │
│ • Interfaces/                       │
│   └─ IRefreshTokenRepository (TODO) │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ SRC/PRESENTATION/LIVORA-LITE-API    │
├─────────────────────────────────────┤
│ • Program.cs                        │
│   ├─ JWT Configuration              │
│   └─ Rate Limiting (TODO)           │
│ • Controllers/AuthController.cs     │
│   ├─ POST /api/auth/login           │
│   ├─ POST /api/auth/register        │
│   └─ POST /api/auth/refresh (TODO)  │
│ • Middleware/                       │
│   └─ AuthenticationLoggingMiddleware│
└─────────────────────────────────────┘
```

---

**Documentação completa:** [AVALIACAO_AUTH_BLAZOR_2026.md](AVALIACAO_AUTH_BLAZOR_2026.md)  
**Guia de Implementação:** [IMPLEMENTACAO_AUTH_CORRECOES.md](IMPLEMENTACAO_AUTH_CORRECOES.md)  
**Data:** 20.03.2026
