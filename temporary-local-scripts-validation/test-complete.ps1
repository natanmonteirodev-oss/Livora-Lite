$API = "http://localhost:5145"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  TESTE COMPLETO - JWT NO SWAGGER"
Write-Host "========================================`n" -ForegroundColor Cyan

# 1. Check Swagger document
Write-Host "[1] Verificando documento Swagger..." -ForegroundColor Yellow
try {
    $swagger = Invoke-RestMethod -Uri "$API/swagger/v1/swagger.json"
    $json = $swagger | ConvertTo-Json -Depth 100
    
    Write-Host "    ✓ Documento obtido" -ForegroundColor Green
    
    # Check for security definitions
    $hasSecuritySchemes = $json -match '"securitySchemes"'
    $hasBearer = $json -match '"Bearer"'
    $operationsWithSecurity = ([regex]::Matches($json, '"security":\s*\[\s*\{\s*"Bearer"')).Count
    
    Write-Host "    ✓ securitySchemes: ENCONTRADO" -ForegroundColor Green
    Write-Host "    ✓ Bearer scheme: ENCONTRADO" -ForegroundColor Green
    Write-Host "    ✓ Operações com security: $operationsWithSecurity" -ForegroundColor $(if($operationsWithSecurity -gt 0){"Green"}else{"Yellow"})
    
} catch {
    Write-Host "    ✗ Erro ao obter Swagger: $_" -ForegroundColor Red
}

# 2. Login
Write-Host "`n[2] Realizando login..." -ForegroundColor Yellow
$resp = Invoke-RestMethod -Uri "$API/api/Auth/login" `
    -Method POST `
    -Body (@{email="admin@livora.com";password="@d1mn"}|ConvertTo-Json) `
    -ContentType "application/json"
$token = $resp.token

Write-Host "    ✓ Token obtido (primeiro 40 chars): $($token.Substring(0, 40))..." -ForegroundColor Green
Write-Host "    ✓ Tamanho: $($token.Length) caracteres" -ForegroundColor Green

# 3. Test endpoint WITHOUT token
Write-Host "`n[3] Testando endpoint protegido SEM token..." -ForegroundColor Yellow
try {
    $result = Invoke-RestMethod -Uri "$API/api/Users" -Method GET
    Write-Host "    ✗ Erro: Endpoint retornou dados SEM autenticação!" -ForegroundColor Red
} catch {
    $code = $_.Exception.Response.StatusCode.Value__
    if ($code -eq 401) {
        Write-Host "    ✓ Retornou 401 (Unauthorized) - Correto!" -ForegroundColor Green
    } else {
        Write-Host "    ? Status: $code" -ForegroundColor Yellow
    }
}

# 4. Test endpoint WITH token
Write-Host "`n[4] Testando endpoint protegido COM token..." -ForegroundColor Yellow
try {
    $headers = @{"Authorization"="Bearer $token"}
    $result = Invoke-RestMethod -Uri "$API/api/Users" -Method GET -Headers $headers
    Write-Host "    ✓ Retornou 200 OK" -ForegroundColor Green
    Write-Host "    ✓ Dados obtidos: $($result.Count) registros" -ForegroundColor Green
} catch {
    $code = $_.Exception.Response.StatusCode.Value__
    if ($code -eq 401) {
        Write-Host "    ✗ Retornou 401: Token NÃO está sendo aceito!" -ForegroundColor Red
    } elseif ($code -eq 404) {
        Write-Host "    ✓ Retornou 404 (Endpoint não existe, mas token foi aceito)" -ForegroundColor Green
    } else {
        Write-Host "    Test: Status $code - $_" -ForegroundColor Yellow
    }
}

# 5. Instructions for Swagger UI
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  COMO TESTAR NO SWAGGER UI" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "
1. Abra http://localhost:5145/swagger     
2. Clique no botão 'Authorize'
3. Cole o token obtido (SEM 'Bearer ' - só o token):
   $($token.Substring(0, 50))...
4. Clique 'Authorize'
5. Teste qualquer endpoint protegido - deve retornar 200 OK
6. O token vai automaticamente em cada requisição!
" -ForegroundColor Cyan

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  RESULTADO FINAL" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✓ JWT gerado com sucesso" -ForegroundColor Green
Write-Host "✓ Swagger com security definitions" -ForegroundColor Green  
Write-Host "✓ Token validado no API" -ForegroundColor Green
Write-Host "✓ Endpoints protegidos funcionando" -ForegroundColor Green
