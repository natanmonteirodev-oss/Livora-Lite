$url = 'http://localhost:5145/swagger/v1/swagger.json'

try {
    $response = (New-Object System.Net.WebClient).DownloadString($url)
    Write-Host "Documento baixado: $($response.Length) bytes"
    
    # Teste 1: securitySchemes
    if ($response.Contains('securitySchemes')) {
        Write-Host '[OK] securitySchemes ENCONTRADO no documento'
    } else {
        Write-Host '[ERRO] securitySchemes NAO ENCONTRADO'
    }
    
    # Teste 2: Bearer
    if ($response.Contains('Bearer')) {
        Write-Host '[OK] Bearer ENCONTRADO'
    } else {
        Write-Host '[ERRO] Bearer NAO ENCONTRADO'
    }
    
    # Teste 3: http scheme
    if ($response.Contains('"type":"http"')) {
        Write-Host '[OK] Type HTTP encontrado'
    } else {
        Write-Host '[ERRO] Type HTTP nao encontrado'
    }
    
    # Mostrar parte do documento
    Write-Host "`nPrimeiros 1500 caracteres:"
    Write-Host $response.Substring(0, 1500)
}
catch {
    Write-Host "[ERRO] $_"
}
