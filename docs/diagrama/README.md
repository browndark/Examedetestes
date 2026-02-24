# Diagramas PlantUML - MinhasFinancas

Esta pasta contém diagramas do projeto em formato **PlantUML** (.puml). Você pode visualizar e gerar imagens desses diagramas de várias formas.

## Arquivos de Diagrama

### 1. `01-arquitetura-sistema.puml`
Visão geral da arquitetura em camadas (Clean Architecture):
- API Layer (Controllers, Middlewares)
- Application Layer (Services, DTOs, Mapping)
- Domain Layer (Entities, Interfaces, ValueObjects)
- Infrastructure Layer (Repositories, UnitOfWork, Data)

### 2. `02-classes-domain.puml`
Diagrama de classes do Domain (modelo de negócio):
- Pessoa, Categoria, Transacao
- Enums (EFinalidade, ETipo)
- ValueObject (PagedResult<T>)
- Relacionamentos entre entidades

### 3. `03-fluxo-criar-transacao.puml`
Sequência de criação de uma transação:
- Request HTTP
- Validação de DTO
- Mapeamento para Domain
- Persistência no banco
- Response

### 4. `04-fluxo-validacao-pessoa.puml`
Fluxo de validação de Pessoa:
- Validações obrigatórias (nome, email, data)
- Validações de range e formato
- Lógica de maior de idade

### 5. `05-fluxo-validacao-transacao.puml`
Fluxo de validação de Transacao:
- Campos obrigatórios
- Ranges de valores
- Validação de relacionamentos
- Permissão de categoria para tipo

### 6. `06-fluxo-validacao-categoria.puml`
Fluxo de validação de Categoria:
- Descrição obrigatória e range
- Finalidade válida
- Validações de enum

### 7. `07-cobertura-testes.puml`
Visão geral da cobertura de testes:
- 72 testes total
- Breakdownpor camada (Domain, Services, Validation)
- Resultado: 100% passing

### 8. `08-estrutura-testes.puml`
Organização da estrutura de testes:
- Pacotes de testes
- Fixtures e mocks
- Utilitários de teste

## Como Visualizar os Diagramas

### Opção 1: Online (PlantUML Online Editor)
1. Acesse: https://www.plantuml.com/plantuml/uml/
2. Copie o conteúdo do arquivo .puml
3. Cole no editor
4. Visualize em tempo real

### Opção 2: VS Code Extension
1. Instale a extensão "PlantUML" no VS Code
2. Abra um arquivo .puml
3. Clique em "Preview" ou use `Alt+D`

### Opção 3: Gerar Arquivos PNG/SVG
```bash
# Windows - usando Java (requer PlantUML instalado)
java -jar plantuml.jar docs/diagrama/*.puml

# Ou usando Docker
docker run --rm -v %cd%:/data think/plantuml -o /data/output docs/diagrama
```

### Opção 4: CLI Windows PowerShell
```powershell
# Se tiver PlantUML instalado
plantuml docs\diagrama\01-arquitetura-sistema.puml
```

## Sintaxe PlantUML Básica

```plantuml
' Componentes
component [Nome]

' Classes
class NomeClasse {
  - propriedade: tipo
  + metodo(): retorno
}

' Relações
classe1 --> classe2 : usa
classe1 -- classe2 : tem

' Pacotes
package "Nome" {
  ' conteúdo
}

' Atores/Participantes
actor "Nome"
participant "Nome"

' Estados
state "Nome" as alias
estado1 --> estado2 : condição
```

## Editar os Diagramas

Os arquivos .puml são texto puro, então você pode editá-los diretamente no VS Code. Qualquer mudança será refletida automaticamente na visualização se usar a extensão PlantUML.

## Recomendações

- Use a extensão PlantUML do VS Code para desenvolvimento rápido
- Mantenha os diagramas sincronizados com o código
- Documente mudanças significativas na arquitetura
- Use os diagramas como referência para onboarding de novos desenvolvedores

---

**Última atualização:** Fevereiro 23, 2026
