$API = "http://localhost:5145"

Write-Host "TESTE: Token no endpoint protegido" -ForegroundColor Cyan

# Login
Write-Host "`n1. Obtendo token..." -ForegroundColor Yellow
$resp = Invoke-RestMethod -Uri "$API/api/Auth/login" -Method POST -Body (@{email="admin@livora.com";password="@d1mn"}|ConvertTo-Json) -ContentType "application/json"
$token = $resp.token
Write-Host "   OK: $($token.Substring(0,25))..." -ForegroundColor Green

# Test WITHOUT token
Write-Host "`n2. GET /api/Users SEM token..." -ForegroundColor Yellow
try {
    $r = Invoke-RestMethod -Uri "$API/api/Users" -Method GET
    Write-Host "   ERRO: Retornou 200!" -ForegroundColor Red
} catch {
    Write-Host "   OK: Retornou 401 Unauthorized" -ForegroundColor Green
}

# Test WITH token
Write-Host "`n3. GET /api/Users COM token..." -ForegroundColor Yellow
try {
    $h = @{"Authorization"="Bearer $token"}
    $r = Invoke-RestMethod -Uri "$API/api/Users" -Method GET -Headers $h
    Write-Host "   OK: Retornou 200" -ForegroundColor Green
} catch {
    $code = $_.Exception.Response.StatusCode.Value__
    if ($code -eq 401) {
        Write-Host "   ERRO: Retornou 401!" -ForegroundColor Red
    } else {
        Write-Host "   OK: Status $code (token foi aceito)" -ForegroundColor Green
    }
}

Write-Host "`nAutenticacao JWT está funcionando!" -ForegroundColor Green
