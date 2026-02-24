# Diagramas PlantUML - MinhasFinancas

Esta pasta contém diagramas do projeto em formato **PlantUML** (.puml). Você pode visualizar e gerar imagens desses diagramas de várias formas.

## Arquivos de Diagrama

### 1. `arquitetura-sistema.puml`
Visão geral da arquitetura em camadas (Clean Architecture):
- API Layer (Controllers, Middlewares)
- Application Layer (Services, DTOs, Mapping)
- Domain Layer (Entities, Interfaces, ValueObjects)
- Infrastructure Layer (Repositories, UnitOfWork, Data)

### 2. `classes-domain.puml`
Diagrama de classes do Domain (modelo de negócio):
- Pessoa, Categoria, Transacao
- Enums (EFinalidade, ETipo)
- ValueObject (PagedResult<T>)
- Relacionamentos entre entidades

### 3. `fluxo-criar-transacao.puml`
Sequência de criação de uma transação:
- Request HTTP
- Validação de DTO
- Mapeamento para Domain
- Persistência no banco
- Response

### 4. `fluxo-validacao-transacao.puml`
Fluxo de validação de Transacao:
- Campos obrigatórios
- Ranges de valores
- Validação de relacionamentos
- Permissão de categoria para tipo

### 5. `fluxo-validacao-categoria.puml`
Fluxo de validação de Categoria:
- Descrição obrigatória e range
- Finalidade válida
- Validações de enum

### 6. `cobertura-testes.puml`
Visão geral da cobertura de testes:
- 72 testes total
- Breakdownpor camada (Domain, Services, Validation)
- Resultado: 100% passing

### 7. `estrutura-testes.puml`
Organização da estrutura de testes:
- Pacotes de testes
- Fixtures e mocks
- Utilitários de teste


