@echo off
REM Script para iniciar a aplicação React no Windows

echo Iniciando Livora React Application...
echo.

REM Verificar se o Node.js está instalado
where node >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo X Node.js nao esta instalado. Por favor, instale Node.js primeiro.
    pause
    exit /b 1
)

echo Versao do Node.js:
node --version
echo Versao do npm:
npm --version
echo.

REM Instalar dependências se necessário
if not exist node_modules (
    echo [*] Instalando dependencias...
    call npm install
    echo.
)

REM Iniciar a aplicação em modo desenvolvimento
echo [+] Iniciando servidor de desenvolvimento...
echo [*] A aplicacao estara disponivel em: http://localhost:5173
echo.
echo [!] Certifique-se de que a API esta rodando em: http://localhost:5145
echo.
echo Pressione Ctrl+C para parar o servidor
echo.

call npm run dev
pause
