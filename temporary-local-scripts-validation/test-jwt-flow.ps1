#!/usr/bin/env pwsh

<#
.SYNOPSIS
Test JWT authentication flow and Bearer token transmission
#>

$API_URL = "http://localhost:5145"
$LOGIN_EMAIL = "admin@livora.com"
$LOGIN_PASSWORD = "@d1mn"

Write-Host "======================================" -ForegroundColor Cyan
Write-Host "JWT Authentication Flow Test"
Write-Host "======================================" -ForegroundColor Cyan

# Step 1: Login
Write-Host "`n[1/3] Attempting login..." -ForegroundColor Yellow
try {
    $loginPayload = @{
        email = $LOGIN_EMAIL
        password = $LOGIN_PASSWORD
    } | ConvertTo-Json

    $response = Invoke-RestMethod -Uri "$API_URL/api/Auth/login" `
        -Method POST `
        -ContentType "application/json" `
        -Body $loginPayload `
        -ErrorAction Stop

    if ($response.token) {
        Write-Host "✓ Login successful!" -ForegroundColor Green
        Write-Host "Token (first 50 chars): $($response.token.Substring(0, [Math]::Min(50, $response.token.Length)))..." -ForegroundColor Gray
        Write-Host "Token length: $($response.token.Length) chars" -ForegroundColor Gray
        
        $token = $response.token
    } else {
        Write-Host "✗ Login failed: No token in response" -ForegroundColor Red
        Write-Host "Response: $($response | ConvertTo-Json)" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "✗ Login request failed: $_" -ForegroundColor Red
    exit 1
}

# Step 2: Check Swagger document
Write-Host "`n[2/3] Checking Swagger document for security definitions..." -ForegroundColor Yellow
try {
    $swaggerJson = Invoke-RestMethod -Uri "$API_URL/swagger/v1/swagger.json" `
        -Method GET `
        -ErrorAction Stop

    # Convert to string for searching
    $swaggerStr = $swaggerJson | ConvertTo-Json -Depth 50

    if ($swaggerStr -match '"securitySchemes"') {
        Write-Host "✓ securitySchemes found" -ForegroundColor Green
    } else {
        Write-Host "✗ securitySchemes NOT found" -ForegroundColor Red
    }

    if ($swaggerStr -match '"Bearer"') {
        Write-Host "✓ Bearer scheme found" -ForegroundColor Green
    } else {
        Write-Host "✗ Bearer scheme NOT found" -ForegroundColor Red
    }

    if ($swaggerStr -match '"security".*"Bearer"') {
        Write-Host "✓ Security requirements found on operations" -ForegroundColor Green
    } else {
        Write-Host "⚠ Security requirements on operations not detected" -ForegroundColor Yellow
    }
} catch {
    Write-Host "⚠ Could not check Swagger: $_" -ForegroundColor Yellow
}

# Step 3: Test protected endpoint WITH token
Write-Host "`n[3/3] Testing protected endpoint with Bearer token..." -ForegroundColor Yellow
try {
    $headers = @{
        "Authorization" = "Bearer $token"
        "Content-Type" = "application/json"
    }

    # Try to access a protected endpoint
    $response = Invoke-RestMethod -Uri "$API_URL/api/Dashboard/admin" `
        -Method GET `
        -Headers $headers `
        -ErrorAction Stop

    Write-Host "✓ Protected endpoint accessible!" -ForegroundColor Green
    Write-Host "Response status: Success (200)" -ForegroundColor Green
    Write-Host "Response preview: $($response | ConvertTo-Json -Depth 2 | Select-Object -First 5)" -ForegroundColor Gray
} catch {
    $statusCode = $_.Exception.Response.StatusCode.Value__
    if ($statusCode -eq 401) {
        Write-Host "✗ 401 Unauthorized - Token not accepted!" -ForegroundColor Red
        Write-Host "Issue: Bearer token may not be transmitted correctly by the middleware" -ForegroundColor Red
    } elseif ($statusCode -eq 404) {
        Write-Host "⚠ 404 Not Found - Endpoint may not exist, trying /api/Property..." -ForegroundColor Yellow
        try {
            $response = Invoke-RestMethod -Uri "$API_URL/api/Property" `
                -Method GET `
                -Headers $headers `
                -ErrorAction Stop
            Write-Host "✓ Alternative endpoint works!" -ForegroundColor Green
        } catch {
            Write-Host "✗ Also failed: $_" -ForegroundColor Red
        }
    } else {
        Write-Host "✗ Request failed with status $statusCode : $_" -ForegroundColor Red
    }
}

Write-Host "`n======================================" -ForegroundColor Cyan
Write-Host "Test Complete"
Write-Host "======================================" -ForegroundColor Cyan
