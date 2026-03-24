# Documentação das Páginas CRUD

Este documento descreve todas as páginas CRUD implementadas na aplicação React Livora-Lite.

## 📑 Índice

1. [Propriedades](#propriedades)
2. [Inquilinos](#inquilinos)
3. [Contratos](#contratos)
4. [Cobranças](#cobranças)
5. [Pagamentos](#pagamentos)
6. [Manutenção](#manutenção)
7. [Relatórios](#relatórios)

---

## 🏠 Propriedades

**Rota:** `/properties`

Página para gerenciar todas as propriedades imobiliárias.

### Funcionalidades:
- ✅ **Listar** todas as propriedades em uma tabela responsiva
- ✅ **Criar** nova propriedade via modal
- ✅ **Editar** propriedade existente
- ✅ **Deletar** propriedade com confirmação

### Campos do Formulário:
- **Endereço** (obrigatório) - Rua, número e bairro
- **Tipo de Propriedade** (obrigatório) - Apartamento, Casa, Comercial, Terreno
- **Status** (obrigatório) - Disponível, Alugado, Manutenção, Indisponível
- **Detalhes** (opcional) - Descrição adicional da propriedade

### Colunas da Tabela:
- Endereço (40%)
- Tipo (20%)
- Status (com badge de cor) (20%)
- Detalhes (20%)

---

## 👥 Inquilinos

**Rota:** `/tenants`

Página para gerenciar informações dos inquilinos.

### Funcionalidades:
- ✅ **Listar** todos os inquilinos
- ✅ **Criar** novo inquilino
- ✅ **Editar** dados do inquilino
- ✅ **Deletar** inquilino
- ✅ **Email clicável** para enviar correspondências

### Campos do Formulário:
- **Nome Completo** (obrigatório)
- **Email** (obrigatório) - Validação de email
- **Telefone** (obrigatório) - Formato de telefone

### Colunas da Tabela:
- Nome (35%)
- Email (35%) - Link clicável
- Telefone (30%)

---

## 📋 Contratos

**Rota:** `/contracts`

Página para gerenciar contratos de aluguel.

### Funcionalidades:
- ✅ **Listar** todos os contratos
- ✅ **Criar** novo contrato
- ✅ **Editar** contrato existente
- ✅ **Deletar** contrato
- ✅ **Formatação automática** de datas e valores

### Campos do Formulário:
- **Data de Início** (obrigatório) - Seletor de data
- **Data de Término** (obrigatório) - Seletor de data
- **Valor do Aluguel** (obrigatório) - Em reais, aceita decimais
- **Status** (obrigatório) - Ativo, Expirado, Encerrado

### Colunas da Tabela:
- Data de Início (20%) - Formato DD/MM/YYYY
- Data de Término (20%) - Formato DD/MM/YYYY
- Valor (20%) - Formatado em R$ com 2 casas decimais
- Status (20%) - Com badge de cor

---

## 💳 Cobranças

**Rota:** `/billings`

Página para gerenciar cobranças de aluguel.

### Funcionalidades:
- ✅ **Listar** todas as cobranças
- ✅ **Criar** nova cobrança
- ✅ **Editar** cobrança existente
- ✅ **Deletar** cobrança
- ✅ **Formatação de valores** em reais

### Campos do Formulário:
- **Valor** (obrigatório) - Em reais
- **Data de Vencimento** (obrigatório)
- **Status** (obrigatório) - Pendente, Pago, Vencido

### Colunas da Tabela:
- Valor (25%) - Formatado em R$
- Vencimento (25%) - Formato DD/MM/YYYY
- Status (25%) - Com badge (verde para pago, amarelo para pendente, vermelho para vencido)

---

## 💰 Pagamentos

**Rota:** `/payments`

Página para registrar e acompanhar pagamentos.

### Funcionalidades:
- ✅ **Listar** todos os pagamentos
- ✅ **Criar** novo pagamento
- ✅ **Editar** pagamento existente
- ✅ **Deletar** pagamento
- ✅ **Múltiplos métodos** de pagamento

### Campos do Formulário:
- **Valor** (obrigatório) - Em reais
- **Data do Pagamento** (obrigatório) - Preenchido com data atual por padrão
- **Método de Pagamento** (obrigatório):
  - Cartão de Crédito
  - Cartão de Débito
  - Transferência Bancária
  - Dinheiro
  - Cheque

### Colunas da Tabela:
- Valor (25%) - Formatado em R$
- Data do Pagamento (25%) - Formato DD/MM/YYYY
- Método (25%)

---

## 🔧 Manutenção

**Rota:** `/maintenance`

Página para gerenciar solicitações de manutenção.

### Funcionalidades:
- ✅ **Listar** todas as solicitações
- ✅ **Criar** nova solicitação
- ✅ **Editar** solicitação existente
- ✅ **Deletar** solicitação
- ✅ **Priorização** de tarefas
- ✅ **Acompanhamento** de status

### Campos do Formulário:
- **Descrição** (obrigatório) - Detalhes da solicitação
- **Prioridade** (obrigatório):
  - Baixa
  - Média
  - Alta
- **Status** (obrigatório):
  - Aberta
  - Em Andamento
  - Concluída
  - Cancelada

### Colunas da Tabela:
- Descrição (35%)
- Prioridade (20%) - Com badge de cor (vermelho=alta, amarelo=média, verde=baixa)
- Status (20%) - Com badge de cor
- Data de Criação (25%) - Formato DD/MM/YYYY

---

## 📊 Relatórios

**Rota:** `/reports`

Página para gerar e visualizar relatórios consolidados.

### Tipos de Relatórios:
1. **Relatório Financeiro** 💰
   - Resumo de renda
   - Despesas totais
   - Fluxo de caixa

2. **Relatório de Propriedades** 🏠
   - Status de ocupação
   - Lista de propriedades
   - Disponibilidade

3. **Relatório de Inquilinos** 👥
   - Informações dos inquilinos
   - Histórico de contratos
   - Pagamentos por inquilino

4. **Relatório de Contratos** 📋
   - Contratos ativos
   - Contratos expirados
   - Próximos vencimentos

5. **Relatório de Cobranças** 💳
   - Cobranças pendentes
   - Cobranças recebidas
   - Valor total em aberto

6. **Relatório de Manutenção** 🔧
   - Solicitações abertas
   - Histórico de manutenção
   - Custos totais

### Filtros:
- **Data Inicial** - Seletor de data
- **Data Final** - Seletor de data
- **Tipo de Relatório** - Dropdown com opções

### Funcionalidades:
- ✅ **Gerar** relatório com filtros personalizados
- ✅ **Exportar** dados (futuro)
- ✅ **Interface intuitiva** com cards informativos

---

## 🎨 Componentes Reutilizáveis

### Modal Component
```tsx
<Modal
  isOpen={boolean}
  title="Título do Modal"
  onClose={() => {}}
  size="small" | "medium" | "large"
>
  {children}
</Modal>
```

### Table Component
```tsx
<Table<T>
  columns={[
    { key: 'field', label: 'Rótulo', width: '20%' },
    { key: 'field', label: 'Campo', render: (value) => <span>{value}</span> }
  ]}
  data={data}
  loading={false}
  onRowClick={(row) => {}}
  actions={(row) => <button>Ação</button>}
/>
```

---

## 🔗 Navegação entre Páginas

A aplicação possui menu lateral/dropdown com acesso rápido:

**Header Navigation:**
- Dashboard
- Gestão (Propriedades, Inquilinos, Contratos)
- Financeiro (Cobranças, Pagamentos, Relatórios)
- Manutenção
- Perfil

---

## ⚡ Funcionalidades Gerais

### Todas as páginas CRUD possuem:
- ✅ Tabela responsiva com dados
- ✅ Modal para criação/edição
- ✅ Botões de ação (Editar, Deletar)
- ✅ Confirmação antes de deletar
- ✅ Mensagens de erro/sucesso
- ✅ Loading state durante requisições
- ✅ Formatação automática de dados (datas, valores)
- ✅ Design consistente com paleta de cores
- ✅ Responsividade para mobile e tablet

### Tratamento de Erros:
- Validação de formulários
- Mensagens de erro claras
- Requisições com timeout
- Autenticação com JWT

---

## 📱 Responsividade

Todas as páginas são responsivas e otimizadas para:
- **Desktop** (1920px+)
- **Tablet** (768px - 1024px)
- **Mobile** (até 768px)

---

## 🚀 Próximas Melhorias

- [ ] Exportar relatórios em PDF
- [ ] Filtros avançados nas tabelas
- [ ] Paginação para grandes volumes
- [ ] Busca em tempo real
- [ ] Múltiplas seleções na tabela
- [ ] Ordenação por coluna
- [ ] Permissões por perfil
- [ ] Auditoria de alterações
- [ ] Notificações em tempo real
- [ ] Integração com gráficos

---

## 🔐 Segurança

Todas as rotas CRUD são protegidas:
- Requerem autenticação (JWT token)
- Redirecionam para login se não autenticado
- Token expirado redireciona automaticamente
- Confirmação para ações destrutivas

---

## 📞 Suporte

Para dúvidas sobre as páginas CRUD, consulte:
- [README.md](README.md) - Documentação geral
- [src/types/api.ts](src/types/api.ts) - Tipos de dados
- [src/services/api.ts](src/services/api.ts) - Métodos da API
