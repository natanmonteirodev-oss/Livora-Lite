#!/bin/bash

# Script para iniciar a aplicação React
echo "Iniciando Livora React Application..."
echo ""

# Verificar se o Node.js está instalado
if ! command -v node &> /dev/null; then
    echo "❌ Node.js não está instalado. Por favor, instale Node.js primeiro."
    exit 1
fi

echo "✓ Node.js versão: $(node --version)"
echo "✓ npm versão: $(npm --version)"
echo ""

# Instalar dependências se necessário
if [ ! -d "node_modules" ]; then
    echo "📦 Instalando dependências..."
    npm install
    echo ""
fi

# Iniciar a aplicação em modo desenvolvimento
echo "🚀 Iniciando servidor de desenvolvimento..."
echo "📍 A aplicação estará disponível em: http://localhost:5173"
echo ""
echo "⚠️  Certifique-se de que a API está rodando em: http://localhost:5145"
echo ""
echo "Pressione Ctrl+C para parar o servidor"
echo ""

npm run dev
