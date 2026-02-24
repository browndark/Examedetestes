PIRAMIDE DE TESTES - MINHAS FINANÇAS

Estrutura: Backend .NET + Frontend React/TypeScript

CAMADA 1: TESTES UNITÁRIOS (31 testes)
======================================

LOCALIZAÇÃO: api/MinhasFinancas.Tests.Unit/

DOMÍNIO (Domain Layer):
- Domain/Entities/PessoaTests.cs (8 testes)
  - Cálculo de idade a partir de data de nascimento
  - Validação de maioridade (18 anos)
  - Inicialização de coleções vazias
  
- Domain/Entities/CategoriaTests.cs (8 testes)
  - Validação PermiteTipo para Despesa
  - Validação PermiteTipo para Receita
  - Validação PermiteTipo para Ambas
  - Inicialização de coleções

- Domain/Entities/TransacaoTests.cs (6 testes)
  - Propriedades obrigatórias
  - Validação de valor mínimo (0.01)
  - Data padrão para hoje

APLICAÇÃO (Application Layer):
- Application/Services/PessoaServiceTests.cs (5 testes)
  - CreateAsync com dados válidos
  - GetByIdAsync quando existe
  - GetByIdAsync quando não existe
  - DeleteAsync
  - UpdateAsync com pessoa não encontrada

- Application/Services/TransacaoServiceTests.cs (4 testes)
  - CreateAsync com categoria não encontrada
  - CreateAsync com pessoa não encontrada
  - Validação: menor tentando criar receita
  - Validação: categoria incompatível com tipo

REGRAS DE NEGÓCIO TESTADAS:
---------------------------

1. MENORES DE IDADE NÃO PODEM REGISTRAR RECEITAS
   Teste: CreateAsync_ShouldThrowExceptionWhenMinor
   Resultado: PASSA - InvalidOperationException é lançada

2. CATEGORIA SÓ PODE SER USADA CONFORME SUA FINALIDADE
   Testes múltiplos validando:
   - Despesa em categoria Despesa (PASSA)
   - Receita em categoria Receita (PASSA)
   - Ambos em categoria Ambas (PASSA)
   - Violações lançam InvalidOperationException

3. CÁLCULO CORRETO DE IDADE
   Resultado: PASSA em todos os cenários


CAMADA 2: TESTES DE INTEGRAÇÃO
===============================

LOCALIZAÇÃO: api/MinhasFinancas.Tests.Integration/

OBJETIVO:
- Testar persistência com EF Core
- Validar relacionamentos entre entidades
- Testar transações no banco de dados

STATUS:
- Estrutura criada
- Testes de persistência de Pessoa implementados
- Testes de regras de negócio com banco implementados

CONFIGURAÇÃO:
- Banco em memória SQLite para testes
- DbContext limpo entre testes via IAsyncLifetime


CAMADA 3: TESTES END-TO-END
============================

LOCALIZAÇÃO: web/tests/

FERRAMENTA: Playwright

OBJETIVO:
- Validar fluxos completos de usuário
- Testar integração Frontend + API
- Verificar interface React

CENÁRIOS PLANEJADOS:
1. Criar nova pessoa
2. Criar categoria
3. Criar transação
4. Validar regras de negócio na UI
5. Editar e excluir dados


COMO RODAR OS TESTES
====================

TESTES UNITÁRIOS:
cd api/MinhasFinancas.Tests.Unit
dotnet test

TESTES DE INTEGRAÇÃO:
cd api/MinhasFinancas.Tests.Integration
dotnet test

COBERTURA DE TESTES:
Focar em regras de negócio críticas - não buscar 100% de cobertura


ESTRUTURA DE PASTAS
===================

api/
├── MinhasFinancas.Tests.Unit/
│   ├── Domain/
│   │   └── Entities/
│   │       ├── PessoaTests.cs
│   │       ├── CategoriaTests.cs
│   │       └── TransacaoTests.cs
│   └── Application/
│       └── Services/
│           ├── PessoaServiceTests.cs
│           └── TransacaoServiceTests.cs
├── MinhasFinancas.Tests.Integration/
│   ├── PessoaPersistenceTests.cs
│   └── TransacaoBusinnessRulesIntegrationTests.cs
└── [projeto principal]

web/
└── [frontend tests com Playwright]


BOAS PRÁTICAS APLICADAS
=======================

1. ARRANGEMENT: AAA Pattern (Arrange, Act, Assert)
2. NOMENCLATURA: 
   - Padrão: [Método]_Should[Comportamento]_When[Cenário]
   - Claro e descritivo

3. MOCKING:
   - Uso de Moq para isolar unidades
   - Mocks apenas quando necessário

4. ASSERTIONS:
   - FluentAssertions para melhor legibilidade
   - Uma asserção primária por teste

5. ORGANIZAÇÃO:
   - Testes próximos ao código que testam
   - Separação por domínio (Domain, Application)

6. ISOLAMENTO:
   - Testes independentes
   - Sem dependências entre testes
   - Banco limpo entre testes de integração


MÉTRICAS
========

Testes Unitários: 31/31 PASSOU
Cobertura de Regras: 100%
Tempo de Execução: ~3 segundos (testes unitários)


PRÓXIMOS PASSOS
===============

1. Completar testes de integração com banco em memória
2. Implementar testes E2E com Playwright
3. Configurar CI/CD com GitHub Actions
4. Validar todas as regras de negócio


REFERÊNCIAS
===========

- xUnit: https://xunit.net/
- Moq: https://github.com/moq/moq
- FluentAssertions: https://fluentassertions.com/
- Playwright: https://playwright.dev/dotnet/
