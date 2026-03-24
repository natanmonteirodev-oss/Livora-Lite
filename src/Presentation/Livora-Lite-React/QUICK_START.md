# 🚀 Guia de Uso Rápido - Livora React App

## 1️⃣ Instalação e Execução

### Opção 1: Via NPM
```bash
cd src/Presentation/Livora-Lite-React
npm install
npm run dev
```

### Opção 2: Via Script (Windows)
```bash
cd src/Presentation/Livora-Lite-React
start.bat
```

### Opção 3: Via Script (Linux/Mac)
```bash
cd src/Presentation/Livora-Lite-React
chmod +x start.sh
./start.sh
```

## 2️⃣ Acessar a Aplicação

```
🌐 URL: http://localhost:5173
🔌 API: http://localhost:5145 (precisa estar rodando)
```

## 3️⃣ Fluxo de Uso

### 📍 Passo 1: Landing Page
- Acesse `http://localhost:5173`
- Veja informações sobre o Livora
- Clique em "Começar Agora" ou "Registre-se"

### 📍 Passo 2: Criar Conta (Register)
- **Email**: seu@email.com
- **Primeira Nome**: Seu
- **Sobrenome**: Nome
- **Senha**: seu_password123
- **Confirmar Senha**: seu_password123
- Clique em "Registrar"

### 📍 Passo 3: Fazer Login
- **Email**: seu@email.com
- **Senha**: seu_password123
- Clique em "Entrar"

### 📍 Passo 4: Dashboard
- Veja resumo com 4 cards:
  - Total de Propriedades
  - Total de Inquilinos
  - Renda Total
  - Pagamentos Pendentes
- Menu de ações rápidas

## 4️⃣ Explorar Funcionalidades CRUD

### 🏠 Propriedades (`/properties`)
1. Clique em **"Gestão"** → **"Propriedades"**
2. Clique em **"+ Nova Propriedade"**
3. Preencha o formulário:
   - Endereço: Rua das Flores, 100
   - Tipo: Apartamento
   - Status: Disponível
   - Detalhes: (opcional)
4. Clique em **"Criar Propriedade"**
5. Veja a propriedade na tabela
6. Use **Editar** ou **Deletar** conforme necessário

### 👥 Inquilinos (`/tenants`)
1. Clique em **"Gestão"** → **"Inquilinos"**
2. Clique em **"+ Novo Inquilino"**
3. Preencha:
   - Nome Completo: João Silva
   - Email: joao@email.com
   - Telefone: (11) 99999-9999
4. Clique em **"Criar Inquilino"**

### 📋 Contratos (`/contracts`)
1. Clique em **"Gestão"** → **"Contratos"**
2. Clique em **"+ Novo Contrato"**
3. Preencha:
   - Data de Início: 01/01/2024
   - Data de Término: 31/12/2024
   - Valor do Aluguel: 1500.00
   - Status: Ativo
4. Clique em **"Criar Contrato"**

### 💳 Cobranças (`/billings`)
1. Clique em **"Financeiro"** → **"Cobranças"**
2. Clique em **"+ Nova Cobrança"**
3. Preencha:
   - Valor: 1500.00
   - Data de Vencimento: 05/01/2024
   - Status: Pendente
4. Clique em **"Criar Cobrança"**

### 💰 Pagamentos (`/payments`)
1. Clique em **"Financeiro"** → **"Pagamentos"**
2. Clique em **"+ Novo Pagamento"**
3. Preencha:
   - Valor: 1500.00
   - Data do Pagamento: (hoje)
   - Método: Transferência Bancária
4. Clique em **"Criar Pagamento"**

### 🔧 Manutenção (`/maintenance`)
1. Clique em **"Manutenção"**
2. Clique em **"+ Nova Solicitação"**
3. Preencha:
   - Descrição: Reparo no telhado
   - Prioridade: Alta
   - Status: Aberta
4. Clique em **"Criar Solicitação"**

### 📊 Relatórios (`/reports`)
1. Clique em **"Financeiro"** → **"Relatórios"**
2. Veja 6 cards de relatórios disponíveis
3. Clique em **"Gerar"** em qualquer card
4. Ou use os filtros abaixo:
   - Data Inicial
   - Data Final
   - Tipo de Relatório

## 5️⃣ Recursos de Cada CRUD

### Tabelas
- ✅ Listagem responsiva
- ✅ Dados formatados automaticamente
- ✅ Badges coloridos para status
- ✅ Paginação (futuro)

### Formulários
- ✅ Validação de campos obrigatórios
- ✅ Formatação automática (valores, datas)
- ✅ Modal responsivo
- ✅ Mensagens de erro claras

### Ações
- ✅ Editar - Abre modal com dados existentes
- ✅ Deletar - Com confirmação de segurança
- ✅ Criar - Modal vazio para novo registro

## 6️⃣ Cores e Status

### Badges de Status:
- 🟢 **Verde** (Disponível, Ativo, Pago) - Sucesso
- 🔵 **Azul** (Em Andamento) - Informação
- 🟡 **Amarelo** (Pendente, Médio) - Aviso
- 🔴 **Vermelho** (Expirado, Alta, Deletado) - Perigo

## 7️⃣ Dicas Úteis

### ⚡ Atalhos
- `Ctrl+K` - Busca (futuro)
- `Esc` - Fechar modal
- `Enter` - Enviar formulário

### 🎨 Temas
- Cores personalizadas da Livora
- Dark mode (futuro)
- Modo acessibilidade

### 💾 Dados Locais
- Token JWT salvo em localStorage
- Informações de usuário em localStorage
- Limpos ao fazer logout

## 8️⃣ Troubleshooting

### Erro 404 na API
- Certifique-se que a API está rodando em `http://localhost:5145`
- Verifique o arquivo `vite.config.ts` para o proxy

### Página em branco
- Limpe o cache: `Ctrl+Shift+Delete`
- Recarregue: `F5` ou `Ctrl+R`

### Erro de autenticação
- Faça logout e login novamente
- Limpe localStorage: Dev Tools → Application → localStorage → Clear

### Formulário não envia
- Verifique se todos os campos obrigatórios estão preenchidos
- Procure pela mensagem de erro em vermelho

## 9️⃣ Compilação e Deploy

### Build para Produção
```bash
npm run build
```
Gera pasta `dist/` pronta para deploy

### Preview do Build
```bash
npm run preview
```
Acesse `http://localhost:4173`

### Deploy
- GitHub Pages
- Vercel
- Netlify
- Sua hospedagem

## 🔟 Próximos Passos

1. **Implementar exportação de relatórios**
   - PDF
   - Excel
   - CSV

2. **Adicionar gráficos**
   - Chart.js
   - Recharts
   - D3.js

3. **Melhorar dashboard**
   - Gráficos de tendências
   - Alertas
   - Widgets customizáveis

4. **Mobile app**
   - React Native
   - Tecnologias offline

5. **Integração com outras APIs**
   - Pagamento (Stripe, PayPal)
   - Email (Mailgun, SendGrid)
   - SMS (Twilio)

## 📞 Suporte

- **Documentação**: Veja `README.md` e `CRUD_DOCUMENTATION.md`
- **Código**: Explore os comentários no código
- **Issues**: Registre problemas no GitHub
- **Discussões**: Participe das discussões da comunidade

---

**Criado com ❤️ para facilitar a gestão de aluguéis no Brasil**
