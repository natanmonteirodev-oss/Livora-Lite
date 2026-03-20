$API = "http://localhost:5145"
$swag = Invoke-RestMethod -Uri "$API/swagger/v1/swagger.json"
$json = $swag | ConvertTo-Json -Depth 50

Write-Host "Swagger Security Definition Check" -ForegroundColor Cyan
Write-Host "==================================`n"

Write-Host "1. securitySchemes defined:" -ForegroundColor Yellow
if ($json -match '"securitySchemes"') { Write-Host "   YES" -ForegroundColor Green } else { Write-Host "   NO" -ForegroundColor Red }

Write-Host "2. Bearer scheme exists:" -ForegroundColor Yellow  
if ($json -match '"Bearer".*"type":"http"') { Write-Host "   YES" -ForegroundColor Green } else { Write-Host "   NO" -ForegroundColor Red }

Write-Host "3. Operation-level security requirements:" -ForegroundColor Yellow
$securityCount = ([regex]::Matches($json, '"security":\s*\[\s*\{\s*"Bearer"')).Count
Write-Host "   Found $securityCount operations with security requirements" -ForegroundColor $(if($securityCount -gt 10){"Green"}else{"Yellow"})

Write-Host "4. Sample from document (first 2000 chars):" -ForegroundColor Yellow
Write-Host $($json.Substring(0, 2000))`n

Write-Host "Checking paths section..." -ForegroundColor Yellow
$pathsMatch = $json | Select-String -Pattern '"paths":\{' | Measure-Object
if ($pathsMatch.Count -gt 0) { Write-Host "   Paths section: FOUND" -ForegroundColor Green }
