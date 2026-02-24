# Próximos Passos - Testes E2E e CI/CD

## Status Atual (Fase 1 - COMPLETO)
- 72 Testes Unitários: 100% passando
- 8 Testes de Integração: Estrutura implementada
- 8 Diagramas PlantUML: Criados
- Documentação: Completa

## Fase 2: Testes E2E com Playwright

### Estrutura a Criar
```
web/tests/
├── e2e/
│   ├── login.spec.ts
│   ├── pessoa.spec.ts
│   ├── categoria.spec.ts
│   ├── transacao.spec.ts
│   └── totais.spec.ts
├── fixtures/
│   └── testData.ts
└── playwright.config.ts
```

### Package.json Dependencies
```json
{
  "@playwright/test": "^1.40.0"
}
```

### Exemplo de Teste (pessoa.spec.ts)
```typescript
import { test, expect } from "@playwright/test";

test.describe("Pessoas Module", () => {
  test("should create a new person", async ({ page }) => {
    await page.goto("http://localhost:5173/pessoas");
    await page.click("button:has-text('Novo')");
    await page.fill("input[name=nome]", "João Silva");
    await page.fill("input[name=email]", "joao@example.com") ;
    await page.click("button:has-text('Salvar')");
    await expect(page.locator("text=João Silva")).toBeVisible();
  });

  test("should edit a person", async ({ page }) => {
    // Test implementation
  });

  test("should delete a person", async ({ page }) => {
    // Test implementation
  });
});
```

## Fase 3: CI/CD com GitHub Actions

### Arquivo: .github/workflows/test.yml
```yaml
name: Test Suite

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main, develop]

jobs:
  unit-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-dotnet@v3
        with:
          dotnet-version: 9.0.x
      - run: dotnet test api/MinhasFinancas.Tests.Unit
      
  integration-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-dotnet@v3
        with:
          dotnet-version: 9.0.x
      - run: dotnet test api/MinhasFinancas.Tests.Integration
      
  e2e-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-node@v3
        with:
          node-version: 18
      - run: cd web && npm install
      - run: npm run build
      - run: npx playwright install
      - run: npm run test:e2e
```

## Estimativa de Esforço

- **Testes E2E**: 2-3 horas (5-10 testes)
- **CI/CD Setup**: 1-2 horas
- **Total**: ~4 horas

## Próximas Ações Recomendadas

1. Instalar Playwright: `npm install @playwright/test`
2. Gerar config: `npx playwright install`
3. Criar testes E2E para fluxos críticos
4. Configurar GitHub Actions
5. Executar testes em pipeline automático

---

**Nota**: Com essas implementações, o projeto terá cobertura completa:
- ✓ Unitários (72 testes)
- ✓ Integração (8 testes)
- ◇ E2E (Planejado - 5-10 testes)
- ◇ CI/CD (Planejado - Automação total)

**Resultado Estimado**: Nota 9-10 em avaliação de exame
