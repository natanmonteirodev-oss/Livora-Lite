# Guia de Teste - Processo de Login

## Status Atual

✅ **API rodando em**: `http://localhost:5145`
✅ **Blazor rodando em**: `http://localhost:5023`

## Passos para Testar Login

### 1. **Criar um Usuário (Preview)**
1. Acesse: `http://localhost:5023/register`
2. Preencha com os dados:
   - Nome: `João`
   - Sobrenome: `Silva`
   - Email: `joao@teste.com`
   - Senha: `Teste123456`
   - Confirmar Senha: `Teste123456`
3. Clique em "Criar Conta"
4. Verifique se o registro foi bem-sucedido

### 2. **Testar o Login**
1. Acesse: `http://localhost:5023/login`
2. Preencha com:
   - Email: `joao@teste.com`
   - Senha: `Teste123456`
3. Clique em "Entrar"
4. Verifique o comportamento

## 🔍 Debugar o Processo

### Abrir Console de Debug
1. Pressione `F12` no navegador
2. Vá para aba **Console**
3. Você verá logs como:
   ```
   [DEBUG] Iniciando login para: joao@teste.com
   [DEBUG] API Base URL: http://localhost:5145
   [AuthService] Enviando login request para: joao@teste.com
   [AuthService] Status Code: 200 (ou erro)
   ```

### Pontos de Verificação

#### Se vir na Console:
```
[AuthService] Status Code: 401
[AuthService] HttpRequestException: ...
```
**Problema**: Credenciais inválidas ou usuário não existe

#### Se vir:
```
[AuthService] Status Code: 200
[AuthService] Login Success: true
[AuthService] User: joao@teste.com
[AuthService] Token: Recebido
[AuthService] ✓ Login bem-sucedido
```
**Resultado**: Login funcionando! Será redirecionado para Dashboard

#### Se vir:
```
[AuthService] Erro de conexão: ...
```
**Problema**: API não está respondendo na porta 5145

## 🔧 Checklist de Diagnóstico

### Se o login não funciona:

- [ ] Verifique a porta da API (deve ser 5145)
- [ ] Verifique a porta do Blazor (deve ser 5023)
- [ ] Abra F12 e veja os logs exatos
- [ ] Verifique se a API está realmente rodando:
  - Abra PowerShell
  - Execute: `Get-NetTCPConnection -LocalPort 5145 -ErrorAction SilentlyContinue`
  - Deve mostrar LISTENING
- [ ] Verifique se o usuário foi criado:
  - O banco está em: `c:\Users\natan\repos\Livora-Lite\temporary-local-database-for-test\livora.db`
  - Use DB Browser for SQLite para verificar tabela `Users`
- [ ] Verifique CORS na API:
  - A configuração Allow "AllowAll" deve estar funcionando

## 📊 Fluxo de Dados Esperado

```
1. Usuário preenche formulário
   ↓
2. Click em "Entrar"
   ↓
3. AuthService.LoginAsync() chamado
   ↓
4. POST para http://localhost:5145/api/auth/login
   ↓
5. API verifica email e senha no banco
   ↓
6. Se OK → Gera JWT Token
   ↓
7. Retorna { success: true, token: "...", user: {...} }
   ↓
8. Blazor armazena token em memória
   ↓
9. Redireciona para /dashboard
```

## Possíveis Problemas e Soluções

### Problema: "Erro de conexão"
**Causa**: API não está acessível
**Solução**: 
- Feche e inicie a API novamente
- Verifique firewall
- Verifique se a porta 5145 não está sendo usada por outro programa

### Problema: "Credenciais inválidas"
**Causa**: Email/senha incorretos ou usuário não existeno banco
**Solução**:
- Verifique se registrou o usuário corretamente
- Tente criar novo usuário
- Verifique no banco se o usuário foi salvo

### Problema: Login funciona mas não entra no dashboard
**Causa**: Token não está sendo enviado nas requisições subsequentes
**Solução**:
- Ainda é limitação conhecida (token em memória)
- Próximo passo: persistência em LocalStorage

## 📝 Próximos Passos

1. **Persistência de Token**: Salvar token em LocalStorage
2. **Refresh Token**: Renovar token ao expirar
3. **Interceptor HTTP**: Adicionar token automaticamente em todas as requisições
4. **Route Guards**: Proteger rotas não autenticadas

---

**Data**: 20 de Março de 2026
**Status**: Em Debugging
**Responsável**: Teste de Login do Blazor
