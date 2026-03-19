# Livora-Lite API Integration Guide

## Overview

Este guia explica como consumir os endpoints da **Livora-Lite-API** no projeto **Livora-Lite-Blazor**.

## Configuração Inicial

### 1. Dependências Instaladas

O projeto Blazor já possui as seguintes dependências configuradas:

- **ProjectReference**: Application, Domain (para DTOs e Interfaces)
- **PackageReference**: 
  - `Microsoft.AspNetCore.Components.WebAssembly`
  - `System.Net.Http.Json` (para serialização JSON)

### 2. Registros de Serviço

No `Program.cs` do Blazor, os seguintes serviços estão configurados:

```csharp
// HttpClient para comunicação com a API
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
});

// Serviço genérico para requisições HTTP
builder.Services.AddScoped<ApiHttpClientService>();

// Todos os serviços da Application (reusados no Blazor)
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
// ... etc
```

### 3. Configuração de URL da API

No `appsettings.json`:

```json
{
  "ApiBaseAddress": "https://localhost:7001"
}
```

**Altere a porta conforme necessário para seu ambiente.**

## Uso da API no Blazor

### Método 1: Usando ApiHttpClientService (Recomendado)

```csharp
@inject ApiHttpClientService ApiClient
@inject ILogger<MyComponent> Logger

<button @onclick="LoadUsers">Carregar Usuários</button>

@if (users != null)
{
    <ul>
        @foreach (var user in users)
        {
            <li>@user.FirstName @user.LastName</li>
        }
    </ul>
}

@code {
    private List<UserDTO>? users;

    protected override async Task OnInitializedAsync()
    {
        await LoadUsers();
    }

    private async Task LoadUsers()
    {
        try
        {
            users = await ApiClient.GetAsync<List<UserDTO>>("api/users");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao carregar usuários");
        }
    }
}
```

### Método 2: Usando Serviços Injetados

```csharp
@inject IUserService UserService
@inject ILogger<MyComponent> Logger

@code {
    protected override async Task OnInitializedAsync()
    {
        var users = await UserService.GetAllUsersAsync();
    }
}
```

## Endpoints Disponíveis na API

### Autenticação
- `POST /api/auth/login` - Login
- `POST /api/auth/register` - Registro

### Usuários
- `GET /api/users` - Listar todos
- `GET /api/users/{id}` - Obter por ID
- `POST /api/users` - Criar
- `PUT /api/users/{id}` - Atualizar
- `DELETE /api/users/{id}` - Deletar

### Propriedades
- `GET /api/properties` - Listar todas
- `GET /api/properties/{id}` - Obter por ID
- `POST /api/properties` - Criar
- `PUT /api/properties/{id}` - Atualizar
- `DELETE /api/properties/{id}` - Deletar

### Inquilinos
- `GET /api/tenants` - Listar todos
- `GET /api/tenants/{id}` - Obter por ID
- `POST /api/tenants` - Criar
- `PUT /api/tenants/{id}` - Atualizar
- `DELETE /api/tenants/{id}` - Deletar

### Contratos
- `GET /api/contracts` - Listar todos
- `GET /api/contracts/{id}` - Obter por ID
- `POST /api/contracts` - Criar
- `PUT /api/contracts/{id}` - Atualizar
- `DELETE /api/contracts/{id}` - Deletar

### Cobranças
- `GET /api/billings` - Listar todas
- `GET /api/billings/{id}` - Obter por ID
- `POST /api/billings` - Criar
- `PUT /api/billings/{id}` - Atualizar
- `DELETE /api/billings/{id}` - Deletar

### Pagamentos
- `GET /api/payments` - Listar todos
- `GET /api/payments/{id}` - Obter por ID
- `POST /api/payments` - Criar
- `PUT /api/payments/{id}` - Atualizar
- `DELETE /api/payments/{id}` - Deletar

### Manutenção
- `GET /api/maintenancerequests` - Listar todos
- `GET /api/maintenancerequests/{id}` - Obter por ID
- `POST /api/maintenancerequests` - Criar
- `PUT /api/maintenancerequests/{id}` - Atualizar
- `DELETE /api/maintenancerequests/{id}` - Deletar

### Relatórios (Admin/Owner)
- `GET /api/reports/financial` - Relatório Financeiro
- `GET /api/reports/property-performance` - Performance de Propriedades
- `GET /api/reports/contract-analysis` - Análise de Contratos
- `GET /api/reports/maintenance` - Manutenção

### auditoria
- `GET /api/auditlogs` - Listar todos
- `GET /api/auditlogs/{id}` - Obter por ID
- `GET /api/auditlogs/by-entity` - Obter por entidade
- `GET /api/auditlogs/by-user` - Obter por usuário
- `GET /api/auditlogs/export` - Exportar para CSV

### Dashboard
- `GET /api/dashboard/current` - Dashboard do usuário atual (automático)
- `GET /api/dashboard/admin` - Dashboard Admin
- `GET /api/dashboard/owner` - Dashboard Proprietário
- `GET /api/dashboard/tenant` - Dashboard Inquilino

### Tipos e Status (Lookup)
- `GET /api/propertytypes` - Tipos de Propriedade
- `GET /api/propertystatuses` - Status de Propriedade
- `GET /api/tenantstatuses` - Status de Inquilino
- `GET /api/contractstatuses` - Status de Contrato
- `GET /api/billingstatuses` - Status de Cobrança
- `GET /api/paymentmethods` - Métodos de Pagamento

## DTOs Disponíveis

Todos os DTOs estão disponíveis no projeto Application e podem ser importados:

```csharp
using Livora_Lite.Application.DTO;

// Exemplos
var user = new UserDTO { };
var property = new PropertyDTO { };
var contract = new ContractRequestDTO { };
```

## Autenticação

### Fluxo de Autenticação

1. Fazer login na API:

```csharp
var loginRequest = new LoginRequestDTO 
{ 
    Email = "usuario@example.com", 
    Password = "senha123" 
};

var response = await ApiClient.PostAsync<AuthResponseDTO>("api/auth/login", loginRequest);

if (response != null)
{
    ApiClient.SetAuthToken(response.Token);
    // Armazenar token no localStorage
}
```

2. Usar o token em requisições subsequentes:

```csharp
ApiClient.SetAuthToken(token);
```

3. Limpar token quando fazer logout:

```csharp
ApiClient.ClearAuthToken();
```

## Tratamento de Erros

```csharp
try
{
    var result = await ApiClient.GetAsync<List<PropertyDTO>>("api/properties");
}
catch (HttpRequestException ex)
{
    Logger.LogError(ex, "Erro na requisição HTTP");
}
catch (Exception ex)
{
    Logger.LogError(ex, "Erro desconhecido");
}
```

## Exemplo Completo: Página de Propriedades

```csharp
@page "/properties"
@inject ApiHttpClientService ApiClient
@inject ILogger<Properties> Logger

<h1>Propriedades</h1>

@if (properties == null)
{
    <p>Carregando...</p>
}
else if (properties.Count == 0)
{
    <p>Nenhuma propriedade encontrada.</p>
}
else
{
    <table class="table">
        <thead>
            <tr>
                <th>Nome</th>
                <th>Endereço</th>
                <th>Status</th>
                <th>Ações</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var property in properties)
            {
                <tr>
                    <td>@property.Name</td>
                    <td>@property.Address?.Street</td>
                    <td>@property.PropertyStatus?.Name</td>
                    <td>
                        <button @onclick="() => EditProperty(property.Id)">Editar</button>
                        <button @onclick="() => DeleteProperty(property.Id)">Deletar</button>
                    </td>
                </tr>
            }
        </tbody>
    </table>
}

@code {
    private List<PropertyDTO>? properties;

    protected override async Task OnInitializedAsync()
    {
        await LoadProperties();
    }

    private async Task LoadProperties()
    {
        try
        {
            properties = await ApiClient.GetAsync<List<PropertyDTO>>("api/properties");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao carregar propriedades");
        }
    }

    private async Task DeleteProperty(int id)
    {
        var success = await ApiClient.DeleteAsync($"api/properties/{id}");
        if (success)
        {
            await LoadProperties();
        }
    }

    private void EditProperty(int id)
    {
        // Implementar navegação para página de edição
    }
}
```

## Melhorias Recomendadas

1. **State Management**: Considere usar um serviço centralizado de state (ex: Fluxor)
2. **Caching**: Implemente cache para dados que não mudam frequentemente
3. **Paginação**: Adicione suporte a paginação nos endpoints GET
4. **Filtros**: Implemente filtros avançados nos componentes
5. **Validação**: Valide dados no lado do cliente antes de enviar

## Troubleshooting

### CORS Issue
Se receber erro de CORS, verifique:
- A API está rodando na URL correta
- A API tem CORS configurado (já está no Program.cs da API)
- A URL no `appsettings.json` está correta

### Autenticação Falha
- Verifique se o token está sendo enviado corretamente
- Confirme que o token não expirou
- Verifique os Claims do token

### LocalStorage para Token
Para armazenar o token persistentemente:

```csharp
// Instale: MudBlazor ou Blazored.LocalStorage
@inject Blazored.LocalStorage.LocalStorageService localStorage

// Salvando
await localStorage.SetItemAsync("token", response.Token);

// Recuperando
var token = await localStorage.GetItemAsync<string>("token");
```

## Suporte

Para dúvidas ou problemas, consulte:
- [API Documentation](../Livora-Lite-API/README.md)
- [Application Layer](../../Aplication/README.md)
- [Domain Models](../../Domain/README.md)
