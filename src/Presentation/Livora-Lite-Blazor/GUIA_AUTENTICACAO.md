# Guia de Uso - Serviço de Autenticação Blazor

## Visão Geral

O serviço de autenticação foi criado para integrar a aplicação Blazor Web App com os endpoints de autenticação disponibilizados pela API Livora-Lite-API.

## Estrutura Criada

### 1. **IAuthService** (Interface)
- **Arquivo**: `Services/IAuthService.cs`
- Define todos os métodos disponíveis para autenticação

#### Métodos Disponíveis:

```csharp
// Realiza o login
Task<AuthResponseDTO> LoginAsync(string email, string password);

// Registra um novo usuário
Task<AuthResponseDTO> RegisterAsync(string firstName, string lastName, string email, string password, string confirmPassword);

// Realiza logout
Task LogoutAsync();

// Verifica se está autenticado
bool IsAuthenticated();

// Obtém o token JWT
string? GetToken();

// Obtém o usuário autenticado
UserDTO? GetCurrentUser();

// Atualiza o usuário
void SetCurrentUser(UserDTO? user);

// Atualiza o token
void SetToken(string? token);
```

### 2. **AuthService** (Implementação)
- **Arquivo**: `Services/AuthService.cs`
- Implementação concreta do IAuthService
- Faz chamadas HTTP para a API
- Gerencia token e dados do usuário em memória

### 3. **Páginas de Exemplo**

#### Login (`Components/Pages/Login.razor`)
- Página completa de login com validação
- Integrada com o AuthService
- Redireiona para home após sucesso

#### Register (`Components/Pages/Register.razor`)
- Página completa de registro com validação
- Validação de senhas matching
- Requisito de mínimo de 6 caracteres

### 4. **Componente de Status** (`Components/AuthStatus.razor`)
- Componente reutilizável que mostra status de autenticação
- Pode ser inserido em layouts ou outros componentes
- Exibe nome do usuário se autenticado

## Como Usar

### Exemplo 1: Injetar o serviço em um componente

```razor
@page "/exemplo"
@using Livora_Lite_Blazor.Services
@inject IAuthService AuthService

<div>
    @if (AuthService.IsAuthenticated())
    {
        <p>Bem-vindo, @AuthService.GetCurrentUser()?.FirstName</p>
        <button @onclick="Logout">Sair</button>
    }
    else
    {
        <p><a href="/login">Faça login</a></p>
    }
</div>

@code {
    private async Task Logout()
    {
        await AuthService.LogoutAsync();
    }
}
```

### Exemplo 2: Usar em um layout

```razor
@inherits LayoutComponentBase
@using Livora_Lite_Blazor.Services

<div class="navbar">
    <div class="navbar-content">
        <h1>Livora Lite</h1>
        <AuthStatus />
    </div>
</div>

<div class="page-content">
    @Body
</div>
```

### Exemplo 3: Proteger uma página com autenticação

```razor
@page "/dashboard"
@using Livora_Lite_Blazor.Services
@inject IAuthService AuthService
@inject NavigationManager NavigationManager

@if (Initialized)
{
    @if (AuthService.IsAuthenticated())
    {
        <h1>Dashboard</h1>
        <p>Usuário: @AuthService.GetCurrentUser()?.Email</p>
    }
    else
    {
        <p>Você não está autenticado. <a href="/login">Fazer login</a></p>
    }
}

@code {
    private bool Initialized = false;

    protected override async Task OnInitializedAsync()
    {
        if (!AuthService.IsAuthenticated())
        {
            NavigationManager.NavigateTo("/login");
        }
        Initialized = true;
    }
}
```

## Configuração

### 1. **appsettings.json**
A URL base da API está configurada em:
```json
"ApiSettings": {
    "BaseUrl": "http://localhost:5000"
}
```

Atualize para o ambiente apropriado (desenvolvimento, produção, etc).

### 2. **Program.cs**
O serviço foi registrado automaticamente:
```csharp
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService>(sp => sp.GetRequiredService<AuthService>());
```

## Endpoints da API Utilizados

### POST /api/auth/login
**Request:**
```json
{
  "email": "usuario@exemplo.com",
  "password": "senha"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Login realizado com sucesso",
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "user": {
    "id": 1,
    "firstName": "João",
    "lastName": "Silva",
    "email": "usuario@exemplo.com"
  }
}
```

### POST /api/auth/register
**Request:**
```json
{
  "firstName": "João",
  "lastName": "Silva",
  "email": "novo@exemplo.com",
  "password": "senha123",
  "confirmPassword": "senha123"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Usuário registrado com sucesso",
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "user": {
    "id": 1,
    "firstName": "João",
    "lastName": "Silva",
    "email": "novo@exemplo.com"
  }
}
```

## Tratamento de Erros

O AuthService retorna sempre um `AuthResponseDTO` com:
- `Success`: true/false
- `Message`: mensagem descritiva
- `Token`: token JWT (se sucesso)
- `User`: dados do usuário (se sucesso)

**Exemplo de tratamento:**
```razor
@code {
    private async Task Login()
    {
        var response = await AuthService.LoginAsync(email, password);
        
        if (response.Success)
        {
            // Login bem-sucedido
            NavigationManager.NavigateTo("/dashboard");
        }
        else
        {
            // Mostrar erro
            ErrorMessage = response.Message;
        }
    }
}
```

## Persistência de Token

⚠️ **Nota Importante**: Atualmente, o token é armazenado apenas em memória. Para uma solução de produção, recomenda-se:

1. **Local Storage** (não seguro para dados sensíveis)
2. **Session Storage** (seguro apenas em tokens curtos)
3. **Cookies HttpOnly** (mais seguro)
4. **IndexedDB com segurança** (alternativa mais segura)

Para implementar LocalStorage, veja: `IJSRuntime` com `localStorage`

## Próximos Passos Recomendados

1. Implementar persistência de token (LocalStorage/Cookies)
2. Adicionar refresh token para manter sessão ativa
3. Criar componente de proteção de rotas (RouteGuard)
4. Implementar interceptor HTTP para adicionar token automaticamente
5. Adicionar validação pelo servidor antes de aceitar requisições
6. Implementar função de "Lembrar-me" no login
7. Adicionar recuperação de senha

---

**Criado para**: Livora Lite - Blazor Web App
**Data**: Março 2026
