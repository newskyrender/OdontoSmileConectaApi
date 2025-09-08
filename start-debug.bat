@echo off
REM Script para iniciar a API em modo debug
REM OdontoSmileConecta API Debug Mode

echo.
echo 🦷 Iniciando OdontoSmileConecta API em modo DEBUG...
echo.

REM Define variáveis de ambiente para debug
set ASPNETCORE_ENVIRONMENT=Development
set ASPNETCORE_DETAILEDERRORS=true
set ASPNETCORE_LOGGING__LOGLEVEL__DEFAULT=Debug

REM Navega para o diretório da API
cd /d "%~dp0src\services\Integration.Api"

echo 📂 Diretório atual: %CD%
echo 🔧 Ambiente: %ASPNETCORE_ENVIRONMENT%
echo 📊 Log Level: Debug
echo 🌐 URL da API: http://localhost:8080
echo 📝 Swagger: http://localhost:8080/swagger/index.html
echo.

echo 🔍 Verificando porta 8080...
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :8080') do (
    echo ⚠️  Encerrando processo %%a na porta 8080...
    taskkill /F /PID %%a >nul 2>&1
)

timeout /t 2 >nul

echo.
echo 🚀 Iniciando aplicação...
echo ⏹️  Para parar a aplicação, pressione Ctrl+C
echo.

REM Inicia a aplicação
dotnet run --project "Integration.Api.csproj" --launch-profile "Integration.Api" --verbosity detailed

pause
