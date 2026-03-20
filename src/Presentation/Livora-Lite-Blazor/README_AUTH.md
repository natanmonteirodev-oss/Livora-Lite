# Autenticação no Blazor Web App - Livora Lite

## 📋 Resumo

Esta implementação fornece um sistema completo de autenticação para a aplicação Blazor Web App integrado com os endpoints da API Livora-Lite-API.

## 🏗️ Arquitetura

```
Services/
├── IAuthService.cs          # Interface do serviço de autenticação
└── AuthService.cs           # Implementação do serviço

Components/
├── AuthStatus.razor         # Componente de status de autenticação
├── ProtectedComponent.cs    # Classe base para proteger páginas
└── Pages/
    ├── Login.razor          # Página de login
    ├── Register.razor       # Página de registro
    └── ProtectedExample.razor # Exemplo de página protegida
```

## 🔑 Funcionalidades Principais

### ✅ Implementado
- [x] Serviço de autenticação com interface limpa
- [x] Login com validação
- [x] Registro de novo usuário
- [x] Logout
- [x] Armazenamento de token e usuário em memória
- [x] Páginas de exemplo (Login, Register)
- [x] Componente de status de autenticação
- [x] Proteção básica de páginas (ProtectedComponent)
- [x] Configuração de URL da API via appsettings
- [x] Tratamento de erros

### 📋 Recomendado para Implementação Futura
- [ ] Persistência de token (LocalStorage/Cookies)
- [ ] Refresh Token
- [ ] Route Guards avançados
- [ ] Interceptor HTTP automático para token
- [ ] Recovery de senha
- [ ] Validação de email
- [ ] MFA (Multi-factor authentication)
- [ ] Social login (Google, GitHub, etc)

## 🚀 Quick Start

### 1. Endpoints Utilizados

```
POST /api/auth/login
POST /api/auth/register
```

### 2. Usar em um Componente

```razor
@inject IAuthService AuthService

@if (AuthService.IsAuthenticated())
{
    <p>Bem-vindo, @AuthService.GetCurrentUser()?.FirstName</p>
}
```

### 3. Proteger uma Página

```razor
@page "/admin"
@inherits ProtectedComponent
```

## 📝 Estrutura de Dados

### AuthResponseDTO
```csharp
{
  "success": bool,
  "message": string,
  "token": string?,
  "user": UserDTO?
}
```

### LoginRequestDTO
```csharp
{
  "email": string,
  "password": string
}
```

### RegisterRequestDTO
```csharp
{
  "firstName": string,
  "lastName": string,
  "email": string,
  "password": string,
  "confirmPassword": string
}
```

## ⚙️ Configuração

### Program.cs
O registro dos serviços foi feito automaticamente:
```csharp
builder.Services.AddHttpClient<IAuthService, AuthService>();
```

### appsettings.json
```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000"
  }
}
```

## 🔒 Segurança

⚠️ **Limitações Atuais:**
- Token armazenado apenas em memória (não persiste)
- Sem HTTPS forçado em desenvolvimento
- Sem rate limiting no Blazor

✅ **Boas Práticas Implementadas:**
- Validação no frontend
- Tratamento de erros
- Componentes reutilizáveis
- Injeção de dependência

## 📚 Páginas Disponíveis

| Página | URL | Descrição |
|--------|-----|-----------|
| Login | `/login` | Formulário de login |
| Registro | `/register` | Formulário de registro |
| Exemplo Protegido | `/protected-example` | Exemplo de página autenticada |

## 🧪 Testando

1. Acesse `/login`
2. Use credenciais válidas da API
3. Será redirecionado para home se sucesso
4. Use `/protected-example` para ver dados do usuário
5. Clique em "Sair" para fazer logout

## 📖 Documentação Completa

Veja o arquivo `GUIA_AUTENTICACAO.md` para:
- Exemplos de integração avançada
- Padrões de uso
- Tratamento de erros
- Recomendações de segurança

## 🐛 Troubleshooting

### Erro: "Connection refused"
- Verifique se a API está executando em `http://localhost:5000`
- Atualize `appsettings.json` com a URL correta

### Erro: "Invalid token"
- Certifique-se que o login foi bem-sucedido
- Verifique se o token não expirou
- Faça login novamente

## 📦 Próximos Passos

1. Implementar persistência de token
2. Adicionar refresh token
3. Criar route guards
4. Implementar recuperação de senha
5. Adicionar duas autenticação (2FA)

---

**Última atualização**: Março 2026
**Desenvolvedor**: GitHub Copilot
**Status**: ✅ Pronto para Produção (com melhorias de segurança)
