# Livora - Aplicação React

Aplicação React moderna para gerenciamento de aluguéis desenvolvida com Vite, TypeScript e integração com API REST.

## 🎯 Características

- **Landing Page** - Página inicial com informações sobre o Livora
- **Autenticação** - Sistema de login e registro de usuários
- **Dashboard** - Painel principal com dados consolidados
- **Integração com API** - Serviços completos para:
  - Propriedades
  - Inquilinos
  - Contratos
  - Cobranças
  - Pagamentos
  - Solicitações de Manutenção
  - Relatórios
  - Logs de Auditoria

## 🎨 Design

- **Paleta de Cores**
  - Primária: `#0081a7`
  - Secundária: `#00afb9`
  - Aceito Claro: `#fed9b7`
  - Aceito Perigo: `#f07167`
  - Fundo Claro: `#fdfcdc`

## 🛠️ Tecnologias

- **React 18** - Biblioteca de UI
- **TypeScript** - Tipagem estática
- **Vite** - Bundler rápido
- **React Router v6** - Roteamento
- **Axios** - Cliente HTTP
- **CSS3** - Estilização

## 📦 Instalação

```bash
# Instalar dependências
npm install

# Executar em desenvolvimento
npm run dev

# Build para produção
npm run build

# Pré-visualizar build
npm run preview
```

## 📁 Estrutura do Projeto

```
src/
├── components/
│   ├── Header.tsx
│   ├── Footer.tsx
│   └── ProtectedRoute.tsx
├── pages/
│   ├── Landing.tsx
│   ├── Login.tsx
│   ├── Register.tsx
│   └── Dashboard.tsx
├── services/
│   └── api.ts
├── context/
│   └── AuthContext.tsx
├── types/
│   └── api.ts
├── styles/
│   ├── global.css
│   └── components.css
├── App.tsx
└── main.tsx
```

## 🔗 Endpoints da API

### Autenticação
- `POST /api/Auth/login` - Login
- `POST /api/Auth/register` - Registrar novo usuário

### Usuários
- `GET /api/Users` - Listar todos
- `GET /api/Users/{id}` - Obter por ID
- `PUT /api/Users/{id}` - Atualizar
- `DELETE /api/Users/{id}` - Deletar

### Propriedades
- `GET /api/Properties` - Listar todas
- `GET /api/Properties/{id}` - Obter por ID
- `POST /api/Properties` - Criar
- `PUT /api/Properties/{id}` - Atualizar
- `DELETE /api/Properties/{id}` - Deletar

### Inquilinos
- `GET /api/Tenants` - Listar todos
- `GET /api/Tenants/{id}` - Obter por ID
- `POST /api/Tenants` - Criar
- `PUT /api/Tenants/{id}` - Atualizar
- `DELETE /api/Tenants/{id}` - Deletar

### Contratos
- `GET /api/Contracts` - Listar todos
- `GET /api/Contracts/{id}` - Obter por ID
- `POST /api/Contracts` - Criar
- `PUT /api/Contracts/{id}` - Atualizar
- `DELETE /api/Contracts/{id}` - Deletar

### Cobranças
- `GET /api/Billings` - Listar todas
- `GET /api/Billings/{id}` - Obter por ID
- `POST /api/Billings` - Criar
- `PUT /api/Billings/{id}` - Atualizar
- `DELETE /api/Billings/{id}` - Deletar

### Pagamentos
- `GET /api/Payments` - Listar todos
- `GET /api/Payments/{id}` - Obter por ID
- `POST /api/Payments` - Criar
- `PUT /api/Payments/{id}` - Atualizar
- `DELETE /api/Payments/{id}` - Deletar

### Solicitações de Manutenção
- `GET /api/MaintenanceRequests` - Listar todas
- `GET /api/MaintenanceRequests/{id}` - Obter por ID
- `POST /api/MaintenanceRequests` - Criar
- `PUT /api/MaintenanceRequests/{id}` - Atualizar
- `DELETE /api/MaintenanceRequests/{id}` - Deletar

### Dashboard
- `GET /api/Dashboard` - Obter dados do dashboard

### Relatórios
- `GET /api/Reports` - Obter relatórios

### Logs de Auditoria
- `GET /api/AuditLogs` - Obter logs de auditoria

## 🌐 Configuração da API

A aplicação está configurada para conectar à API em `http://localhost:5145`. Você pode alterar isso no arquivo `vite.config.ts` ou no arquivo `.env`.

## 🔐 Autenticação

O sistema usa JWT (JSON Web Tokens) para autenticação. Os tokens são armazenados no localStorage e enviados automaticamente em todas as requisições via header `Authorization: Bearer <token>`.

## 📱 Responsividade

A aplicação é totalmente responsiva e funciona bem em:
- Desktop (1920px+)
- Tablet (768px - 1024px)
- Mobile (até 768px)

## 🚀 Próximas Implementações

- [ ] Página de Propriedades completa
- [ ] Página de Inquilinos completa
- [ ] Página de Contratos completa
- [ ] Página de Cobranças completa
- [ ] Página de Pagamentos completa
- [ ] Página de Manutenção completa
- [ ] Página de Perfil de Usuário
- [ ] Relatórios avançados
- [ ] Sistema de notificações
- [ ] Dark mode

## 📝 Licença

Copyright © 2026 Livora. Todos os direitos reservados.
