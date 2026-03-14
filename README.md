# Livora-Lite

Projeto desenvolvido com arquitetura em camadas.

## Estrutura do Projeto

```
src/
├── Application/
│   ├── DTO/
│   ├── Interface/
│   ├── Mappers/
│   ├── Services/
│   └── Validators/
├── CrossCutting/
├── Domain/
│   ├── Entities/
│   └── Interfaces/
├── Infrastructure/
│   ├── Mapping/
│   ├── Migrations/
│   └── Persistence/
└── Presentation/
```

## Descrição das Camadas

### 📂 **Application**
Camada de aplicação responsável pela lógica de negócio da aplicação.

- **DTO/**: Data Transfer Objects - objetos para transferência de dados entre camadas
- **Interface/**: Interfaces de contrato da aplicação
- **Mappers/**: Mapeadores para conversão entre DTOs e entidades
- **Services/**: Serviços que implementam regras de negócio
- **Validators/**: Validadores de entrada de dados

### 📂 **CrossCutting**
Camada transversal com funcionalidades compartilhadas (logging, caching, etc.)

### 📂 **Domain**
Camada de domínio contendo as entidades e interfaces de negócio.

- **Entities/**: Classes de entidade que representam os objetos de negócio
- **Interfaces/**: Contratos e interfaces de domínio

### 📂 **Infrastructure**
Camada de infraestrutura responsável por acesso a dados e recursos externos.

- **Mapping/**: Configurações de mapeamento de dados
- **Migrations/**: Migrations de banco de dados
- **Persistence/**: Implementação de repositórios e contexto de dados

### 📂 **Presentation**
Camada de apresentação responsável pela interface com o usuário.

---
### Esquema de cores:

- #0081a7 (Primaria, backgounds, etc)
- #00afb9 (Cards, PopUps, etc)
- #fdfcdc 
- #fed9b7
- #f07167

---

## 🌿 LIVORA — BRANDING & CONCEITO

Tudo sobre seu aluguel, em um só lugar

Livora nasce para simplificar a relação entre pessoas e imóveis, trazendo clareza, controle e confiança para o aluguel de longa duração no Brasil.

Enquanto grandes players focam em intermediação ou estadias temporárias, a Livora foca no dia a dia do aluguel, na gestão contínua e na visão completa do imóvel, respeitando a realidade regional, burocrática e operacional brasileira.

### 🎯 Propósito (Por quê existimos?)

Facilitar a vida de quem aluga, de quem é dono e de quem administra imóveis.

A Livora existe para reduzir ruídos, centralizar informações e transformar o aluguel em um processo mais transparente, previsível e organizado para todos os envolvidos.

### 🧭 Posicionamento

Livora é a plataforma brasileira de gestão de aluguéis de longa duração, que conecta locatários, locadores e empresas em um único ecossistema simples e confiável.

### 🧩 Público-alvo (3 pilares claros)

**👤 Locatário**

- Controle de despesas (aluguel, taxas, reajustes)
- Acesso fácil a contrato, datas, histórico e comunicados
- Transparência e previsibilidade

**🏠 Locador**

- Centralização de imóveis, contratos e recebimentos
- Visão geral da carteira de aluguéis
- Base para integrações futuras (governo, prestadores, serviços)

**🏢 Empresas / Consultores**

- Gestão de múltiplos imóveis e clientes
- Organização operacional
- Escalabilidade e visão consolidada

### 🧠 Personalidade da marca

A Livora não é fria nem burocrática. Ela é:

- **Clara** → informação fácil, sem ruído
- **Confiável** → dados organizados, histórico e rastreabilidade
- **Próxima** → fala simples, sem juridiquês excessivo
- **Moderna** → tecnologia a favor do cotidiano
- **Brasileira** → entende regras, hábitos e desafios locais

### 🧱 Estrutura do Produto (Branding dos módulos)

**🔹 Livora Home (Locatário)**

Tudo sobre seu aluguel, sem complicação. Função emocional: segurança e controle

**🔹 Livora Owner (Locador)**

Seus imóveis organizados, seus aluguéis sob controle. Função emocional: tranquilidade e visão

**🔹 Livora Pro (Empresas / Consultores)**

Gestão profissional de imóveis, do seu jeito. Função emocional: escala e eficiência

---

**Desenvolvido por:** Natan  
**Data:** 2026
