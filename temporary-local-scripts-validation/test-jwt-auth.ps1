# Test JWT authentication flow
$API_URL = "http://localhost:5145"
$email = "admin@livora.com"
$password = "@d1mn"

Write-Host "=== JWT Auth Test ===" -ForegroundColor Cyan

# Login
Write-Host "`n[1] Attempting login..." -ForegroundColor Yellow
$loginBody = @{ email = $email; password = $password } | ConvertTo-Json
$loginResp = Invoke-RestMethod -Uri "$API_URL/api/Auth/login" -Method POST -ContentType "application/json" -Body $loginBody

if ($loginResp.token) {
    Write-Host "✓ Login successful!" -ForegroundColor Green
    $token = $loginResp.token
    Write-Host "Token length: $($token.Length) chars"
} else {
    Write-Host "✗ Login failed" -ForegroundColor Red
    exit
}

# Test protected endpoint
Write-Host "`n[2] Testing protected endpoint..." -ForegroundColor Yellow
$headers = @{ Authorization = "Bearer $token" }

try {
    $result = Invoke-RestMethod -Uri "$API_URL/api/Property" -Method GET -Headers $headers
    Write-Host "✓ Success! (200 OK)" -ForegroundColor Green
} catch {
    $code = $_.Exception.Response.StatusCode.Value__
    Write-Host "✗ Failed with status $code" -ForegroundColor Red
    if ($code -eq 401) {
        Write-Host "→ Token not being transmitted correctly!" -ForegroundColor Red
    }
}

Write-Host "`n=== Test Complete ===" -ForegroundColor Cyan
