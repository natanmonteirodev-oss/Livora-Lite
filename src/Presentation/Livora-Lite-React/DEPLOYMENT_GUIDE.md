# 🚀 Guia de Deploy e Próximos Passos

## 1️⃣ Deploy Local (Desenvolvimento)

### Pré-requisitos
- Node.js 18+ instalado
- API Livora rodando em `http://localhost:5145`
- Porta 5173 disponível

### Iniciar o servidor de desenvolvimento

```bash
# Opção 1: Usando npm
npm run dev

# Opção 2: Usando script Windows
start.bat

# Opção 3: Usando script Linux/Mac
bash start.sh
```

Acesse: [http://localhost:5173](http://localhost:5173)

---

## 2️⃣ Deploy em Produção

### Build para Produção
```bash
npm run build
```

Outputs:
- `dist/index.html` - Arquivo principal
- `dist/assets/` - CSS e JS minificados
- Tamanho total: ~300 KB (com gzip)

### Opções de Deploy

#### **Opção A: Vercel** (Recomendado)
```bash
# 1. Instalar CLI
npm install -g vercel

# 2. Deploy
vercel

# 3. Seguir prompts
```

#### **Opção B: Netlify**
```bash
# 1. Instalar CLI
npm install -g netlify-cli

# 2. Deploy
netlify deploy --prod --dir=dist

# 3. Configurar domínio
```

#### **Opção C: IIS (Windows Server)**
1. Build: `npm run build`
2. Copiar pasta `dist/` para servidor IIS
3. Configurar redirect para `index.html`
4. Definir MIME type para `.js` files

#### **Opção D: Docker**
```dockerfile
FROM node:18-alpine as build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

Buildar e rodar:
```bash
docker build -t livora-react .
docker run -p 80:80 livora-react
```

---

## 3️⃣ Configuração para Produção

### Variáveis de Ambiente

Criar `.env` na raiz do projeto:
```env
VITE_API_URL=https://sua-api.com/api
VITE_APP_NAME=Livora
VITE_APP_VERSION=1.0.0
```

Usar em código:
```typescript
const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5145/api'
```

### CORS Configuration

Adicionar ao backend (.NET):
```csharp
app.UseCors(builder =>
    builder
        .WithOrigins("http://localhost:5173", "https://seu-dominio.com")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
);
```

### HTTPS/SSL

Para ambiente de produção:
```typescript
// src/services/api.ts
const BASE_URL = process.env.NODE_ENV === 'production' 
  ? 'https://api.seu-dominio.com/api'
  : 'http://localhost:5145/api'
```

---

## 🔧 Melhorias Imediatas

### 1. Adicionar Environment Variables
```bash
npm install dotenv
```

Criar arquivo `.env.example`:
```env
VITE_API_URL=http://localhost:5145/api
VITE_LOG_LEVEL=debug
```

### 2. Implementar Paginação
```typescript
// src/hooks/usePagination.ts
export const usePagination = (items: any[], pageSize = 10) => {
  const [currentPage, setCurrentPage] = useState(1)
  const totalPages = Math.ceil(items.length / pageSize)
  const startIdx = (currentPage - 1) * pageSize
  
  return {
    pagedItems: items.slice(startIdx, startIdx + pageSize),
    currentPage,
    totalPages,
    setCurrentPage
  }
}
```

### 3. Adicionar Busca/Filtro
```typescript
// src/hooks/useSearch.ts
export const useSearch = (items: any[], searchKey: string) => {
  const [searchTerm, setSearchTerm] = useState('')
  
  const filtered = items.filter(item =>
    item[searchKey]?.toLowerCase().includes(searchTerm.toLowerCase())
  )
  
  return { filtered, searchTerm, setSearchTerm }
}
```

### 4. Implementar Toast Notifications
```bash
npm install sonner
```

```typescript
import { Toaster, toast } from 'sonner'

export default function App() {
  const handleSuccess = () => {
    toast.success('Operação realizada com sucesso!')
  }
  
  return (
    <>
      <Toaster />
      {/* ... */}
    </>
  )
}
```

---

## 📊 Adicionar Gráficos (Dashboard)

### Opção 1: Recharts (Recomendado)
```bash
npm install recharts
```

```typescript
import { LineChart, Line, XAxis, YAxis } from 'recharts'

export function ChartExample() {
  const data = [
    { month: 'Jan', revenue: 4000 },
    { month: 'Feb', revenue: 3000 },
    { month: 'Mar', revenue: 2000 }
  ]
  
  return (
    <LineChart width={500} height={300} data={data}>
      <XAxis dataKey="month" />
      <YAxis />
      <Line type="monotone" dataKey="revenue" stroke="#0081a7" />
    </LineChart>
  )
}
```

### Opção 2: Chart.js
```bash
npm install react-chartjs-2 chart.js
```

---

## 📁 Exportar Relatórios

### PDF (usando pdfkit)
```bash
npm install pdfkit
```

```typescript
import PDFDocument from 'pdfkit'

export const generatePDF = (data: any) => {
  const doc = new PDFDocument()
  const stream = fs.createWriteStream('report.pdf')
  
  doc.pipe(stream)
  doc.fontSize(25).text('Relatório Livora')
  doc.fontSize(12).text(JSON.stringify(data, null, 2))
  doc.end()
}
```

### Excel (usando xlsx)
```bash
npm install xlsx
```

```typescript
import * as XLSX from 'xlsx'

export const exportToExcel = (data: any[], filename: string) => {
  const ws = XLSX.utils.json_to_sheet(data)
  const wb = XLSX.utils.book_new()
  XLSX.utils.book_append_sheet(wb, ws, 'Sheet1')
  XLSX.writeFile(wb, `${filename}.xlsx`)
}
```

---

## 🧪 Adicionar Testes

### Install Vitest
```bash
npm install -D vitest @testing-library/react @testing-library/jest-dom
```

### Test Example
```typescript
// src/services/__tests__/api.test.ts
import { describe, it, expect } from 'vitest'
import { login } from '../api'

describe('API Service', () => {
  it('should login with valid credentials', async () => {
    const result = await login('test@email.com', 'password')
    expect(result).toHaveProperty('token')
  })
})
```

### Run Tests
```bash
npm run test
npm run test:ui  # UI mode
npm run test:coverage  # Coverage
```

---

## 🔍 Verificação de Produção

### Performance
```bash
npm run preview
# Simula ambiente de produção localmente
```

### Checklist de Deploy
- [ ] Variáveis de ambiente configuradas
- [ ] API URL correta
- [ ] CORS habilitado
- [ ] SSL/HTTPS ativo
- [ ] Build testado localmente
- [ ] Logs monitorados
- [ ] Backup da base de dados
- [ ] CDN configurado (opcional)
- [ ] Rate limiting habilitado
- [ ] Backup automático agendado

---

## 📝 Monitoramento em Produção

### Sentry (Error Tracking)
```bash
npm install @sentry/react @sentry/tracing
```

```typescript
import * as Sentry from '@sentry/react'

Sentry.init({
  dsn: import.meta.env.VITE_SENTRY_DSN,
  environment: import.meta.env.MODE,
  tracesSampleRate: 1.0
})
```

### Google Analytics
```bash
npm install react-ga4
```

```typescript
import ReactGA from 'react-ga4'

ReactGA.initialize(import.meta.env.VITE_GA_ID)
```

---

## 🔐 Segurança

### Content Security Policy
Adicione ao `index.html`:
```html
<meta http-equiv="Content-Security-Policy" 
      content="default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'">
```

### Proteção contra XSS
Nunca use `dangerouslySetInnerHTML`:
```typescript
// ❌ NUNCA
<div dangerouslySetInnerHTML={{ __html: userInput }} />

// ✅ SEMPRE
<div>{userInput}</div>
```

### Validação de Formulários
```bash
npm install zod react-hook-form
```

---

## 📈 Escalabilidade Futura

### SPA para PWA
```json
{
  "name": "Livora",
  "icons": [
    { "src": "/logo.png", "sizes": "192x192", "type": "image/png" }
  ],
  "theme_color": "#0081a7",
  "background_color": "#ffffff",
  "display": "standalone"
}
```

### State Management
Para aplicações maiores, considere:
- Zustand (simples)
- Redux Toolkit (complexo)
- Jotai (moderno)

### Backend Queries
Para otimizar, considere:
- React Query (data fetching)
- SWR (data fetching + caching)
- Apollo Client (GraphQL)

---

## 📞 Suporte e Troubleshooting

### Erro: "Cannot find module 'react'"
```bash
npm install
npm ci  # Instalar versões exatas
```

### Erro: "API returning 401"
- [ ] Verificar JWT token
- [ ] Confirmar token em localStorage
- [ ] Validar CORS settings
- [ ] Checar URL da API

### Erro: "Blank page after deploy"
- [ ] Verificar `public/` folder
- [ ] Validar build output em `dist/`
- [ ] Confirmar `index.html` está sendo servido
- [ ] Checar console do browser (F12)

---

## ✅ Checklist de Deploy

```
Pré-deploy:
☐ npm install
☐ npm run build (sem erros)
☐ npm run preview (testar produção)
☐ .env configurado
☐ API URL correta
☐ CORS habilitado

Deploy:
☐ Build artifacts criados
☐ Subir para servidor
☐ Domínio/DNS apontado
☐ SSL certificado ativo
☐ Testar em navegadores

Pós-deploy:
☐ Verificar console (F12)
☐ Testar login/logout
☐ Testar CRUD operations
☐ Verificar responsividade
☐ Monitorar erros (Sentry)
☐ Coletar feedback
```

---

## 🎓 Recursos Úteis

- [Vite Documentation](https://vitejs.dev)
- [React Documentation](https://react.dev)
- [TypeScript Handbook](https://www.typescriptlang.org/docs)
- [Axios Documentation](https://axios-http.com)
- [React Router Documentation](https://reactrouter.com)
- [MDN Web Docs](https://developer.mozilla.org)

---

**Status:** ✅ Aplicação pronta para deploy  
**Última atualização:** Março 2026  
**Versão:** 1.0.0  

Desenvolvido com ❤️ para o projeto Livora-Lite
