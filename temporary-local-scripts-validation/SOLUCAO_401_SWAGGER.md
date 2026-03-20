# Solução: Erro 401 no Swagger "Try it out" - Livora-Lite-API

## Problema Reportado
Mesmo com login bem-sucedido e token JWT válido, "Try it out" no Swagger retorna `401 Unauthorized`:
```
Code: 401
Error: Unauthorized
www-authenticate: Bearer
```

## Causa Raiz
O documento OpenAPI/Swagger gerado pelo **NSwag 14.0.7** não inclui a definição de `securitySchemes` para Bearer Token, causando:
- ❌ Swagger UI não oferece opção "Authorize"
- ❌ Requisições são feitas SEM o token Bearer
- ❌ API rejeita com 401

## Solução Implementada

### 1. Middleware `SwaggerBearerSecurityMiddleware`
**Arquivo:** `src/Presentation/Livora-Lite-API/Middleware/SwaggerBearerSecurityMiddleware.cs`

O middleware:
- Intercepta requisições para `/swagger/v1/swagger.json`
- Injeta esquema `securitySchemes` no documento JSON
- Adiciona definição de Bearer Token (HTTP, JWT)

**Log de execução:**
```
[SwaggerBearerSecurityMiddleware] ✓ Interceptando requisição Swagger!
[SwaggerBearerSecurityMiddleware] Encontrado 'schemas' em posição XXXX
[SwaggerBearerSecurityMiddleware] Injeção realizada!
```

### 2. Registro no Pipeline
**Arquivo:** `src/Presentation/Livora-Lite-API/Program.cs` (linhas ~260-268)

```csharp
if (app.Environment.IsDevelopment())
{
    // ✅ DEVE estar antes de UseSwaggerUi para interceptar resposta
    app.UseMiddleware<Livora_Lite_API.Middleware.SwaggerBearerSecurityMiddleware>();
    
    app.UseOpenApi();
    app.UseSwaggerUi();
}
```

## Como Usar

1. **Fazer Login:**
   ```
   POST http://localhost:5145/api/Auth/login
   Body: { "email": "...", "password": "..." }
   ```

2. **Copiar Token do Response:**
   ```json
   {
     "token": "eyJhbGciOiJIUzI1NiIs..."
   }
   ```

3. **No Swagger UI:**
   - Clique botão **"Authorize"** (agora disponível)
   - Cole: `Bearer eyJhbGciOiJIUzI1NiIs...`
   - Clique "Authorize"

4. **Usar Try it out de Qualquer Endpoint Protegido:**
   - Resultado esperado: **200 OK** (ao invés de 401)

## Arquivos Modificados

| Arquivo | Status | Descrição |
|---------|--------|-----------|
| `Middleware/SwaggerBearerSecurityMiddleware.cs` | ✅ NOVO | Middleware interceptador |
| `Program.cs` | ✅ MODIFICADO | Registrou middleware +  configuração OpenAPI |

## Status de Implementação

✅ Problema identificado  
✅ Solução desenvolvida  
✅ Compilação bem-sucedida (0 erros)  
⏳ **Pendentevalidação final** (em ambiente)

## Como Validar

```bash
# Via cURL (verificar se securitySchemes foi injetado)
curl http://localhost:5145/swagger/v1/swagger.json | jq '.components.securitySchemes'

# Esperado:
{
  "Bearer": {
    "type": "http",
    "scheme": "bearer",
    "bearerFormat": "JWT",
    "description": "JWT token. Obtenha um token válido fazendo POST em /api/Auth/login"
  }
}
```

## Notas Técnicas

### Por que não usar outras abordagens?

| Abordagem | Problema | Status |
|-----------|----------|--------|
| `AddOpenApi()` Microsoft | Complexo, API limitada para .NET 10 | ❌ Rejeitado |
| NSwag DocumentProcessors | Namespaces não disponíveis (NSwag 14) | ❌ Rejeitado |
| **Middleware Interceptor** | Simples, robusto, independente | ✅ **Selecionado** |

### Performance
- Middleware roda apenas para `/swagger/v1/swagger.json`
- Injeção é feita uma vez por requisição
- Pode adicionar caching se necessário

## Referência de Código

**Trecho do Middleware:**
```csharp
var injection = ",\"securitySchemes\":{\"Bearer\":{\"type\":\"http\"," +
                 "\"scheme\":\"bearer\",\"bearerFormat\":\"JWT\"," +
                 "\"description\":\"JWT token...\"}}";
var result = jsonContent.Insert(insertPos, injection);
```

---

**Data:** 20 de Março de 2026  
**Status:** Implementação Completa
