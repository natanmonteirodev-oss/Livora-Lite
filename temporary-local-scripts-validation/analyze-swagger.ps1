$url = 'http://localhost:5145/swagger/v1/swagger.json'

$response = (New-Object System.Net.WebClient).DownloadString($url)

# Encontrar seções principais
Write-Host "Analisando estrutura do documento..."
Write-Host ""

$sections = @(
    '"components"',
    '"schemas"',
    '"paths"',
    '"securitySchemes"',
    '"servers"',
    '"info"'
)

foreach ($section in $sections) {
    if ($response.Contains($section)) {
        $idx = $response.IndexOf($section)
        Write-Host "[OK] $section encontrado na posicao $idx"
    } else {
        Write-Host "[NAO] $section nao encontrado"
    }
}

# Procurar pela posição de "components"
$componentsIdx = $response.IndexOf('"components"')
if ($componentsIdx -ge 0) {
    Write-Host ""
    Write-Host "Seção 'components' encontrada. Mostrando contexto:"
    Write-Host $response.Substring($componentsIdx, 600)
}
