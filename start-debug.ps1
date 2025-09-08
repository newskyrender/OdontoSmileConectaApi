# Script para iniciar a API em modo debug
# OdontoSmileConecta API Debug Mode

Write-Host "🦷 Iniciando OdontoSmileConecta API em modo DEBUG..." -ForegroundColor Green

# Define variáveis de ambiente para debug
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:ASPNETCORE_DETAILEDERRORS = "true"
$env:ASPNETCORE_LOGGING__LOGLEVEL__DEFAULT = "Debug"

# Navega para o diretório da API
$apiPath = ".\src\services\Integration.Api"
Set-Location $apiPath

Write-Host "📂 Diretório atual: $(Get-Location)" -ForegroundColor Yellow
Write-Host "🔧 Ambiente: $env:ASPNETCORE_ENVIRONMENT" -ForegroundColor Yellow
Write-Host "📊 Log Level: Debug" -ForegroundColor Yellow
Write-Host "🌐 URL da API: http://localhost:8080" -ForegroundColor Cyan
Write-Host "📝 Swagger: http://localhost:8080/swagger/index.html" -ForegroundColor Cyan
Write-Host "" 

# Verifica se existem processos dotnet rodando na porta 8080
Write-Host "🔍 Verificando porta 8080..." -ForegroundColor Yellow
$portCheck = netstat -ano | findstr :8080
if ($portCheck) {
    Write-Host "⚠️  Porta 8080 está em uso. Tentando liberar..." -ForegroundColor Red
    $processes = netstat -ano | findstr :8080 | ForEach-Object { ($_ -split '\s+')[-1] } | Sort-Object -Unique
    foreach ($processId in $processes) {
        if ($processId -and $processId -ne "0") {
            try {
                Stop-Process -Id $processId -Force -ErrorAction SilentlyContinue
                Write-Host "✅ Processo $processId encerrado" -ForegroundColor Green
            }
            catch {
                Write-Host "❌ Não foi possível encerrar o processo $processId" -ForegroundColor Red
            }
        }
    }
    Start-Sleep -Seconds 2
}

Write-Host "🚀 Iniciando aplicação..." -ForegroundColor Green
Write-Host "⏹️  Para parar a aplicação, pressione Ctrl+C" -ForegroundColor Yellow
Write-Host ""

# Inicia a aplicação
try {
    dotnet run --project "Integration.Api.csproj" --launch-profile "Integration.Api" --verbosity detailed
}
catch {
    Write-Host "❌ Erro ao iniciar a aplicação: $_" -ForegroundColor Red
    Read-Host "Pressione Enter para sair"
}
