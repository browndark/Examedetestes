using Xunit;
using Moq;
using FluentAssertions;
using MinhasFinancas.Application.Services;
using MinhasFinancas.Application.DTOs;
using MinhasFinancas.Domain.Entities;
using MinhasFinancas.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace MinhasFinancas.Tests.Unit.Application.Services;

public class TransacaoServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly TransacaoService _transacaoService;
    private readonly DateTime _today = DateTime.Today;

    public TransacaoServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _transacaoService = new TransacaoService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowExceptionWhenCategoriaNotFound()
    {
        // Arrange
        var dto = new CreateTransacaoDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = Transacao.ETipo.Despesa,
            Data = _today,
            CategoriaId = Guid.NewGuid(),
            PessoaId = Guid.NewGuid()
        };

        _mockUnitOfWork.Setup(x => x.Categorias.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Categoria)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _transacaoService.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowExceptionWhenPessoaNotFound()
    {
        // Arrange
        var categoriaId = Guid.NewGuid();
        var pessoaId = Guid.NewGuid();

        var categoria = new Categoria
        {
            Id = categoriaId,
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        var dto = new CreateTransacaoDto
        {
            Descricao = "Teste",
            Valor = 100m,
            Tipo = Transacao.ETipo.Despesa,
            Data = _today,
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };

        _mockUnitOfWork.Setup(x => x.Categorias.GetByIdAsync(categoriaId))
            .ReturnsAsync(categoria);
        _mockUnitOfWork.Setup(x => x.Pessoas.GetByIdAsync(pessoaId))
            .ReturnsAsync((Pessoa)null!);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _transacaoService.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowExceptionWhenMinorTriesToCreateReceita()
    {
        // Arrange - Critical business rule test
        var categoriaId = Guid.NewGuid();
        var pessoaId = Guid.NewGuid();

        var categoriaReceita = new Categoria
        {
            Id = categoriaId,
            Descricao = "Salário",
            Finalidade = Categoria.EFinalidade.Receita
        };

        var menorDeIdade = new Pessoa
        {
            Id = pessoaId,
            Nome = "Pedro Jovem",
            DataNascimento = DateTime.Today.AddYears(-15)
        };

        var dto = new CreateTransacaoDto
        {
            Descricao = "Mesada",
            Valor = 50m,
            Tipo = Transacao.ETipo.Receita,
            Data = _today,
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };

        _mockUnitOfWork.Setup(x => x.Categorias.GetByIdAsync(categoriaId))
            .ReturnsAsync(categoriaReceita);
        _mockUnitOfWork.Setup(x => x.Pessoas.GetByIdAsync(pessoaId))
            .ReturnsAsync(menorDeIdade);

        // Act & Assert - Validates business rule enforcement
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _transacaoService.CreateAsync(dto));
        
        exception.Message.Should().Contain("Menores de 18 anos não podem registrar receitas");
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowExceptionWhenCategoryDoesNotAllowTransactionType()
    {
        // Arrange - Tests category-transaction type compatibility
        var categoriaId = Guid.NewGuid();
        var pessoaId = Guid.NewGuid();

        var categoriaDespesa = new Categoria
        {
            Id = categoriaId,
            Descricao = "Alimentação",
            Finalidade = Categoria.EFinalidade.Despesa
        };

        var pessoa = new Pessoa
        {
            Id = pessoaId,
            Nome = "Roberto",
            DataNascimento = DateTime.Today.AddYears(-40)
        };

        var dto = new CreateTransacaoDto
        {
            Descricao = "Ganho inesperado",
            Valor = 200m,
            Tipo = Transacao.ETipo.Receita,
            Data = _today,
            CategoriaId = categoriaId,
            PessoaId = pessoaId
        };

        _mockUnitOfWork.Setup(x => x.Categorias.GetByIdAsync(categoriaId))
            .ReturnsAsync(categoriaDespesa);
        _mockUnitOfWork.Setup(x => x.Pessoas.GetByIdAsync(pessoaId))
            .ReturnsAsync(pessoa);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _transacaoService.CreateAsync(dto));
        
        exception.Message.Should().Contain("receita em categoria de despesa");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTransacaoWhenExists()
    {
        // Arrange
        var transacaoId = Guid.NewGuid();

        var transacao = new Transacao
        {
            Id = transacaoId,
            Descricao = "Compras",
            Valor = 150m,
            Tipo = Transacao.ETipo.Despesa,
            Data = _today
        };

        _mockUnitOfWork.Setup(x => x.Transacoes.GetByIdAsync(transacaoId))
            .ReturnsAsync(transacao);

        // Act
        var result = await _transacaoService.GetByIdAsync(transacaoId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(transacaoId);
        result.Descricao.Should().Be("Compras");
        result.Valor.Should().Be(150m);
        result.Data.Should().Be(_today);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNullWhenNotExists()
    {
        // Arrange
        var transacaoId = Guid.NewGuid();

        _mockUnitOfWork.Setup(x => x.Transacoes.GetByIdAsync(transacaoId))
            .ReturnsAsync((Transacao)null!);

        // Act
        var result = await _transacaoService.GetByIdAsync(transacaoId);

        // Assert
        result.Should().BeNull();
    }
}
