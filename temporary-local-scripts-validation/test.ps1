$API = "http://localhost:5145"
Write-Host "Testing JWT auth..." -ForegroundColor Cyan

Write-Host "Logging in..." -ForegroundColor Yellow
$resp = Invoke-RestMethod -Uri "$API/api/Auth/login" -Method POST -Body (@{email="admin@livora.com";password="@d1mn"}|ConvertTo-Json) -ContentType "application/json"
$token = $resp.token
Write-Host "Token: $($token.Substring(0, 30))..." -ForegroundColor Green
Write-Host "Token Claims: $(($token.Split('.')[1] | ForEach-Object {[System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String(($_+'====').Substring(0, $_.Length + 4 - $_.Length % 4)))}) | ConvertFrom-Json | ConvertTo-Json -Compress)" -ForegroundColor Gray

Write-Host "Testing protected endpoint..." -ForegroundColor Yellow
$headers = @{"Authorization"="Bearer $token"}

Write-Host "Trying GET /api/Tenant..." -ForegroundColor Yellow
$result = Invoke-RestMethod -Uri "$API/api/Tenant" -Method GET -Headers $headers
Write-Host "Success: 200 OK" -ForegroundColor Green
Write-Host "Result count: $($result.Count) items"
