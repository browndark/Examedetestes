GUIA DE EXECUÇÃO DE TESTES

Projeto: MinhasFinancas - Sistema de Controle de Gastos
Último Update: 23 de Fevereiro de 2026


PRÉ-REQUISITOS
==============

- .NET 9.0 SDK ou posterior
- Visual Studio Code ou Visual Studio 2022
- Git (opcional)


ESTRUTURA DO PROJETO
====================

ExameDesenvolvedorDeTestes/
├── api/
│   ├── MinhasFinancas.API/              (Projeto da API)
│   ├── MinhasFinancas.Application/      (Lógica de negócio)
│   ├── MinhasFinancas.Domain/           (Entidades e interfaces)
│   ├── MinhasFinancas.Infrastructure/   (Acesso a dados)
│   └── MinhasFinancas.Tests.Unit/       (Testes unitários) ✓ PRONTO
│       └── MinhasFinancas.Tests.Unit.csproj
├── web/                                 (Frontend React)
└── data/                                (Dados e scripts)


COMO EXECUTAR OS TESTES
=======================

OPÇÃO 1: Linha de Comando
-------------------------

Abra o PowerShell/Terminal e navegue até a pasta api/:

    cd c:\Users\bruno\Downloads\ExameDesenvolvedorDeTestes\api

Para executar TODOS os testes:

    dotnet test

Para executar APENAS testes unitários:

    dotnet test MinhasFinancas.Tests.Unit

Para executar com informações detalhadas:

    dotnet test --verbosity=detailed

Para executar um teste específico:

    dotnet test --filter "TestClassName"


OPÇÃO 2: Visual Studio Code
----------------------------

1. Abra o workspace em VS Code
2. Instale a extensão "C# Dev Kit"
3. Abra Explorer (Ctrl+Shift+E)
4. Localize "MinhasFinancas.Tests.Unit"
5. Clique direito → "Run Tests" ou "Run Test in Explorer"


OPÇÃO 3: Visual Studio 2022
----------------------------

1. Abra a solução MinhasFinancas.slnx
2. Abra o painel "Test Explorer" (Test → Test Explorer)
3. Clique "Run All Tests" ou selecione testes individuais


ENTENDENDO OS TESTES
====================

TESTES UNITÁRIOS IMPLEMENTADOS
Total: 31 testes
Status: TODOS PASSANDO ✓


Área 1: TESTES DE ENTIDADE PESSOA (8 testes)
─────────────────────────────────────────────
Arquivo: MinhasFinancas.Tests.Unit/Domain/Entities/PessoaTests.cs

Testes implementados:
  ✓ Pessoa_EhMaiorDeIdade_RetornaTrue_QuandoTem18Anos
  ✓ Pessoa_EhMaiorDeIdade_RetornaFalse_QuandoTem17Anos
  ✓ Pessoa_EhMaiorDeIdade_RetornaTrue_QuandoTem20Anos
  ✓ Pessoa_GetIdade_RetornaIdadeCorreta
  ✓ Pessoa_Constructor_InicializaTransacoesVazio
  ✓ Pessoa_EhMaiorDeIdade_RetornaTrue_QuandoMaiorDe30
  ✓ Pessoa_PropriedadesBasicas_SaoPreenchidas
  ✓ Pessoa_EhMaiorDeIdade_CalculaCorretamente

Valida: A regra "menores não podem registrar receitas" depende do cálculo 
correto de maioridade. Estes testes garantem que Pessoa.EhMaiorDeIdade() 
funciona para todas as faixas etárias críticas.


Área 2: TESTES DE ENTIDADE CATEGORIA (8 testes)
────────────────────────────────────────────────
Arquivo: MinhasFinancas.Tests.Unit/Domain/Entities/CategoriaTests.cs

Testes implementados:
  ✓ Categoria_PermiteTipo_RetornaTrue_ParaDespesa_QuandoFinalizadeEDespesa
  ✓ Categoria_PermiteTipo_RetornaFalse_ParaDespesa_QuandoFinalidadeEReceita
  ✓ Categoria_PermiteTipo_RetornaTrue_ParaReceita_QuandoFinalidadeEReceita
  ✓ Categoria_PermiteTipo_RetornaFalse_ParaReceita_QuandoFinalidadeEDespesa
  ✓ Categoria_PermiteTipo_RetornaTrue_ParaEspecialista_QuandoFinalidadeEAmbas
  ✓ Categoria_PropriedadesBasicas_SaoPreenchidas
  ✓ Categoria_PermiteTipo_RetornaTrue_ParaDespesa_QuandoFinalidadeEAmbas
  ✓ Categoria_PermiteTipo_RetornaTrue_ParaReceita_QuandoFinalidadeEAmbas

Valida: A regra "categorias respeitam sua finalidade". Se categoria é 
"DESPESA", não pode registrar RECEITA. Se é "AMBAS", aceita qualquer tipo.


Área 3: TESTES DE ENTIDADE TRANSACAO (6 testes)
────────────────────────────────────────────────
Arquivo: MinhasFinancas.Tests.Unit/Domain/Entities/TransacaoTests.cs

Testes implementados:
  ✓ Transacao_Constructor_CriaComSucesso
  ✓ Transacao_ValorMinimo_EZeroVirgula01
  ✓ Transacao_DataTransacao_InicializaComHoje
  ✓ Transacao_PropriedadesBasicas_SaoPreenchidas
  ✓ Transacao_Valor_NaoPodeSerNegativo
  ✓ Transacao_Valor_NaoPodeSerZero

Valida: A estrutura da transação está correta. Valor mínimo é 0.01.
Data padrão é hoje. Propriedades obrigatórias.


Área 4: TESTES DE SERVIÇO PESSOA (5 testes)
─────────────────────────────────────────────
Arquivo: MinhasFinancas.Tests.Unit/Application/Services/PessoaServiceTests.cs

Testes implementados:
  ✓ PessoaService_CreateAsync_CommandoValidoDeveCriar
  ✓ PessoaService_GetByIdAsync_DeveRetornarPessoa
  ✓ PessoaService_GetByIdAsync_RetornaNullQuandoNaoEncontrada
  ✓ PessoaService_DeleteAsync_DeveRemover
  ✓ PessoaService_CreateAsync_LancaExcecaoQuandoCPFNull

Valida: O serviço PessoaService CRUD funciona corretamente. Criação, 
leitura, exclusão. Validação de CPF obrigatório.


Área 5: TESTES DE SERVIÇO TRANSACAO (4 testes)
───────────────────────────────────────────────
Arquivo: MinhasFinancas.Tests.Unit/Application/Services/TransacaoServiceTests.cs

Testes implementados:
  ✓ TransacaoService_CreateAsync_LancaExcecao_QuandoPessoaNaoEncontrada
  ✓ TransacaoService_CreateAsync_LancaExcecao_QuandoCategoriaNaoEncontrada
  ✓ TransacaoService_CreateAsync_LancaExcecao_QuandoMenorTentaARegistrarReceita
  ✓ TransacaoService_CreateAsync_LancaExcecao_QuandoCategoriaETypeMismatched

Valida: As TRÊS REGRAS DE NEGÓCIO principais:
  1. Pessoa deve existir (não pode ser nula)
  2. Categoria deve existir (não pode ser nula)
  3. Menores não podem registrar receita (regra crítica)
  4. Categoria deve aceitar o tipo de transação


COMO INTERPRETAR OS RESULTADOS
===============================

Saída esperada ao rodar os testes:

    > dotnet test
    
    Descobrindo testes... PessoaTests...CategoriaTests...
    [xUnit.net 00:00:01.234] 31 testes encontrados (0.5 ms)
    
    ✓ PessoaTests.cs (8 testes, 0.15s)
    ✓ CategoriaTests.cs (8 testes, 0.12s)
    ✓ TransacaoTests.cs (6 testes, 0.08s)
    ✓ PessoaServiceTests.cs (5 testes, 0.28s)
    ✓ TransacaoServiceTests.cs (4 testes, 0.22s)
    
    ════════════════════════════════════════════════════════
    Aprovado 31 testes... Tempo total: 2,79 segundos
    ════════════════════════════════════════════════════════


SE UM TESTE FALHAR
==================

1. Leia a mensagem de erro completa
2. Procure no código do teste qual é a asserção que falhou
3. Verifique a implementação da classe que está sendo testada
4. Compare com os requisitos de negócio

Exemplo:
    
    ❌ TransacaoService_CreateAsync_LancaExcecao_QuandoMenorTentaARegistrarReceita
    
    Error: Expected an exception of type InvalidOperationException
    but no exception was thrown.

Isto significa que ao criar uma transação com um menor registrando receita,
o sistema não está jogando a exceção esperada. Verifique:
- Pessoa.EhMaiorDeIdade() está funcionando?
- Transacao.Pessoa setter está sendo acionado?


RODANDO TESTES COM MAIS DETALHES
================================

Para ver qual teste está falhando e por quê:

    dotnet test --verbosity=detailed --logger="console;verbosity=detailed"

Para ver apenas os testes que falharam:

    dotnet test --filter "Passed=False"

Para rodar um teste específico:

    dotnet test --filter "TransacaoService_CreateAsync"


ESTRUTURA DOS MOCKS
===================

Os testes unitários usam Moq para mockar dependências:

Exemplo (PessoaServiceTests.cs):

    var mockUnitOfWork = new Mock<IUnitOfWork>();
    var mockPessoaRepository = new Mock<IRepository<Pessoa>>();
    
    mockUnitOfWork
        .Setup(u => u.PessoaRepository)
        .Returns(mockPessoaRepository.Object);
    
    var service = new PessoaService(mockUnitOfWork.Object);

Isso permite testar PessoaService em isolamento, sem dependência de banco
de dados real.


ESTRUTURA DOS ASSERTS
=====================

Todos os testes usam FluentAssertions para legibilidade:

Exemplo:

    action
        .Should()
        .ThrowExactly<InvalidOperationException>()
        .WithMessage("Menores de 18 anos não podem registrar receitas");

Isto é muito mais legível que Assert.Throws<>().


PRÓXIMOS PASSOS
===============

Os 31 testes unitários cobrem as regras críticas do sistema.

Para melhorar a qualidade ainda mais:

1. Adicionar testes de integração (com banco de dados real)
2. Adicionar testes E2E (com Playwright, frontend)
3. Adicionar testes de performance
4. Adicionar CI/CD com GitHub Actions


DUVIDAS?
========

Consulte os arquivos README.md na raiz do projeto ou
a documentação RELATORIO_TESTES.md e PIRAMIDE_TESTES.md.
